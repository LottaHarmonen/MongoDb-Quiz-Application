using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Entities;

public class Question
{
    public ObjectId Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;

    public List<string> AnswerOptions { get; set; }

    public string Category { get; set; }

    public int CorrectAnswerIndex { get; set; }

}