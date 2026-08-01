using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Interfaces;

public interface ICategoryRepository : IBaseRepository<BudgetCategory>
{
    /// <summary>
    /// Retrieves all budget categories associated with a specific budget ID.
    /// </summary>
    /// <param name="budgetId"></param>
    /// <returns></returns>
    Task<IEnumerable<BudgetCategory>> GetCategoriesByBudgetAsync(Guid budgetId);

    /// <summary>
    /// Retrieves a budget category along with its associated expenses based on the provided category ID.
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    Task<BudgetCategory?> GetCategoryWithExpensesAsync(Guid categoryId);

    /// <summary>
    /// Checks if a category with the specified name exists within the given budget.
    /// </summary>
    /// <param name="budgetId"></param>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    Task<bool> CategoryExistsAsync(Guid budgetId, string categoryName);
}
