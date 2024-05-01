namespace DTOs;

public record CategoryRecord(string categoryName)
{
    public override string ToString()
    {
        return categoryName;
    }
}

