namespace ExpenseTracker.Web.State;

public class BudgetState
{
    public int SelectedMonth { get; private set; } = DateTime.Now.Month;

    public int SelectedYear { get; private set; } = DateTime.Now.Year;

    public event Action? OnChange;

    public void SetSelectedMonth(int month, int year)
    {
        SelectedMonth = month;
        SelectedYear = year;

        OnChange?.Invoke();
    }
}
