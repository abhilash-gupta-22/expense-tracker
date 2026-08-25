namespace ExpenseTracker.Web.Models.Dashboard;

public class RecentExpenseModel
{
    /// <summary>
    /// Gets or sets the expense identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the category name.
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the expense amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the date of the expense.
    /// </summary>
    public DateTime ExpenseDate { get; set; }

    /// <summary>
    /// Gets or sets the expense remarks.
    /// </summary>
    public string Remarks { get; set; } = string.Empty;
}
