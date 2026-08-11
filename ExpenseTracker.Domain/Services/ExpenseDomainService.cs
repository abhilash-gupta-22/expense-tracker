using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Domain.Services;

public class ExpenseDomainService : IExpenseDomainService
{
    public Result AddExpense(BudgetCategory category, Expense expense)
    {
        Guard.AgainstNull(category, nameof(category));
        Guard.AgainstNull(expense, nameof(expense));

        category.Expenses ??= new List<Expense>();
        category.Expenses.Add(expense);

        return Result.Success();
    }

    public Result RemoveExpense(BudgetCategory category, Guid expenseId)
    {
        Guard.AgainstNull(category, nameof(category));
        Guard.AgainstNullOrEmptyGuid(expenseId, nameof(expenseId));

        var expenseToRemove = category.Expenses?.FirstOrDefault(e => e.Id == expenseId);
        if (expenseToRemove != null)
        {
            _ = category.Expenses?.Remove(expenseToRemove);
        }

        return Result.Success();
    }

    public Result<decimal> GetTotalExpense(BudgetCategory category)
    {
        Guard.AgainstNull(category, nameof(category));

        if (category.Expenses == null || !category.Expenses.Any())
        {
            return Result<decimal>.Success(0);
        }

        return Result<decimal>.Success(category.Expenses.Sum(e => e.Amount));
    }

    public Result<decimal> GetRemainingCategoryBudget(BudgetCategory category)
    {
        Guard.AgainstNull(category, nameof(category));

        var totalExpense = GetTotalExpense(category);
        return Result<decimal>.Success(category.AllocatedBudget - totalExpense.Value);
    }

    public Result<bool> IsCategoryLimitExceeded(BudgetCategory category)
    {
        Guard.AgainstNull(category, nameof(category));

        var totalExpense = GetTotalExpense(category).Value;

        if (totalExpense > category.AllocatedBudget)
        {
            return Result<bool>.Success(true);
        }

        return Result<bool>.Success(false);
    }

    public Result<bool> CanAddExpense(BudgetCategory category, decimal amount)
    {
        Guard.AgainstNull(category, nameof(category));
        Guard.AgainstZeroOrNegative(amount, nameof(amount));

        var totalExpense = GetTotalExpense(category).Value;
        return Result<bool>.Success(totalExpense + amount <= category.AllocatedBudget);
    }
}
