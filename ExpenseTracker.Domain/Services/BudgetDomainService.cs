#nullable enable
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Domain.Services;

public class BudgetDomainService : IBudgetDomainService
{
    public Result<Budget> CreateBudget(decimal totalBudget, int month, int year)
    {
        Guard.AgainstZeroOrNegative(totalBudget, nameof(totalBudget));
        Guard.AgainstInvalidMonth(month, nameof(month));
        Guard.AgainstInvalidYear(year, nameof(year));

        var budget = new Budget
        {
            Month = month,
            Year = year,
            TotalBudget = totalBudget
        };

        return Result<Budget>.Success(budget);
    }

    public Result UpdateBudget(Budget budget, decimal totalbudget)
    {
        Guard.AgainstNull(budget, nameof(budget));
        Guard.AgainstZeroOrNegative(totalbudget, nameof(totalbudget));

        budget.TotalBudget = totalbudget;
        return Result.Success();
    }

    public Result AddCategory(Budget budget, BudgetCategory category)
    {
        Guard.AgainstNull(budget, nameof(budget));
        Guard.AgainstNull(category, nameof(category));

        if (budget.BudgetCategories.Any(x => x.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return Result.Failure($"Category '{category.Name}' already exists.");
        }

        var allocatedAmount = budget.BudgetCategories.Sum(x => x.AllocatedBudget);

        if (allocatedAmount + category.AllocatedBudget > budget.TotalBudget)
        {
            return Result.Failure("Category allocation exceeds the total budget.");
        }

        budget.BudgetCategories.Add(category);

        return Result.Success();
    }

    public Result UpdateCategoryAllocation(Budget budget, Guid categoryId, decimal allocatedBudget)
    {
        Guard.AgainstNull(budget, nameof(budget));
        Guard.AgainstNullOrEmptyGuid(categoryId, nameof(categoryId));
        Guard.AgainstZeroOrNegative(allocatedBudget, nameof(allocatedBudget));

        var budgetCategory = budget.BudgetCategories.FirstOrDefault(c => c.Id == categoryId);
        if (budgetCategory != null)
        {
            budgetCategory.AllocatedBudget = allocatedBudget;
            return Result.Success();
        }

        return Result.Failure("Category not found.");
    }

    public Result RemoveCategory(Budget budget, Guid categoryId)
    {
        Guard.AgainstNull(budget, nameof(budget));
        Guard.AgainstNullOrEmptyGuid(categoryId, nameof(categoryId));

        var categoryToRemove = budget.BudgetCategories.FirstOrDefault(c => c.Id == categoryId);

        if (categoryToRemove != null)
        {
            _ = budget.BudgetCategories.Remove(categoryToRemove);
            return Result.Success();
        }

        return Result.Failure("Category not found.");
    }

    public Result<decimal> GetAllocatedAmount(Budget budget)
    {
        Guard.AgainstNull(budget, nameof(budget));

        return Result<decimal>.Success(budget.BudgetCategories.Sum(c => c.AllocatedBudget));
    }

    public Result<decimal> GetRemainingBudget(Budget budget)
    {
        Guard.AgainstNull(budget, nameof(budget));

        var totalExpense = budget.BudgetCategories.Sum(c => c.Expenses.Sum(e => e.Amount));
        return Result<decimal>.Success(budget.TotalBudget - totalExpense);
    }

    public Result<bool> CanAllocateBudget(Budget budget, decimal amount)
    {
        Guard.AgainstNull(budget, nameof(budget));
        Guard.AgainstZeroOrNegative(amount, nameof(amount));

        var totalAllocated = budget.BudgetCategories.Sum(c => c.AllocatedBudget);

        return Result<bool>.Success((totalAllocated + amount) <= budget.TotalBudget);
    }

    public Result<bool> IsBudgetAllocationValid(Budget budget)
    {
        Guard.AgainstNull(budget, nameof(budget));

        if (budget.TotalBudget < budget.BudgetCategories.Sum(c => c.AllocatedBudget))
        {
            return Result<bool>.Success(false);
        }

        return Result<bool>.Success(true);
    }
}
