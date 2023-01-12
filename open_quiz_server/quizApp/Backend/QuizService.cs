using System.Text.Json;
using MoreLinq;
using quizApp.Models;
using quizApp.Session;
using RestSharp;

namespace quizApp.Backend;

public class QuizService
{

    private readonly IHttpContextAccessor _httpContextAccessor;
    public QuizService( IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public List<QuizQuestion> GetQuizQuestionsForSession(int categoryID)
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
            internalQuestion.SessionID = _httpContextAccessor.HttpContext.Session.Id;

            viewList.Add(internalQuestion);
        }

        _httpContextAccessor.HttpContext.Session.SetObject("serverQuestions", obj);
        return viewList;
    }

    public QuestionAnswerResponseModel EvaluateAnswerFromSession (Guid questionID, string userAnswer)
    {
        var sessionQuestions =  _httpContextAccessor.HttpContext.Session.GetObject<QuizApiOriginalResponseModel>("serverQuestions");

        if (sessionQuestions == null)
            throw new Exception("Your session hasn´t started");

        var selectedQuestion = sessionQuestions.results.Where(w=> w.InternalUUID == questionID).FirstOrDefault();

        if (selectedQuestion == null || selectedQuestion.IsQuestionAnswered)
            throw new Exception("Question not found in your session or has been previously answered");
        
        var correct_answer = selectedQuestion.correct_answer;

        selectedQuestion.IsQuestionAnswered = true;

        var response =  new QuestionAnswerResponseModel();
        if (correct_answer == userAnswer)
        {
            selectedQuestion.IsCorrectAndwerPoint = true;
            response = new  QuestionAnswerResponseModel {
                IsCorrectAnswer = true, 
                YourAnswer = userAnswer,  
                CorrectAnswer =  correct_answer, 
                IsGameFinished = sessionQuestions.results.All(a=> a.IsQuestionAnswered),
                AcomulatedPoints = sessionQuestions.results.Count(c=> c.IsCorrectAndwerPoint) * 100,
                QuestionID = questionID,
                };
        }

        else
            response = new QuestionAnswerResponseModel
            {
                IsCorrectAnswer = false, 
                YourAnswer = userAnswer, 
                CorrectAnswer =  correct_answer,
                IsGameFinished = sessionQuestions.results.All(a=> a.IsQuestionAnswered),
                AcomulatedPoints = sessionQuestions.results.Count(c=> c.IsCorrectAndwerPoint) * 100,
                QuestionID = questionID
            };

         _httpContextAccessor.HttpContext.Session.SetObject("serverQuestions", sessionQuestions);

         return response;
    }
}