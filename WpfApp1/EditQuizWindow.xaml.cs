using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class EditQuizWindow : Window
    {
        private DatabaseManager databaseManager;
        private JsonManager jsonManager;
        private int quizId;
        private QuestionViewModel selectedQuestion;

        public ObservableCollection<QuestionViewModel> Questions { get; set; }

        public EditQuizWindow(Quiz existingQuiz)
        {
            this.Title = existingQuiz.Title;
            databaseManager = new DatabaseManager();
            jsonManager = new JsonManager();
            quizId = existingQuiz.ID;

            Questions = new ObservableCollection<QuestionViewModel>(LoadQuestionsFromDatabase(quizId));

            InitializeComponent();
            QuestionsListBox.ItemsSource = Questions;
        }

        private IEnumerable<QuestionViewModel> LoadQuestionsFromDatabase(int quizId)
        {
            var questions = databaseManager.getQuestionsById(quizId);
            var questionViewModels = questions.Select(q => new QuestionViewModel
            {
                QuestionID = q.QuestionID,
                QuestionText = q.QuestionText,
                AnswerA = GetAnswerText(q, 0),
                AnswerB = GetAnswerText(q, 1),
                AnswerC = GetAnswerText(q, 2),
                AnswerD = GetAnswerText(q, 3),
                CorrectAnswer = GetCorrectAnswerText(q)
            });

            return questionViewModels;
        }

        private string GetAnswerText(Question q, int index)
        {
            if (q.Answers.Count > index)
            {
                return q.Answers[index].AnswerText;
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetCorrectAnswerText(Question q)
        {
            var correctAnswer = q.Answers.FirstOrDefault(a => a.isCorrect);
            if (correctAnswer != null)
            {
                return correctAnswer.AnswerText;
            }
            else
            {
                return string.Empty;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                CorrectAnswer = ((ComboBoxItem)CorrectAnswerComboBox.SelectedItem)?.Content.ToString()
            };

            var question = new Question(newQuestion.QuestionText);
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

                var question = new Question(selectedQuestion.QuestionText);
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

        private void SaveQuiz_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var question in Questions)
            {
                databaseManager.AddQuestion(new Question(question.QuestionText), new List<Answer>
                {
                    new Answer(question.AnswerA, question.CorrectAnswer == "A"),
                    new Answer(question.AnswerB, question.CorrectAnswer == "B"),
                    new Answer(question.AnswerC, question.CorrectAnswer == "C"),
                    new Answer(question.AnswerD, question.CorrectAnswer == "D")
                }, quizId);
            }

            MessageBox.Show("Quiz bijgewerkt en opgeslagen in de database!");
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
                CorrectAnswerComboBox.SelectedItem = CorrectAnswerComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == selectedQuestion.CorrectAnswer);

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

            selectedQuestion = null;
            AddQuestionButton.Visibility = Visibility.Visible;
            UpdateQuestionButton.Visibility = Visibility.Collapsed;
        }
    }
}
