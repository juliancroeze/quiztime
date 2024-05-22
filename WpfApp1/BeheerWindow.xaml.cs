using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WpfApp1
{
    public partial class BeheerWindow : Window
    {
        public ObservableCollection<Quiz> Quizzes { get; set; }
        public DatabaseManager databaseManager;
        public JsonManager jsonManager;

        public BeheerWindow()
        {
            databaseManager = new DatabaseManager();
            jsonManager = new JsonManager();
            InitializeComponent();
            Quizzes = new ObservableCollection<Quiz>();
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

        private void NewQuizButton_Click(object sender, RoutedEventArgs e)
        {
            string quizTitle = NewQuizTextBox.Text;
            int newQuizId = databaseManager.AddQuiz(new Quiz { Title = quizTitle });


            AddQuizWindow addQuizWindow = new AddQuizWindow(newQuizId, quizTitle);
            addQuizWindow.Owner = this;  
            bool? result = addQuizWindow.ShowDialog();

            if (result == true)
            {
                LoadQuizzes();  // Reload quizzes if a new quiz was added
            }
        }

        private void DeleteQuizButton_Click(object sender, RoutedEventArgs e)
        {
            Quiz selectedQuiz = QuizListBox.SelectedItem as Quiz;
            if (selectedQuiz != null)
            {
                MessageBoxResult result = MessageBox.Show($"Weet je zeker dat je de quiz '{selectedQuiz.Title}' wilt verwijderen?", "Bevestigen", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    databaseManager.DeleteQuiz(selectedQuiz.ID);
                    LoadQuizzes();
                }
            }
            else
            {
                MessageBox.Show("Selecteer een quiz om te verwijderen.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewQuizTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
