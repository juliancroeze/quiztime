using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class AddQuizWindow : Window
    {
        private DatabaseManager databaseManager;
        private JsonManager jsonManager;

        private int quizId;
        public AddQuizWindow(int newQuizId, string quizName)
        {
            this.Title = quizName;
            databaseManager = new DatabaseManager();
            jsonManager = new JsonManager();
            quizId = newQuizId;
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            string question = QuestionTextBox.Text;
            string answerA = AnswerATextBox.Text;
            string answerB = AnswerBTextBox.Text;
            string answerC = AnswerCTextBox.Text;
            string answerD = AnswerDTextBox.Text;
            string correctAnswer = ((ComboBoxItem)CorrectAnswerComboBox.SelectedItem).Content.ToString();

            Question newQuestion = new Question(question);
            List<Answer> newAnswers = new List<Answer>();
            Answer newAnswerA = new Answer(answerA, correctAnswer == "A");
            Answer newAnswerB = new Answer(answerB, correctAnswer == "B");
            Answer newAnswerC = new Answer(answerC, correctAnswer == "C");
            Answer newAnswerD = new Answer(answerD, correctAnswer == "D");

            newAnswers.Add(newAnswerA);
            newAnswers.Add(newAnswerB);
            newAnswers.Add(newAnswerC);
            newAnswers.Add(newAnswerD);

            databaseManager.AddQuestion(newQuestion, newAnswers, quizId);


            QuestionsListBox.Items.Add($"{newQuestion.QuestionText}");

            QuestionTextBox.Clear();
            AnswerATextBox.Clear();
            AnswerBTextBox.Clear();
            AnswerCTextBox.Clear();
            AnswerDTextBox.Clear();
            CorrectAnswerComboBox.SelectedIndex = -1;
        }

        // dit is een nieuwe quiz aanmaken
        private void CreateNewQuiz_OnClick(object sender, RoutedEventArgs e)
        {
            // Assuming you have a title textbox for the quiz title
            //string title = TitleTextBox.Text;

        //    Quiz newQuiz = new Quiz
          //  {
            //    Title = title,
            //};

            // Add the new quiz to the database and get its ID
            //quizId = databaseManager.AddQuiz(newQuiz);

            // Add each question with the associated quizID
            //foreach (var item in QuestionsListBox.Items)
            //{
            //    databaseManager.AddQuestion(item.ToString(), quizID);
            //}

            



            MessageBox.Show("Quiz toegevoegd en opgeslagen in de database!");
            this.DialogResult = true;  // Indicate that a new quiz was added
            this.Close();
        }
    }
}
