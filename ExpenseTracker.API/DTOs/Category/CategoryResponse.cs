namespace ExpenseTracker.API.DTOs.Category;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public Guid BudgetId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal AllocatedAmount { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal RemainingAmount { get; set; }
}
