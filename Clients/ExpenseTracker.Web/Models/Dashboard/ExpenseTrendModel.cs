namespace ExpenseTracker.Web.Models.Dashboard;

public class ExpenseTrendModel
{
    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the total expense amount for the date.
    /// </summary>
    public decimal TotalAmount { get; set; }
}
