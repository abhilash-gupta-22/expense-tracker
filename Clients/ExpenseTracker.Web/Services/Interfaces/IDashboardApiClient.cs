using ExpenseTracker.Web.Models.Dashboard;

namespace ExpenseTracker.Web.Services.Interfaces;

public interface IDashboardApiClient
{
    /// <summary>
    /// Gets the dashboard summary for the specified month and year.
    /// </summary>
    Task<DashboardSummaryModel> GetSummaryAsync(int month, int year);

    /// <summary>
    /// Gets category-wise expense summaries for the specified month and year.
    /// </summary>
    Task<IEnumerable<CategorySummaryModel>> GetCategorySummaryAsync(int month, int year);

    /// <summary>
    /// Gets the most recent expenses for the specified month and year.
    /// </summary>
    Task<IEnumerable<RecentExpenseModel>> GetRecentExpensesAsync(int month, int year, int count = 5);

    /// <summary>
    /// Gets the expense trend for the specified month and year.
    /// </summary>
    Task<IEnumerable<ExpenseTrendModel>> GetExpenseTrendAsync(int month, int year);
}
