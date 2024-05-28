using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class QuestionViewModel
    {
        public int? QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public string ImagePath { get; set; }
        public string CorrectAnswer { get; set; }


        // deez constructor wil je aanroepen wanneer je een nieuwe vraag maakt om te checken of hij leeg is
        public QuestionViewModel(string questionText, string answerA, string answerB, string answerC, string answerD)
        {
            setQuestionText(questionText);
            AnswerA = answerA;
            AnswerB = answerB;
            AnswerC = answerC;
            AnswerD = answerD;
        }

        public QuestionViewModel() { }
        private void setQuestionText(string questionText)
        {
            if(String.IsNullOrEmpty(questionText))
            {
                throw new QuestionException("is leeggwjkdsgfjkdsjjonguh, tekst ");
            }
            QuestionText = questionText;

        }
    }

public class QuestionException : Exception
    {
        // Property to hold the custom message
        public string CustomMessage { get; }

        // Constructor that takes a message as a parameter
        public QuestionException(string message) : base(message)
        {
            CustomMessage = message;
        }

        // Constructor that takes a message and inner exception
        public QuestionException(string message, Exception innerException) : base(message, innerException)
        {
            CustomMessage = message;
        }
    }


}
