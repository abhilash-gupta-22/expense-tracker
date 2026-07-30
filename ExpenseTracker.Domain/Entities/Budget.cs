using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Entities;

public class Budget : BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the budget.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets the month of the budget based on the ExpenseDate property.
    /// </summary>
    public string Month => ExpenseDate?.ToString("MMMM") ?? string.Empty;

    /// <summary>
    /// Gets the year of the budget based on the ExpenseDate property.
    /// </summary>
    public string Year => ExpenseDate?.ToString("yyyy") ?? string.Empty;

    /// <summary>
    /// Gets or sets the date of the budget, which is used to determine the month and year of the budget.
    /// </summary>
    public DateTime? ExpenseDate { get; set; }

    /// <summary>
    /// Gets or sets the total budget amount.
    /// </summary>
    public decimal TotalBudget { get; set; }

    /// <summary>
    /// Gets or sets the collection of budget categories associated with the budget.
    /// </summary>
    public ICollection<BudgetCategory> BudgetCategories { get; set; } = new List<BudgetCategory>();
}
