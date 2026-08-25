using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Web.Models.Expense;

public class UpdateExpenseModel
{
    /// <summary>
    /// Gets or sets the associated budget category identifier.
    /// </summary>
    [Required]
    public Guid BudgetCategoryId { get; set; }

    /// <summary>
    /// Gets or sets the expense amount.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Expense amount must be greater than zero.")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the expense date.
    /// </summary>
    [Required(ErrorMessage = "Expense date is required.")]
    public DateTime ExpenseDate { get; set; }

    /// <summary>
    /// Gets or sets additional remarks.
    /// </summary>
    [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters.")]
    public string Remarks { get; set; } = string.Empty;
}
