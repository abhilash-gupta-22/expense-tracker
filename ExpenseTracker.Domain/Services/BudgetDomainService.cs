using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Services;

public class BudgetDomainService : IBudgetDomainService
{
    public Budget CreateBudget(int month, int year, decimal totalBudget)
    {
        //TODO: Implement the logic to create a new budget for the specified month and year with the given total budget.
        return null;
    }

    public void AddCategory(Budget budget, BudgetCategory category)
    {
        //TODO: Implement the logic to add a new category to the specified budget.
    }

    public void UpdateCategoryAllocation(Budget budget, Guid categoryId, decimal allocatedAmount)
    {
        //TODO: Implement the logic to update the allocated amount for a specific category in the specified budget.
    }

    public decimal GetAllocatedAmount(Budget budget)
    {
        //TODO: Implement the logic to calculate and return the total allocated amount for the specified budget.
        return 0;
    }

    public decimal GetRemainingBudget(Budget budget)
    {
        //TODO: Implement the logic to calculate and return the remaining budget for the specified budget.
        return 0;
    }

    public bool IsBudgetAllocationValid(Budget budget)
    {
        //TODO: Implement the logic to check if the total allocated amount for the specified budget does not exceed the total budget.
        return false;
    }
}
