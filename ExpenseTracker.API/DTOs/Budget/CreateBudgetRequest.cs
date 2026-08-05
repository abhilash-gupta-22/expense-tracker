using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs.Budget;

public class CreateBudgetRequest
{
    [Range(1, 12)]
    public int Month { get; set; }

    [Range(2020, 2100)]
    public int Year { get; set; }

    [Range(1, double.MaxValue)]
    public decimal TotalBudget { get; set; }
}