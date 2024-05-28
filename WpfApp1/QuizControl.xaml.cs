using System.Windows;

namespace WpfApp1
{
    public partial class QuizControl : Window
    {
        private PlayQuiz _playQuiz;

        public QuizControl(PlayQuiz playQuiz)
        {
            InitializeComponent();
            _playQuiz = playQuiz;
        }

        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            _playQuiz.NextQuestion();
        }

        private void PreviousQuestion_Click(object sender, RoutedEventArgs e)
        {
            _playQuiz.PreviousQuestion();
        }

        private void ShowRightAnswer_Click(object sender, RoutedEventArgs e)
        {
            _playQuiz.ShowRightAnswer();
        }

        private void EndQuiz_Click(object sender, RoutedEventArgs e)
        {
            _playQuiz.EndQuiz();
            this.Close();
        }
    }
}
