using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Services;

public class ExpenseDomainService : IExpenseDomainService
{
    public void AddExpense(BudgetCategory category, Expense expense)
    {
        // TODO: Implement the logic to add an expense to the specified budget category.
    }

    public void RemoveExpense(BudgetCategory category, Guid expenseId)
    {
        // TODO: Implement the logic to remove an expense from the specified budget category by expenseId.
    }

    public decimal GetTotalExpense(BudgetCategory category)
    {
        // TODO: Implement the logic to get the total expense for the specified budget category.
        return 0;
    }

    public decimal GetRemainingCategoryBudget(BudgetCategory category)
    {
        // TODO: Implement the logic to get the remaining budget for the specified budget category.
        return 0;
    }

    public bool IsCategoryLimitExceeded(BudgetCategory category)
    {
        // TODO: Implement the logic to check if the specified budget category has exceeded its limit.
        return false;
    }

    public bool CanAddExpense(BudgetCategory category, decimal amount)
    {
        // TODO: Implement the logic to check if an expense of the specified amount can be added to the budget category.
        return false;
    }
}
