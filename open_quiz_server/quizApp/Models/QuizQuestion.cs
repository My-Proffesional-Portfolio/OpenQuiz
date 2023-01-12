namespace quizApp.Models;
    public class QuizQuestion
    {
        public QuizQuestion()
        {
            Answers = new List<string>();
        }

        public Guid Id { get;  set; }
        public string Question { get;  set; }
        public List<string> Answers { get;  set; }
        public string SessionID { get; set; }
    }