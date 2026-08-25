namespace ExpenseTracker.Web.Models.Dashboard;

public class DashboardSummaryModel
{
    /// <summary>
    /// Gets or sets the total monthly budget.
    /// </summary>
    public decimal TotalBudget { get; set; }

    /// <summary>
    /// Gets or sets the total amount spent.
    /// </summary>
    public decimal TotalExpenses { get; set; }

    /// <summary>
    /// Gets the remaining budget amount.
    /// </summary>
    public decimal RemainingBudget => TotalBudget - TotalExpenses;

    /// <summary>
    /// Gets or sets the total number of categories.
    /// </summary>
    public int TotalCategories { get; set; }

    /// <summary>
    /// Gets the percentage of the budget that has been spent.
    /// </summary>
    public decimal SpentPercentage => TotalBudget == 0
        ? 0
        : Math.Round((TotalExpenses / TotalBudget) * 100, 2);
}
