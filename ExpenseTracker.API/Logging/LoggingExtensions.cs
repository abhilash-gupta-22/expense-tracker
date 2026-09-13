using Microsoft.Extensions.Logging;

namespace ExpenseTracker.API.Logging;

public static class LoggingExtensions
{
    /// <summary>
    /// Configures application logging for the Expense Tracker API.
    /// </summary>
    public static ILoggingBuilder AddExpenseTrackerLogging(
        this ILoggingBuilder logging)
    {
        ArgumentNullException.ThrowIfNull(logging);

        logging.ClearProviders();

        // Console logging is useful during development
        // and works well with ASP.NET Core hosting environments.
        logging.AddConsole();

        // Debug output can be viewed from Visual Studio
        // while running the application.
        logging.AddDebug();

        logging.SetMinimumLevel(LogLevel.Information);

        return logging;
    }
}
