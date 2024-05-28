using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;

namespace WpfApp1
{
    public partial class AddQuizWindow : Window
    {
        private DatabaseManager databaseManager;
        private JsonManager jsonManager;
        private int quizId;
        private QuestionViewModel selectedQuestion;

        public ObservableCollection<QuestionViewModel> Questions { get; set; }

        public AddQuizWindow(int newQuizId, string quizName)
        {
            this.Title = quizName;
            databaseManager = new DatabaseManager();
            jsonManager = new JsonManager();
            quizId = newQuizId;

            Questions = new ObservableCollection<QuestionViewModel>();

            InitializeComponent();
            QuestionsListBox.ItemsSource = Questions;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BrowseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            var newQuestion = new QuestionViewModel
            {
                QuestionText = QuestionTextBox.Text,
                AnswerA = AnswerATextBox.Text,
                AnswerB = AnswerBTextBox.Text,
                AnswerC = AnswerCTextBox.Text,
                AnswerD = AnswerDTextBox.Text,
                CorrectAnswer = ((ComboBoxItem)CorrectAnswerComboBox.SelectedItem)?.Content.ToString(),
                ImagePath = ImagePathTextBox.Text
            };

            var question = new Question(newQuestion.QuestionText, newQuestion.ImagePath);

            var answers = new List<Answer>
            {
                new Answer(newQuestion.AnswerA, newQuestion.CorrectAnswer == "A"),
                new Answer(newQuestion.AnswerB, newQuestion.CorrectAnswer == "B"),
                new Answer(newQuestion.AnswerC, newQuestion.CorrectAnswer == "C"),
                new Answer(newQuestion.AnswerD, newQuestion.CorrectAnswer == "D")
            };

            int questionID = databaseManager.AddQuestion(question, answers, quizId);
            if (questionID != -1)
            {
                newQuestion.QuestionID = questionID;
                Questions.Add(newQuestion);
            }

            ClearQuestionInputs();
        }

        private void UpdateQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedQuestion != null)
            {
                selectedQuestion.QuestionText = QuestionTextBox.Text;
                selectedQuestion.AnswerA = AnswerATextBox.Text;
                selectedQuestion.AnswerB = AnswerBTextBox.Text;
                selectedQuestion.AnswerC = AnswerCTextBox.Text;
                selectedQuestion.AnswerD = AnswerDTextBox.Text;
                selectedQuestion.CorrectAnswer = ((ComboBoxItem)CorrectAnswerComboBox.SelectedItem)?.Content.ToString();
                selectedQuestion.ImagePath = ImagePathTextBox.Text;

                var question = new Question(selectedQuestion.QuestionText, selectedQuestion.ImagePath)
                {
                    ImagePath = selectedQuestion.ImagePath
                };

                var answers = new List<Answer>
                {
                    new Answer(selectedQuestion.AnswerA, selectedQuestion.CorrectAnswer == "A"),
                    new Answer(selectedQuestion.AnswerB, selectedQuestion.CorrectAnswer == "B"),
                    new Answer(selectedQuestion.AnswerC, selectedQuestion.CorrectAnswer == "C"),
                    new Answer(selectedQuestion.AnswerD, selectedQuestion.CorrectAnswer == "D")
                };

                databaseManager.UpdateQuestion(selectedQuestion.QuestionID, question, answers);

                Questions[Questions.IndexOf(selectedQuestion)] = selectedQuestion;
                QuestionsListBox.Items.Refresh();

                ClearQuestionInputs();
            }
        }

        private void DeleteQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is QuestionViewModel question)
            {
                Questions.Remove(question);
                databaseManager.RemoveQuestion(question.QuestionID);
            }
        }

        private void CreateNewQuiz_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Quiz toegevoegd en opgeslagen in de database!");
            this.DialogResult = true;
            this.Close();
        }

        private void QuestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (QuestionsListBox.SelectedItem is QuestionViewModel selectedQuestion)
            {
                this.selectedQuestion = selectedQuestion;
                QuestionTextBox.Text = selectedQuestion.QuestionText;
                AnswerATextBox.Text = selectedQuestion.AnswerA;
                AnswerBTextBox.Text = selectedQuestion.AnswerB;
                AnswerCTextBox.Text = selectedQuestion.AnswerC;
                AnswerDTextBox.Text = selectedQuestion.AnswerD;
                ImagePathTextBox.Text = selectedQuestion.ImagePath;
                CorrectAnswerComboBox.Text = selectedQuestion.CorrectAnswer;



                AddQuestionButton.Visibility = Visibility.Collapsed;
                UpdateQuestionButton.Visibility = Visibility.Visible;
            }
        }

        private void ClearQuestionInputs()
        {
            QuestionTextBox.Clear();
            AnswerATextBox.Clear();
            AnswerBTextBox.Clear();
            AnswerCTextBox.Clear();
            AnswerDTextBox.Clear();
            CorrectAnswerComboBox.SelectedIndex = -1;
            ImagePathTextBox.Clear();

            selectedQuestion = null;
            AddQuestionButton.Visibility = Visibility.Visible;
            UpdateQuestionButton.Visibility = Visibility.Collapsed;
        }
    }
}