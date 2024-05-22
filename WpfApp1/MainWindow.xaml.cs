using System;
using System.Windows;
using System.Windows.Navigation;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BeheerQuizzes_Click(object sender, RoutedEventArgs e)
        {
            BeheerWindow beheerWindow = new BeheerWindow();
            beheerWindow.Show();
        }

        private void CreateNewQuiz_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ViewQuizzes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
