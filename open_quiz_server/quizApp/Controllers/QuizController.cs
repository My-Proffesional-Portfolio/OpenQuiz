using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using quizApp.Session;

namespace quizApp.Controllers;


[ApiController]
[Route("[controller]")]
public class QuizController : ControllerBase
{

    [HttpGet]
    public IActionResult GetQuizQuestion (int categoryID)
    {

        //https://opentdb.com/api.php?amount=10&category=10&difficulty=easy
        var client = new RestClient("https://opentdb.com/api.php");

        var request = new RestRequest("?amount=10&category=" + categoryID + "&difficulty=easy");
        var response =  client.Get(request);
        var obj = JsonSerializer.Deserialize<QuizApiOriginalResponseModel>(response.Content);

        var viewList = new List<QuizQuestion>();
        foreach (var q in obj.results)
        {
            var currentIndex = obj.results.IndexOf(q);
            obj.results[currentIndex].InternalUUID = Guid.NewGuid();
            var internalQuestion = new QuizQuestion();
            internalQuestion.Id =  obj.results[currentIndex].InternalUUID;
            internalQuestion.Question = q.question;
            internalQuestion.Answers = q.incorrect_answers;
            internalQuestion.Answers.Add(q.correct_answer);
        }

        HttpContext.Session.SetObject("serverQuestions", obj);
        return Ok(viewList);
    }
}