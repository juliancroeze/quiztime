using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using WpfApp1;

public class DatabaseManager
{
    private string connectionString = "Server=localhost;Database=quiz_database;Uid=root;Pwd=;";

    public int AddQuiz(Quiz quiz)
    {
        int quizID = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "INSERT INTO quizzes (Title) VALUES (@Title); SELECT LAST_INSERT_ID();";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", quiz.Title);
            connection.Open();
            quizID = Convert.ToInt32(command.ExecuteScalar());
        }
        return quizID;
    }

    public void AddQuestion(Question question, List<Answer> answers, int quizID)
    {


            int questionID;
            string questionQuery = "INSERT INTO questions (QuizID, QuestionText) VALUES (@QuizID, @QuestionText); SELECT LAST_INSERT_ID();";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(questionQuery, connection))
                {
                    command.Parameters.AddWithValue("@QuizID", quizID);
                    command.Parameters.AddWithValue("@QuestionText", question.QuestionText);

                    try
                    {
                        connection.Open();
                        questionID = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving question to database: {ex.Message}");
                        return;
                    }
                }
            }

            //string[] answers = { answerA, answerB, answerC, answerD };
            foreach(Answer answer in answers)
        {

                AddAnswer(questionID, answer);
        }
        
        //}
    }

    private void AddAnswer(int questionID, Answer answer)
    {
        string answerQuery = "INSERT INTO answers (QuestionID, AnswerText, IsCorrect) VALUES (@QuestionID, @AnswerText, @IsCorrect)";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            using (MySqlCommand command = new MySqlCommand(answerQuery, connection))
            {
                command.Parameters.AddWithValue("@QuestionID", questionID);
                command.Parameters.AddWithValue("@AnswerText", answer.AnswerText);
                command.Parameters.AddWithValue("@IsCorrect", answer.isCorrect);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving answer to database: {ex.Message}");
                }
            }
        }
    }

    public void DeleteQuiz(int quizID)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            // First, delete associated answers
            string deleteAnswersQuery = "DELETE FROM answers WHERE QuestionID IN (SELECT QuestionId FROM questions WHERE QuizID = @QuizID)";
            using (MySqlCommand command = new MySqlCommand(deleteAnswersQuery, connection))
            {
                command.Parameters.AddWithValue("@QuizID", quizID);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            // Then, delete associated questions
            string deleteQuestionsQuery = "DELETE FROM questions WHERE QuizID = @QuizID";
            using (MySqlCommand command = new MySqlCommand(deleteQuestionsQuery, connection))
            {
                command.Parameters.AddWithValue("@QuizID", quizID);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            // Finally, delete the quiz itself
            string deleteQuizQuery = "DELETE FROM quizzes WHERE QuizID = @QuizID";
            using (MySqlCommand command = new MySqlCommand(deleteQuizQuery, connection))
            {
                command.Parameters.AddWithValue("@QuizID", quizID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    public List<Quiz> GetAllQuizzes()
    {
        List<Quiz> quizzes = new List<Quiz>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT QuizId, Title FROM quizzes";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Quiz quiz = new Quiz
                    {
                        ID = reader.GetInt32("QuizId"),
                        Title = reader.GetString("Title"),
                    };
                    quizzes.Add(quiz);
                }
            }
        }

        return quizzes;
    }
    public List<Question> getQuestionsById(int id)
    {
        List<Question> questions = new List<Question>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = $"SELECT * FROM questions WHERE QuizId = {id}";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Question question = new Question
                    {
                        QuestionID = reader.GetInt32("QuestionId"),
                        QuizId = id,
                        QuestionText = reader.GetString("QuestionText"),
                    };
                    questions.Add(question);
                }
            }
        }
        return questions;
    }

    public List<Answer> getAnswersById(int? id)
    {
        List<Answer> answers = new List<Answer>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = $"SELECT * FROM answers WHERE QuestionId = {id}";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Answer answer = new Answer
                    {
                        AnswerId = reader.GetInt32("AnswerId"),
                        QuestionId = id,
                        AnswerText = reader.GetString("AnswerText"),
                        isCorrect = reader.GetBoolean("isCorrect")
                    };
                    answers.Add(answer);
                }
            }
        }
        return answers;
    }

    public List<Quiz> GetCompleteQuizList()
    {
        var quizzes = GetAllQuizzes();

        // voor elke quiz, haal alle vragen op
        foreach(Quiz quiz in quizzes)
        {
            
            quiz.questions = getQuestionsById(quiz.ID);
            foreach(Question question in quiz.questions)
            {
                question.answers = getAnswersById(question.QuestionID);
            }
        }

        return quizzes;

        

    }
}


