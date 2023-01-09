namespace quizApp.Models;
    public class QuizQuestion
    {
        public QuizQuestion()
        {
        }

        public Guid Id { get;  set; }
        public string Question { get;  set; }
        public List<string> Answers { get;  set; }
    }