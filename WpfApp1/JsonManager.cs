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
            string json = JsonConvert.SerializeObject(quizzes, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }

        public void SaveQuestionToJson(Question question)
        {

        }

        public Quiz LoadQuizFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);

            Quiz quiz = JsonConvert.DeserializeObject<Quiz>(json);

            return quiz;
        }
    }
}
