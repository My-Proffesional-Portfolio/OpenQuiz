using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using quizApp.Session;
using MoreLinq;
using quizApp.Models;

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

            internalQuestion.Answers = internalQuestion.Answers.Shuffle().ToList();

            viewList.Add(internalQuestion);
        }

        HttpContext.Session.SetObject("serverQuestions", obj);
        return Ok(viewList);
    }


    [HttpGet]
    [Route("session")]
    public IActionResult Session (Guid questionID, string userAnswer)
    {
        var sessionQuestions =  HttpContext.Session.GetObject<QuizApiOriginalResponseModel>("serverQuestions");

        var selectedQuestion = sessionQuestions.results.Where(w=> w.InternalUUID == questionID).FirstOrDefault();

        if (selectedQuestion == null)
            return NotFound("Question not found in your session");
        
        var correct_answer = selectedQuestion.correct_answer;

        if (correct_answer == userAnswer)
        {
            selectedQuestion.IsCorrectAndwerPoint = true;
            HttpContext.Session.SetObject("serverQuestions", sessionQuestions);
            return Ok (new {isCorrectAndwer = true, yourAnswer = userAnswer,  correctAnswer =  correct_answer});
        }


         selectedQuestion.IsCorrectAndwerPoint = false;
         HttpContext.Session.SetObject("serverQuestions", sessionQuestions);
         return Ok (new {isCorrectAndwer = false, yourAnswer = userAnswer, correctAnswer =  correct_answer});
    }
}