using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class AddQuizWindow : Window
    {
        private DatabaseManager databaseManager;
        private JsonManager jsonManager;
        private int quizId;

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

            QuestionTextBox.Clear();
            AnswerATextBox.Clear();
            AnswerBTextBox.Clear();
            AnswerCTextBox.Clear();
            AnswerDTextBox.Clear();
            CorrectAnswerComboBox.SelectedIndex = -1;
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
            // Assuming you have a title textbox for the quiz title
            // string title = TitleTextBox.Text;

            // Quiz newQuiz = new Quiz
            // {
            //     Title = title,
            // };

            // Add the new quiz to the database and get its ID
            // quizId = databaseManager.AddQuiz(newQuiz);

            // Add each question with the associated quizID
            // foreach (var item in Questions)
            // {
            //     databaseManager.AddQuestion(item, quizId);
            // }

            MessageBox.Show("Quiz toegevoegd en opgeslagen in de database!");
            this.DialogResult = true;  // Indicate that a new quiz was added
            this.Close();
        }

        private void QuestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
