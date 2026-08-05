namespace ExpenseTracker.API.DTOs.Expense;

public class ExpenseResponse
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime ExpenseDate { get; set; }

    public string? Remarks { get; set; }
}
