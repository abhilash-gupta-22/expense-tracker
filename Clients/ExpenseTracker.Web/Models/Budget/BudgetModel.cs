using ExpenseTracker.Web.Models.Category;

namespace ExpenseTracker.Web.Models.Budget;

public class BudgetModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the budget.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the month of the budget.
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// Gets or sets the year of the budget.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Gets or sets the total budget amount.
    /// </summary>
    public decimal TotalBudget { get; set; }

    /// <summary>
    /// Gets or sets the categories associated with this budget.
    /// </summary>
    public ICollection<CategoryModel> BudgetCategories { get; set; } = new List<CategoryModel>();
}
