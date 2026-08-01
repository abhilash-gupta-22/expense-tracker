using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Interfaces;

public interface IBudgetRepository : IBaseRepository<Budget>
{
    /// <summary>
    /// Gets the budget for a specific month and year.
    /// </summary>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    Task<Budget?> GetBudgetAsync(int month, int year);

    /// <summary>
    /// Checks if a budget exists for a specific month and year.
    /// </summary>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    Task<bool> BudgetExistsAsync(int month, int year);

    /// <summary>
    /// Gets a budget along with its associated categories.
    /// </summary>
    /// <param name="budgetId"></param>
    /// <returns></returns>
    Task<Budget?> GetBudgetWithCategoriesAsync(Guid budgetId);

    /// <summary>
    /// Gets a budget along with its associated categories and expenses.
    /// </summary>
    /// <param name="budgetId"></param>
    /// <returns></returns>
    Task<Budget?> GetBudgetCompleteAsync(Guid budgetId);
}
