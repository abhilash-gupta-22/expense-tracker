using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Interfaces;

public interface IExpenseRepository : IBaseRepository<Expense>
{
    /// <summary>
    /// Gets all expenses for a specific budget.
    /// </summary>
    /// <param name="budgetId"></param>
    /// <returns></returns>
    Task<IEnumerable<Expense>> GetByBudgetIdAsync(Guid budgetId);

    /// <summary>
    /// Gets all expenses for a specific category.
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(Guid categoryId);

    /// <summary>
    /// Gets all expenses within a specific date range.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime from, DateTime to);

    /// <summary>
    /// Gets the total expense amount for a specific category.
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    Task<decimal> GetTotalExpenseAsync(Guid categoryId);
}
