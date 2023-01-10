using System.Text.Json.Serialization;

namespace quizApp.Models
{
    public class QuizApiOriginalResponseModel
    {
        public int response_code { get; set; }
        public List<Result> results { get; set; }
    }

    public class Result
    {
        public string category { get; set; }
        public string type { get; set; }
        public string difficulty { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public List<string> incorrect_answers { get; set; }
        [JsonIgnore]
        public Guid InternalUUID { get;  set; }
        [JsonIgnore]
        public bool IsCorrectAndwerPoint { get; set; }
        [JsonIgnore]
        public bool IsQuestionAnswered { get;  set; }
    }

}
