using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Domain.Services;

public class BudgetDomainService : IBudgetDomainService
{
    public Budget CreateBudget(decimal totalBudget, int month, int year)
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

        return budget;
    }

    public void AddCategory(Budget budget, BudgetCategory category)
    {
        Guard.AgainstNull(budget, nameof(budget));
        Guard.AgainstNull(category, nameof(category));

        if (!budget.BudgetCategories.Contains(category))
        {
            budget.BudgetCategories.Add(category);
        }
    }

    public void UpdateCategoryAllocation(Budget budget, Guid categoryId, decimal allocatedBudget)
    {
        Guard.AgainstNull(budget, nameof(budget));
        Guard.AgainstNull(categoryId, nameof(categoryId));
        Guard.AgainstZeroOrNegative(allocatedBudget, nameof(allocatedBudget));

        var budgetCategory = budget.BudgetCategories.FirstOrDefault(c => c.Id == categoryId);
        if (budgetCategory != null)
        {
            budgetCategory.AllocatedBudget = allocatedBudget;
        }
    }

    public decimal GetAllocatedAmount(Budget budget)
    {
        Guard.AgainstNull(budget, nameof(budget));

        return budget.BudgetCategories.Sum(c => c.AllocatedBudget);
    }

    public decimal GetRemainingBudget(Budget budget)
    {
        Guard.AgainstNull(budget, nameof(budget));

        var totalExpense = budget.BudgetCategories.Sum(c => c.Expenses.Sum(e => e.Amount));
        return budget.TotalBudget - totalExpense;
    }

    public bool IsBudgetAllocationValid(Budget budget)
    {
        Guard.AgainstNull(budget, nameof(budget));

        foreach (var category in budget.BudgetCategories)
        {
            if (category.AllocatedBudget < category.Expenses.Sum(x => x.Amount))
            {
                return false;
            }
        }

        return true;
    }
}
