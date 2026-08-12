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
        if (expenseToRemove == null)
        {
            return Result.Failure("Expense not found.");
        }

        _ = category.Expenses?.Remove(expenseToRemove);
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
        if (totalExpense.IsFailure)
        {
            return Result<decimal>.Failure(totalExpense.ErrorMessage);
        }

        var remaining = category.AllocatedBudget - totalExpense.Value;
        if (remaining < 0)
        {
            return Result<decimal>.Failure($"Category budget exceeded by {Math.Abs(remaining)}.");
        }

        return Result<decimal>.Success(remaining);
    }

    public Result<bool> IsCategoryLimitExceeded(BudgetCategory category)
    {
        Guard.AgainstNull(category, nameof(category));

        var totalExpenseResult = GetTotalExpense(category);
        if (totalExpenseResult.IsFailure)
        {
            return Result<bool>.Failure(totalExpenseResult.ErrorMessage);
        }

        var totalExpense = totalExpenseResult.Value;
        return Result<bool>.Success(totalExpense > category.AllocatedBudget);
    }

    public Result<bool> CanAddExpense(BudgetCategory category, decimal amount)
    {
        Guard.AgainstNull(category, nameof(category));
        Guard.AgainstZeroOrNegative(amount, nameof(amount));

        var totalExpenseResult = GetTotalExpense(category);
        if (totalExpenseResult.IsFailure)
        {
            return Result<bool>.Failure(totalExpenseResult.ErrorMessage);
        }

        var totalExpense = totalExpenseResult.Value;
        if (totalExpense + amount <= category.AllocatedBudget)
        {
            return Result<bool>.Success(true);
        }

        return Result<bool>.Failure("Insufficient remaining category budget to add the expense.");
    }
}
