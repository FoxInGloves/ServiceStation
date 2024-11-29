namespace ServiceStation.Services.ResultT.Abstraction;

public class Error
{
    private Error(string code, string description, ErrorType errorType)
    {
        Code = code;
        Description = description;
        ErrorType = errorType;
    }

    public string Code { get; }

    public string Description { get; }

    public ErrorType ErrorType { get; }

    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);
    
    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);
    
    public static Error Information(string code, string description) =>
        new(code, description, ErrorType.Information);
}