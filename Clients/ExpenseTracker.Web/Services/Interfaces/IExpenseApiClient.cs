using ExpenseTracker.Web.Models.Expense;

namespace ExpenseTracker.Web.Services.Interfaces;

public interface IExpenseApiClient
{
    /// <summary>
    /// Gets all expenses.
    /// </summary>
    Task<IEnumerable<ExpenseModel>> GetAllAsync();

    /// <summary>
    /// Gets an expense by its unique identifier.
    /// </summary>
    Task<ExpenseModel?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all expenses associated with a specific budget.
    /// </summary>
    /// <param name="budgetId"></param>
    /// <returns></returns>
    Task<IEnumerable<ExpenseModel>> GetByBudgetIdAsync(Guid budgetId);

    /// <summary>
    /// Gets all expenses associated with a specific budget category.
    /// </summary>
    Task<IEnumerable<ExpenseModel>> GetByCategoryIdAsync(Guid budgetCategoryId);

    /// <summary>
    /// Creates a new expense.
    /// </summary>
    Task<ExpenseModel> CreateAsync(CreateExpenseModel model);

    /// <summary>
    /// Updates an existing expense.
    /// </summary>
    Task UpdateAsync(Guid id, UpdateExpenseModel model);

    /// <summary>
    /// Deletes an expense.
    /// </summary>
    Task DeleteAsync(Guid id);
}
