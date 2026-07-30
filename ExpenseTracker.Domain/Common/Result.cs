namespace ExpenseTracker.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public string ErrorMessage { get; }

    protected Result(bool isSuccess, string errorMessage)
    {
        //TODO: Implement the constructor to initialize the Result instance with the provided success status and error message.
    }

    public static Result Success()
    {
        // TODO: Implement the Success method to return a successful Result instance.
        return null;
    }

    public static Result Failure(string error)
    {
        // TODO: Implement the Failure method to return a failed Result instance with the provided error message.
        return null;
    }
}
