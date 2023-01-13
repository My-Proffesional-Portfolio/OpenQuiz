using System.Text.Json;
using MoreLinq;
using quizApp.Models;
using quizApp.Models.Exceptions;
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

        var request = new RestRequest("?amount=10&category=" + categoryID + "&difficulty=easy&encode=base64");
        var response =  client.Get(request);
        var objEncoded = JsonSerializer.Deserialize<QuizApiOriginalResponseModel>(response.Content);

        var obj = DecodeQuizResponse(objEncoded);
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

    private QuizApiOriginalResponseModel DecodeQuizResponse(QuizApiOriginalResponseModel objEncoded)
    {
        var decodedResult =  new QuizApiOriginalResponseModel();
        decodedResult.response_code = objEncoded.response_code;
        decodedResult.results = new List<Result>();

        objEncoded.results.ForEach(fe=> {
            var r = new Result();
            r.category = Utils.DecodeBase64String(fe.category);
            r.correct_answer = Utils.DecodeBase64String(fe.correct_answer);
            r.incorrect_answers = fe.incorrect_answers.Select(s=> Utils.DecodeBase64String(s)).ToList();
            r.question = Utils.DecodeBase64String(fe.question);
            decodedResult.results.Add(r);
        });
        
        return decodedResult;
    }

    public QuestionAnswerResponseModel EvaluateAnswerFromSession (Guid questionID, string userAnswer)
    {
        var sessionQuestions =  _httpContextAccessor.HttpContext.Session.GetObject<QuizApiOriginalResponseModel>("serverQuestions");

        if (sessionQuestions == null)
            throw new NotFoundException("Your session hasn´t started");

        var selectedQuestion = sessionQuestions.results.Where(w=> w.InternalUUID == questionID).FirstOrDefault();

        if (selectedQuestion == null || selectedQuestion.IsQuestionAnswered)
            throw new NotFoundException("Question not found in your session or has been previously answered");
        
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