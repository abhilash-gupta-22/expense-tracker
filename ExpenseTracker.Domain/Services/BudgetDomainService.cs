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

        var allocatedAmount = budget.BudgetCategories.Sum(x => x.AllocatedBudget);
        if (totalbudget < allocatedAmount)
        {
            return Result.Failure($"New total budget ({totalbudget}) cannot be less than already allocated amount ({allocatedAmount}).");
        }

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
            // Ensure updating this category's allocation does not push total allocations over the budget
            var totalOtherAllocations = budget.BudgetCategories.Where(c => c.Id != categoryId).Sum(c => c.AllocatedBudget);
            if (totalOtherAllocations + allocatedBudget > budget.TotalBudget)
            {
                return Result.Failure("Updated category allocation exceeds the total budget.");
            }

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
        var remaining = budget.TotalBudget - totalExpense;
        if (remaining < 0)
        {
            return Result<decimal>.Failure($"Budget exceeded by {Math.Abs(remaining)}.");
        }

        return Result<decimal>.Success(remaining);
    }

    public Result<bool> CanAllocateBudget(Budget budget, decimal amount)
    {
        Guard.AgainstNull(budget, nameof(budget));
        Guard.AgainstZeroOrNegative(amount, nameof(amount));

        var totalAllocated = budget.BudgetCategories.Sum(c => c.AllocatedBudget);

        if ((totalAllocated + amount) <= budget.TotalBudget)
        {
            return Result<bool>.Success(true);
        }

        return Result<bool>.Failure("Insufficient remaining budget to allocate the requested amount.");
    }

    public Result<bool> IsBudgetAllocationValid(Budget budget)
    {
        Guard.AgainstNull(budget, nameof(budget));

        var totalAllocated = budget.BudgetCategories.Sum(c => c.AllocatedBudget);
        if (budget.TotalBudget < totalAllocated)
        {
            return Result<bool>.Failure("Total allocated amount exceeds the total budget.");
        }

        return Result<bool>.Success(true);
    }
}
