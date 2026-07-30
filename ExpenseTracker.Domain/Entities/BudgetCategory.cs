using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Entities;

public class BudgetCategory : BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the budget category.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the budget category.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the allocated budget amount for the category.
    /// </summary>
    public decimal AllocatedBudget { get; set; }

    /// <summary>
    /// Gets or sets the collection of expenses associated with the budget category.
    /// </summary>
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
