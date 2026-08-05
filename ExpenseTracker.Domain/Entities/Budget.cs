using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Entities;

public class Budget : BaseEntity
{
    /// <summary>
    /// Gets the month of the budget based on the ExpenseDate property.
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// Gets the year of the budget based on the ExpenseDate property.
    /// </summary>
    public int Year {  get; set; }

    /// <summary>
    /// Gets or sets the total budget amount.
    /// </summary>
    public decimal TotalBudget { get; set; }

    /// <summary>
    /// Gets or sets the collection of budget categories associated with the budget.
    /// </summary>
    public ICollection<BudgetCategory> BudgetCategories { get; set; } = new List<BudgetCategory>();
}
