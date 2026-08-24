namespace ExpenseTracker.Web.Models.Budget;

public class BudgetModel
{
    public Guid Id { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public decimal TotalBudget { get; set; }
}
