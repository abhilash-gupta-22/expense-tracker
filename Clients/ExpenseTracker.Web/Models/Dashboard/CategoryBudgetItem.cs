namespace ExpenseTracker.Web.Models.Dashboard;

public class CategoryBudgetItem
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal AllocatedBudget { get; set; }

    public decimal SpentAmount { get; set; }
}
