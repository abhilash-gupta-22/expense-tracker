namespace ExpenseTracker.Web.Models.Budget;

public class CreateBudgetModel
{
    public int Month { get; set; }

    public int Year { get; set; }

    public decimal TotalBudget { get; set; }
}
