namespace ExpenseTracker.Domain.Common;

public static class Guard
{
    public static void AgainstNull(object? value, string parameterName)
    {
        if (value == null)
        {
            throw new ArgumentNullException("value");
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
            throw new ArgumentOutOfRangeException("amount", "Amount cannot be negative.");
        }
    }

    public static void AgainstZeroOrNegative(decimal value, string parameterName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException("value", "Value cannot be zero or negative.");
        }
    }

    public static void AgainstInvalidDate(DateTime value, string parameterName)
    {
        if (value == DateTime.MinValue || value == DateTime.MaxValue)
        {
            throw new ArgumentOutOfRangeException("value", "Invalid date provided.");
        }
    }
}
