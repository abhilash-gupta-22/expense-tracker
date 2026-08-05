using ExpenseTracker.API.DTOs.Budget;

namespace ExpenseTracker.API.Mapping;

public static class BudgetMappingExtensions
{
    public static BudgetResponse ToResponse(this Domain.Entities.Budget budget)
    {
        return new BudgetResponse
        {
            Id = budget.Id,
            Month = budget.Month,
            Year = budget.Year,
            TotalBudget = budget.TotalBudget,
            AllocatedAmount = budget.BudgetCategories.Sum(c => c.AllocatedBudget),
            RemainingAmount = budget.TotalBudget - budget.BudgetCategories.Sum(c => c.AllocatedBudget)
        };
    }
}
