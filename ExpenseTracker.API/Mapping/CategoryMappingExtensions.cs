using ExpenseTracker.API.DTOs.Category;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.API.Mapping;

public static class CategoryMappingExtensions
{
    public static CategoryResponse ToResponse(this BudgetCategory category)
    {
        var totalExpense = category.Expenses.Sum(x => x.Amount);

        return new CategoryResponse
        {
            Id = category.Id,
            BudgetId = category.BudgetId,
            Name = category.Name,
            AllocatedAmount = category.AllocatedBudget,
            TotalExpense = totalExpense,
            RemainingAmount = category.AllocatedBudget - totalExpense
        };
    }

    public static IEnumerable<CategoryResponse> ToResponse(this IEnumerable<BudgetCategory> categories)
    {
        return categories.Select(c => c.ToResponse());
    }
}