using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Question
    {
        public int? QuestionID { get; set; }
        public int QuizId { get; set; }

        public string QuestionText { get; set; }
        public List<Answer> answers { get; set; }

        public Question(string questionText, int? questionId = null)
        {
            QuestionText = QuestionText;
            QuestionID = questionId;

        }

        public Question (){}

        public void SetQuestionId(int? id)
        {
            QuestionID = id;
        }

        public void AddAnswer(Answer answer)
        {
            answers.Add(answer);
        }
    }
}
