using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Domain.Services;

public class ExpenseDomainService : IExpenseDomainService
{
    public void AddExpense(BudgetCategory category, Expense expense)
    {
        Guard.AgainstNull(category, nameof(category));
        Guard.AgainstNull(expense, nameof(expense));

        category.Expenses ??= new List<Expense>();
        category.Expenses.Add(expense);
    }

    public void RemoveExpense(BudgetCategory category, Guid expenseId)
    {
        Guard.AgainstNull(category, nameof(category));
        Guard.AgainstNullOrEmpty(expenseId.ToString(), nameof(expenseId));

        var expenseToRemove = category.Expenses?.FirstOrDefault(e => e.Id == expenseId);
        if (expenseToRemove != null)
        {
            _ = category.Expenses?.Remove(expenseToRemove);
        }
    }

    public decimal GetTotalExpense(BudgetCategory category)
    {
        Guard.AgainstNull(category, nameof(category));

        if (category.Expenses == null || !category.Expenses.Any())
        {
            return 0;
        }

        return category.Expenses.Sum(e => e.Amount);
    }

    public decimal GetRemainingCategoryBudget(BudgetCategory category)
    {
        Guard.AgainstNull(category, nameof(category));

        var totalExpense = GetTotalExpense(category);
        return category.AllocatedBudget - totalExpense;
    }

    public bool IsCategoryLimitExceeded(BudgetCategory category)
    {
        Guard.AgainstNull(category, nameof(category));

        var totalExpense = GetTotalExpense(category);

        if (totalExpense > category.AllocatedBudget)
        {
            return true;
        }

        return false;
    }

    public bool CanAddExpense(BudgetCategory category, decimal amount)
    {
        Guard.AgainstNull(category, nameof(category));
        Guard.AgainstZeroOrNegative(amount, nameof(amount));

        var totalExpense = GetTotalExpense(category);
        return totalExpense + amount <= category.AllocatedBudget;
    }
}
