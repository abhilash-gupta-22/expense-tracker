namespace ExpenseTracker.Web.Models.Dashboard;

public class CategorySummaryModel
{
    /// <summary>
    /// Gets or sets the category identifier.
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the category name.
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the allocated budget for the category.
    /// </summary>
    public decimal AllocatedBudget { get; set; }

    /// <summary>
    /// Gets or sets the total expenses for the category.
    /// </summary>
    public decimal TotalExpenses { get; set; }

    /// <summary>
    /// Gets the remaining budget for the category.
    /// </summary>
    public decimal RemainingBudget => AllocatedBudget - TotalExpenses;

    /// <summary>
    /// Gets the percentage of the allocated budget that has been spent.
    /// </summary>
    public decimal SpentPercentage => AllocatedBudget == 0
        ? 0
        : Math.Round((TotalExpenses / AllocatedBudget) * 100, 2);
}
