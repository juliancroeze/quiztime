using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApp1
{
    public partial class QuestionDisplay : Window
    {
        private DispatcherTimer timer;
        private int timeLeft = 30; // Initial time left in seconds

        public QuestionDisplay()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            TimerText.Text = $"Time Left: {timeLeft} sec";
            if (timeLeft <= 0)
            {
                timer.Stop();
                // Optionally, you can trigger an action when time's up
                // For example, move to the next question automatically
                // NextQuestion();
            }
        }

        public void StartTimer()
        {
            ResetTimer();
            timeLeft = 30;
            TimerText.Text = $"Time Left: {timeLeft} sec";
            timer.Start();
        }

        public void StopTimer()
        {
            timer.Stop();
        }

        public void DisplayQuestion(Question question)
        {
            ResetTimer();
            StartTimer();

            QuestionText.Text = question.QuestionText;
            Answer1.Content = question.Answers[0].AnswerText;
            Answer2.Content = question.Answers[1].AnswerText;
            Answer3.Content = question.Answers[2].AnswerText;
            Answer4.Content = question.Answers[3].AnswerText;

            if (!string.IsNullOrEmpty(question.ImagePath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(question.ImagePath, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                QuestionImage.Source = bitmap;
                QuestionImage.Visibility = Visibility.Visible;
            }
            else
            {
                QuestionImage.Visibility = Visibility.Collapsed;
            }
        }

        private void ResetTimer()
        {
            StopTimer();
            timeLeft = 30;
        }

        public void HighlightAnswer(Question question)
        {
            StopTimer();
            // Resetting all answers to the default style first
            ResetAnswerStyles();

            // Highlight the correct answer
            foreach (var answer in question.Answers)
            {
                if (answer.isCorrect)
                {
                    HighlightCorrectAnswer(answer.AnswerText);
                    break;
                }
            }
        }

        public void ResetAnswerStyles()
        {
            // Reset each answer button's background color
            Answer1.ClearValue(Button.BackgroundProperty);
            Answer2.ClearValue(Button.BackgroundProperty);
            Answer3.ClearValue(Button.BackgroundProperty);
            Answer4.ClearValue(Button.BackgroundProperty);
        }

        private void HighlightCorrectAnswer(string correctAnswerText)
        {
            foreach (var button in new[] { Answer1, Answer2, Answer3, Answer4 })
            {
                if (button.Content.ToString() == correctAnswerText)
                {
                    button.Background = new SolidColorBrush(Colors.Gold);
                    break;
                }
            }
        }
    }
}
