namespace DTOs;

public record QuestionRecord(
    string Id,
    string QuestionText,
    List<string> AnswerOptions,
    int CorrectAnswerIndex,
    string Category)
{
  
}
