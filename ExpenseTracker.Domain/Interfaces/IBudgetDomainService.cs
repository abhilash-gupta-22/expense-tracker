using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Interfaces;

public interface IBudgetDomainService
{
    /// <summary>
    /// Creates a new budget for the specified month and year with the given total budget amount.
    /// </summary>
    /// <param name="totalBudget"></param>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    Budget CreateBudget(decimal totalBudget, int month, int year);

    /// <summary>
    /// Adds a new category to the specified budget.
    /// </summary>
    /// <param name="budget"></param>
    /// <param name="category"></param>
    void AddCategory(Budget budget, BudgetCategory category);

    /// <summary>
    /// Updates the allocated amount for a specific category within the specified budget.
    /// </summary>
    /// <param name="budget"></param>
    /// <param name="categoryId"></param>
    /// <param name="allocatedBudget"></param>
    void UpdateCategoryAllocation(Budget budget, Guid categoryId, decimal allocatedBudget);

    /// <summary>
    /// Calculates the total allocated amount for the specified budget by summing up the allocated amounts of all its categories.
    /// </summary>
    /// <param name="budget"></param>
    /// <returns></returns>
    decimal GetAllocatedAmount(Budget budget);

    /// <summary>
    /// Calculates the remaining budget for the specified budget by subtracting the total allocated amount from the total budget.
    /// </summary>
    /// <param name="budget"></param>
    /// <returns></returns>
    decimal GetRemainingBudget(Budget budget);

    /// <summary>
    /// Validates whether the total allocated amount for the specified budget does not exceed the total budget amount.
    /// </summary>
    /// <param name="budget"></param>
    /// <returns></returns>
    bool IsBudgetAllocationValid(Budget budget);
}
