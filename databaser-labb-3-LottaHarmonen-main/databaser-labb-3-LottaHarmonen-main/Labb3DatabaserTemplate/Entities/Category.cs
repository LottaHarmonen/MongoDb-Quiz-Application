using MongoDB.Bson;

namespace DataAccess.Entities;

public class Category
{
    public ObjectId Id { get; set; }

    public string CategoryName { get; set; }

    
}