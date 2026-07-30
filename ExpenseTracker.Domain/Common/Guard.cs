namespace ExpenseTracker.Domain.Common;

public static class Guard
{
    public static void AgainstNull(object? value, string parameterName)
    {
        //TODO: Implement the logic to check for null and throw an exception if the value is null.
    }

    public static void AgainstNullOrEmpty(string? value, string parameterName)
    {
        //TODO: Implement the logic to check for null or empty string and throw an exception if the value is null or empty.
    }

    public static void AgainstNegativeAmount(decimal amount, string parameterName)
    {
        //TODO: Implement the logic to check for negative amount and throw an exception if the amount is negative.
    }

    public static void AgainstZeroOrNegative(decimal value, string parameterName)
    {
        //TODO: Implement the logic to check for zero or negative value and throw an exception if the value is zero or negative.
    }

    public static void AgainstInvalidDate(DateTime value, string parameterName)
    {
        //TODO: Implement the logic to check for invalid date and throw an exception if the date is invalid.
    }
}
