using DataAccess.Entities;
using MongoDB.Driver;
using System;
using DTOs;
using MongoDB.Bson;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic;

namespace DataAccess.Services;

public class MongoDbService
{

    private readonly IMongoCollection<Quiz> _quiz;
    private readonly IMongoCollection<Question> _question;
    private readonly IMongoCollection<Category> _category;


    public MongoDbService()
    {
        var hostName = "localhost";
        var port = "27017";
        var databaseName = "QuizManagerDb";
        var client = new MongoClient($"mongodb://{hostName}:{port}");
        var database = client.GetDatabase(databaseName);
        _quiz = database.GetCollection<Quiz>("Quiz", new MongoCollectionSettings() { AssignIdOnInsert = true });
        _question = database.GetCollection<Question>("Question", new MongoCollectionSettings() { AssignIdOnInsert = true });
        _category = database.GetCollection<Category>("Category", new MongoCollectionSettings() { AssignIdOnInsert = true });

    }

    public List<QuestionRecord> GetAllQuestions()
    {

        var allQuestions = _question.Find(_ => true).ToList()
            .Select(p => new QuestionRecord(p.Id.ToString(), p.QuestionText, p.AnswerOptions, p.CorrectAnswerIndex,p.Category.ToString()));

        return allQuestions.ToList();
    }

    public List<QuizRecord> GetAllQuizzes()
    {

        var allQuizzes = _quiz.Find(_ => true).ToList()
            .Select(p => new QuizRecord(p.Id.ToString(), p.QuizTitle, p.QuizDescription));
        return allQuizzes.ToList();
    }

    public List<CategoryRecord> GetAllCategories()
    {

        var allCategories = _category.Find(_ => true).ToList()
            .Select(p => new CategoryRecord(p.CategoryName.ToString()));
        return allCategories.ToList();
    }

    public IEnumerable<QuestionRecord> GetQuestionsForQuiz(object id)
    {
        var quizId = id.ToString();

        var quiz = _quiz.Find(q => q.Id.ToString() == quizId).FirstOrDefault();

        if (quiz != null)
        {
            var questionIds = quiz.Questions;

            var filter = Builders<Question>.Filter.In(q => q.Id, questionIds);

            var questions = new List<Question>();

            foreach (var question in questionIds)
            {
                
                questions = _question.Find(filter).ToList();
            }

            
            return questions.Select(q => new QuestionRecord(
                Id: q.Id.ToString(),
                QuestionText: q.QuestionText,
                AnswerOptions: q.AnswerOptions,
                CorrectAnswerIndex: q.CorrectAnswerIndex,
                Category: q.Category.ToString()
            ));
        }

        return Enumerable.Empty<QuestionRecord>();

    }

    public IEnumerable<QuestionRecord> GetQuestionsByCategory(string category)
    {
        var filter = Builders<Question>.Filter.Eq("Category", category);

        var questions = _question.Find(filter).ToList();

        return questions.Select(q => new QuestionRecord(
            Id: q.Id.ToString(),
            QuestionText: q.QuestionText,
            AnswerOptions: q.AnswerOptions,
            CorrectAnswerIndex: q.CorrectAnswerIndex,
            Category: q.Category.ToString()
        ));


    }


    public IEnumerable<QuestionRecord> GetQuestionsByTitle(string searchText)
    {

        var allQuestions = GetAllQuestions();

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return allQuestions;
        }
        else
        {
            var filteredQuestions = allQuestions
                .Where(q => q.QuestionText.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            return filteredQuestions.Select(q => new QuestionRecord(
                Id: q.Id.ToString(),
                QuestionText: q.QuestionText,
                AnswerOptions: q.AnswerOptions,
                CorrectAnswerIndex: q.CorrectAnswerIndex,
                Category: q.Category.ToString()
            ));

        }

    }

    public void AddQuestion(Question newQuestion)
    {
        _question.InsertOne(newQuestion);
    }

    public void UpdateQuestion(QuestionRecord updatedQuestionRecord)
    {
        var filter = Builders<Question>.Filter.Eq("_id", ObjectId.Parse(updatedQuestionRecord.Id));
        var update = Builders<Question>.Update
            .Set(q => q.QuestionText, updatedQuestionRecord.QuestionText)
            .Set(q => q.AnswerOptions, updatedQuestionRecord.AnswerOptions)
            .Set(q => q.Category, updatedQuestionRecord.Category)
            .Set(q => q.CorrectAnswerIndex, updatedQuestionRecord.CorrectAnswerIndex);
        
        _question.UpdateOne(filter, update);
    }


    public void RemoveQuestion(string id)
    {
        var filter = Builders<Question>.Filter.Eq("_id", ObjectId.Parse(id));
        _question.DeleteOne(filter);
    }

    public void AddQuiz(Quiz newQuiz)
    {
        _quiz.InsertOne(newQuiz);
    }

    public void UpdateQuiz(QuizRecord updatedQuizRecord)
    {

        var filter = Builders<Quiz>.Filter.Eq("_id", ObjectId.Parse(updatedQuizRecord.id));
        var update = Builders<Quiz>.Update
            .Set(q => q.QuizTitle, updatedQuizRecord.quizTitle)
            .Set(q => q.QuizDescription, updatedQuizRecord.quizDescription);

        _quiz.UpdateOne(filter, update);
    }

    public void RemoveQuiz(string id)
    {
        var filter = Builders<Quiz>.Filter.Eq("_id", ObjectId.Parse(id));
        _quiz.DeleteOne(filter);
    }

    public void AddQuestionToQuiz(QuizRecord newQuizRecord, QuestionRecord newQuestionRecord)
    {
        var quizFilter = Builders<Quiz>.Filter.Eq("_id", ObjectId.Parse(newQuizRecord.id));
        var newQuestionObjectId = ObjectId.Parse(newQuestionRecord.Id);
        var update = Builders<Quiz>.Update
            .AddToSet("Questions", newQuestionObjectId);

        _quiz.UpdateOne(quizFilter, update);
    }

    public void RemoveQuestionFromQuiz(QuizRecord quizRecord, QuestionRecord questionRecord)
    {
        var quizFilter = Builders<Quiz>.Filter.Eq("_id", ObjectId.Parse(quizRecord.id));
        var questionObjectId = ObjectId.Parse(questionRecord.Id);

        var update = Builders<Quiz>.Update.Pull("Questions", questionObjectId);

        _quiz.UpdateOne(quizFilter, update);
    }

    public void AddCategory(CategoryRecord newCategoryRecord)
    {
        var category = new Category()
        {
            CategoryName = newCategoryRecord.categoryName
        };

        _category.InsertOne(category);
    }
}



