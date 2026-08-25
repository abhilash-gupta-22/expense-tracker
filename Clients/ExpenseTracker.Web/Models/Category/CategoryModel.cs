namespace ExpenseTracker.Web.Models.Category;

public class CategoryModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the category.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the budget
    /// to which this category belongs.
    /// </summary>
    public Guid BudgetId { get; set; }

    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the allocated budget amount
    /// for this category.
    /// </summary>
    public decimal AllocatedBudget { get; set; }
}
