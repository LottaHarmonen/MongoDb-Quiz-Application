using MongoDB.Bson;

namespace DataAccess.Entities;

public class Quiz
{
    public ObjectId Id { get; set; }
    public string QuizTitle { get; set; } = string.Empty;
    public string QuizDescription { get; set;}

    public List<ObjectId> Questions { get; set; } = new List<ObjectId>();

}