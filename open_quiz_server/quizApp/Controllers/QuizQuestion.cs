namespace quizApp.Controllers;
    public class QuizQuestion
    {
        public QuizQuestion()
        {
        }

        public Guid Id { get;  set; }
        public string Question { get;  set; }
        public List<string> Answers { get;  set; }
    }