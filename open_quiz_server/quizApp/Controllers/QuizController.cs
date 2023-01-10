using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using quizApp.Session;
using MoreLinq;
using quizApp.Models;
using quizApp.Backend;
using quizApp.Models.Exceptions;

namespace quizApp.Controllers;


[ApiController]
[Route("[controller]")]
public class QuizController : ControllerBase
{

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly QuizService _quizSC;
    public QuizController( IHttpContextAccessor httpContextAccessor, QuizService quizSC)
    {
        _httpContextAccessor = httpContextAccessor;
        _quizSC = quizSC;
    }

    [HttpGet]
    public IActionResult GetQuizQuestion (int categoryID)
    {
        var viewList = _quizSC.GetQuizQuestionsForSession(categoryID);
        return Ok(viewList);
    }


    [HttpGet]
    [Route("evaluateAnswer")]
    public IActionResult EvaluateAnswerFromSession (Guid questionID, string userAnswer)
    {
        try 
        {
            var response = _quizSC.EvaluateAnswerFromSession(questionID, userAnswer);
            return Ok(response);
         }
         catch(NotFoundException ex)
         {
            return NotFound(ex.Message);
         }
         catch(Exception ex)
         {
            return Problem(ex.Message, statusCode: 500);
         }
         
    }
}