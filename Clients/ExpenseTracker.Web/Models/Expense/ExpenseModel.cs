namespace ExpenseTracker.Web.Models.Expense;

public class ExpenseModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the expense.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the associated budget category identifier.
    /// </summary>
    public Guid BudgetCategoryId { get; set; }

    /// <summary>
    /// Gets or sets the name of the associated budget category.
    /// </summary>
    public string? BudgetCategoryName { get; set; }

    /// <summary>
    /// Gets or sets the expense amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the date of the expense.
    /// </summary>
    public DateTime ExpenseDate { get; set; }

    /// <summary>
    /// Gets or sets additional remarks for the expense.
    /// </summary>
    public string Remarks { get; set; } = string.Empty;
}
