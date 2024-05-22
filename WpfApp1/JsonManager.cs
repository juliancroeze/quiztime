using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace WpfApp1
{
    public class JsonManager
    {
        public void SaveQuizToJson(List<Quiz> quizzes, string filePath)
        {
            // Serialize the quiz object to JSON
            string json = JsonConvert.SerializeObject(quizzes, Formatting.Indented);

            // Write the JSON to a file
            File.WriteAllText(filePath, json);
        }

        public void SaveQuestionToJson(Question question)
        {

        }

        public Quiz LoadQuizFromJson(string filePath)
        {
            // Read the JSON from the file
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON to a Quiz object
            Quiz quiz = JsonConvert.DeserializeObject<Quiz>(json);

            return quiz;
        }
    }
}
