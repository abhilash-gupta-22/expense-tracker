namespace ExpenseTracker.API.DTOs.Budget;

public class BudgetResponse
{
    public Guid Id { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TotalBudget { get; set; }
    public decimal AllocatedAmount { get; set; }
    public decimal RemainingAmount { get; set; }
}