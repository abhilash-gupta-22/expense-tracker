using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs.Budget;

public class UpdateBudgetRequest
{
    [Range(1, double.MaxValue)]
    public decimal TotalBudget { get; set; }
}