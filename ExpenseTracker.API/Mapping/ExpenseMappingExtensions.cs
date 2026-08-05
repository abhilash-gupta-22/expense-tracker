using ExpenseTracker.API.DTOs.Expense;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.API.Mapping;

public static class ExpenseMappingExtensions
{
    public static ExpenseResponse ToResponse(this Expense expense)
    {
        return new ExpenseResponse
        {
            Id = expense.Id,
            CategoryId = expense.BudgetCategoryId,
            CategoryName = expense.BudgetCategoryName ?? string.Empty,
            Amount = expense.Amount,
            ExpenseDate = expense.ExpenseDate,
            Remarks = expense.Remarks
        };
    }

    public static IEnumerable<ExpenseResponse> ToResponse(this IEnumerable<Expense> expenses)
    {
        return expenses.Select(e => e.ToResponse());
    }
}
