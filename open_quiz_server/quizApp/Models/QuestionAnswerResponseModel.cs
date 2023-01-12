using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quizApp.Models
{
    public class QuestionAnswerResponseModel
    {
        public bool IsCorrectAnswer { get;  set; }
        public string YourAnswer { get;  set; }
        public int AcomulatedPoints { get;  set; }
        public bool IsGameFinished { get;  set; }
        public string CorrectAnswer { get;  set; }
        public Guid QuestionID { get;  set; }
    }
}