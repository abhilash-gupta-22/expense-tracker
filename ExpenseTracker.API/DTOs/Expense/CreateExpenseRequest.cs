using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs.Expense;

public class CreateExpenseRequest
{
    [Required]
    public Guid CategoryId { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public DateTime ExpenseDate { get; set; }

    [StringLength(250)]
    public string? Remarks { get; set; }
}
