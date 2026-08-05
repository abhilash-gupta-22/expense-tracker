namespace ExpenseTracker.Domain.Common;

public static class Guard
{
    public static void AgainstNull(object? value, string parameterName)
    {
        if (value == null)
        {
            throw new ArgumentNullException("Value cannot be null.", parameterName);
        }
    }

    public static void AgainstNullOrEmpty(string? value, string parameterName)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Value cannot be null or empty.", parameterName);
        }
    }

    public static void AgainstNegativeAmount(decimal amount, string parameterName)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Amount cannot be negative.");
        }
    }

    public static void AgainstZeroOrNegative(decimal value, string parameterName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Value cannot be zero or negative.");
        }
    }

    public static void AgainstInvalidDate(DateTime value, string parameterName)
    {
        if (value == DateTime.MinValue || value == DateTime.MaxValue)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Invalid date provided.");
        }
    }

    public static void AgainstInvalidMonth(int month, string parameterName)
    {
        if (month < 1 || month > 12)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Month must be between 1 and 12.");
        }
    }

    public static void AgainstInvalidYear(int year, string parameterName)
    {
        if (year < 1)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Year must be a positive integer.");
        }
    }
}
