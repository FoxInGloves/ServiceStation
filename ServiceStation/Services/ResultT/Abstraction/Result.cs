namespace ServiceStation.Services.ResultT.Abstraction;

public class Result
{
    protected Result()
    {
        IsSuccess = true;
        Error = default;
    }
    
    protected Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }
    
    public bool IsSuccess { get; }
    
    public Error? Error { get; }
    
    public static implicit operator Result(Error error) =>
        new(error);
    
    public static Result Success() =>
        new();
    
    public static Result Failure(Error error) =>
        new(error);
}