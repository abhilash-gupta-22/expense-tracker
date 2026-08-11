using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Interfaces;

public interface IExpenseDomainService
{
    /// <summary>
    /// Adds an expense to a budget category and updates the total expense for that category.
    /// </summary>
    /// <param name="category"></param>
    /// <param name="expense"></param>
    Result AddExpense(BudgetCategory category, Expense expense);

    /// <summary>
    /// Removes an expense from a budget category and updates the total expense for that category.
    /// </summary>
    /// <param name="category"></param>
    /// <param name="expenseId"></param>
    Result RemoveExpense(BudgetCategory category, Guid expenseId);

    /// <summary>
    /// Calculates the total expense for a given budget category.
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    Result<decimal> GetTotalExpense(BudgetCategory category);

    /// <summary>
    /// Calculates the remaining budget for a given budget category.
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    Result<decimal> GetRemainingCategoryBudget(BudgetCategory category);

    /// <summary>
    /// Checks if the total expense for a given budget category exceeds the allocated budget.
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    Result<bool> IsCategoryLimitExceeded(BudgetCategory category);

    /// <summary>
    /// Checks if an expense can be added to a budget category without exceeding the allocated budget.
    /// </summary>
    /// <param name="category"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    Result<bool> CanAddExpense(BudgetCategory category, decimal amount);
}
