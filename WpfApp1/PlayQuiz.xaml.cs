using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WpfApp1
{
    public partial class PlayQuiz : Window
    {
        public ObservableCollection<Quiz> Quizzes { get; set; }
        public DatabaseManager databaseManager;
        public JsonManager jsonManager;
        private int currentQuizIndex = 0;
        private int currentQuestionIndex = 0;
        private QuestionDisplay questionDisplay;
        private QuizControl quizControl;

        public PlayQuiz()
        {
            InitializeComponent();
            Quizzes = new ObservableCollection<Quiz>();
            databaseManager = new DatabaseManager();
            jsonManager = new JsonManager();
            LoadQuizzes();
            DataContext = this;
        }

        public void LoadQuizzes()
        {
            Quizzes.Clear();
            List<Quiz> temp = databaseManager.GetCompleteQuizList();
            jsonManager.SaveQuizToJson(temp, "quiz.json");
            foreach (Quiz quiz in temp)
            {
                Quizzes.Add(quiz);
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            currentQuizIndex = QuizListBox.SelectedIndex;
            OpenQuizScreens();
        }

        private void OpenQuizScreens()
        {
            if (Quizzes.Count == 0) return;

            questionDisplay = new QuestionDisplay();
            quizControl = new QuizControl(this);

            DisplayCurrentQuestion();
            questionDisplay.Show();
            quizControl.Show();
            this.Hide();
        }

        public void DisplayCurrentQuestion()
        {
            questionDisplay.ResetAnswerStyles();
            if (currentQuizIndex < Quizzes.Count && currentQuestionIndex < Quizzes[currentQuizIndex].questions.Count)
            {
                Question currentQuestion = Quizzes[currentQuizIndex].questions[currentQuestionIndex];
                questionDisplay.StartTimer();
                questionDisplay.DisplayQuestion(currentQuestion);
            }
        }

        public void NextQuestion()
        {
            if (currentQuestionIndex < Quizzes[currentQuizIndex].questions.Count - 1)
            {

                currentQuestionIndex++;
                DisplayCurrentQuestion();
            }
        }

        public void PreviousQuestion()
        {
            if (currentQuestionIndex > 0)
            {
                currentQuestionIndex--;
                DisplayCurrentQuestion();
            }
        }

        public void ShowRightAnswer()
        {
            if (currentQuizIndex < Quizzes.Count && currentQuestionIndex < Quizzes[currentQuizIndex].questions.Count)
            {
                Question currentQuestion = Quizzes[currentQuizIndex].questions[currentQuestionIndex];
                questionDisplay.HighlightAnswer(currentQuestion);
            }
        }

        public void EndQuiz()
        {
            questionDisplay.Close();
            this.Show();
        }
    }
}
