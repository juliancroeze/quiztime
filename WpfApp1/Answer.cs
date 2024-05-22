namespace WpfApp1
{
    public class Answer
    {
        public int? AnswerId { get; set; }
        public int? QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool isCorrect { get; set; }

        // Constructor with parameters
        public Answer(string answerText, bool isCorrect, int? answerId = null)
        {
            AnswerId = answerId;
       
            AnswerText = answerText;
            this.isCorrect = isCorrect;
        }

        public Answer()
        {

        }

        public int? GetAnswerId()
        {
            return AnswerId;
        }

        public void SetAnswerId(int? answerId)
        {
            AnswerId = answerId;
        }

        public int? GetQuestionId()
        {
            return QuestionId;
        }

        public void SetQuestionId(int questionId)
        {
            QuestionId = questionId;
        }

        public string GetAnswerText()
        {
            return AnswerText;
        }

        public void SetAnswerText(string answerText)
        {
            AnswerText = answerText;
        }

        public bool GetIsCorrect()
        {
            return isCorrect;
        }

        public void SetIsCorrect(bool isCorrect)
        {
            this.isCorrect = isCorrect;
        }
    }
}
