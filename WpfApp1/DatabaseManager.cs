using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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


    public int AddQuestion(Question question, List<Answer> answers, int quizID)
    {
        int questionID;
        string questionQuery = "INSERT INTO questions (QuizID, QuestionText, imagePath) VALUES (@QuizID, @QuestionText, @ImagePath); SELECT LAST_INSERT_ID();";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            using (MySqlCommand command = new MySqlCommand(questionQuery, connection))
            {
                command.Parameters.AddWithValue("@QuizID", quizID);
                command.Parameters.AddWithValue("@QuestionText", question.QuestionText);
                command.Parameters.AddWithValue("@ImagePath", question.ImagePath);

                try
                {
                    connection.Open();
                    questionID = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving question to database: {ex.Message}");
                    return -1;
                }
            }
        }

        foreach (Answer answer in answers)
        {
            AddAnswer(questionID, answer);
        }

        return questionID;
    }

    public void RemoveQuestion(int? questionID)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            // Delete associated answers
            string deleteAnswersQuery = "DELETE FROM answers WHERE QuestionID = @QuestionID";
            using (MySqlCommand command = new MySqlCommand(deleteAnswersQuery, connection))
            {
                command.Parameters.AddWithValue("@QuestionID", questionID);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            // Delete the question itself
            string deleteQuestionQuery = "DELETE FROM questions WHERE QuestionID = @QuestionID";
            using (MySqlCommand command = new MySqlCommand(deleteQuestionQuery, connection))
            {
                command.Parameters.AddWithValue("@QuestionID", questionID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    private void AddAnswer(int? questionID, Answer answer)
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
            string query = $"SELECT q.QuestionId, q.QuizId, q.QuestionText, q.imagePath, a.AnswerId, a.AnswerText, a.IsCorrect " +
                           $"FROM questions q " +
                           $"LEFT JOIN answers a ON q.QuestionId = a.QuestionId " +
                           $"WHERE q.QuizId = {id}";

            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int questionId = reader.GetInt32("QuestionId");

                    // Check if the question already exists in the list
                    Question question = questions.FirstOrDefault(q => q.QuestionID == questionId);

                    if (question == null)
                    {
                        question = new Question
                        {
                            QuestionID = questionId,
                            QuizId = reader.GetInt32("QuizId"),
                            QuestionText = reader.GetString("QuestionText"),
                            ImagePath = reader.GetString("imagePath"),
                            Answers = new List<Answer>()
                        };
                        questions.Add(question);
                    }

                    // Add the answer to the question's list of answers
                    if (!reader.IsDBNull(reader.GetOrdinal("AnswerId")))
                    {
                        Answer answer = new Answer
                        {
                            AnswerId = reader.GetInt32("AnswerId"),
                            AnswerText = reader.GetString("AnswerText"),
                            isCorrect = reader.GetBoolean("IsCorrect")
                        };
                        question.Answers.Add(answer);
                    }
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
                question.Answers = getAnswersById(question.QuestionID);
            }
        }

        return quizzes;

        

    }

    public void UpdateQuestion(int? questionID, Question updatedQuestion, List<Answer> updatedAnswers)
    {
        string updateQuestionQuery = "UPDATE questions SET QuestionText = @QuestionText WHERE QuestionID = @QuestionID";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            using (MySqlCommand command = new MySqlCommand(updateQuestionQuery, connection))
            {
                command.Parameters.AddWithValue("@QuestionID", questionID);
                command.Parameters.AddWithValue("@QuestionText", updatedQuestion.QuestionText);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating question in the database: {ex.Message}");
                    return;
                }
            }

            // First, delete existing answers
            string deleteAnswersQuery = "DELETE FROM answers WHERE QuestionID = @QuestionID";
            using (MySqlCommand command = new MySqlCommand(deleteAnswersQuery, connection))
            {
                command.Parameters.AddWithValue("@QuestionID", questionID);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting old answers from database: {ex.Message}");
                    return;
                }
            }

            // Add updated answers
            foreach (Answer answer in updatedAnswers)
            {
                AddAnswer(questionID, answer);
            }
        }
    }

}


