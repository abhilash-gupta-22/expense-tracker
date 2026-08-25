using ExpenseTracker.Web.Models.Dashboard;
using ExpenseTracker.Web.Services.Interfaces;

namespace ExpenseTracker.Web.Services;

public class DashboardApiClient : IDashboardApiClient
{
    private readonly IBudgetApiClient _budgetApiClient;
    private readonly ICategoryApiClient _categoryApiClient;
    private readonly IExpenseApiClient _expenseApiClient;

    public DashboardApiClient(IBudgetApiClient budgetApiClient, ICategoryApiClient categoryApiClient,
        IExpenseApiClient expenseApiClient)
    {
        ArgumentNullException.ThrowIfNull(budgetApiClient);
        ArgumentNullException.ThrowIfNull(categoryApiClient);
        ArgumentNullException.ThrowIfNull(expenseApiClient);

        _budgetApiClient = budgetApiClient;
        _categoryApiClient = categoryApiClient;
        _expenseApiClient = expenseApiClient;
    }

    public async Task<DashboardSummaryModel> GetSummaryAsync(int month, int year)
    {
        var budget = await _budgetApiClient.GetByMonthAsync(year, month).ConfigureAwait(false);

        if (budget is null)
        {
            return new DashboardSummaryModel();
        }

        var categories = await _categoryApiClient.GetByBudgetIdAsync(budget.Id).ConfigureAwait(false);

        var totalExpenses = 0m;

        foreach (var category in categories)
        {
            var expenses = await _expenseApiClient.GetByCategoryIdAsync(category.Id).ConfigureAwait(false);

            totalExpenses += expenses.Sum(x => x.Amount);
        }

        return new DashboardSummaryModel
        {
            TotalBudget = budget.TotalBudget,
            TotalExpenses = totalExpenses,
            TotalCategories = categories.Count()
        };
    }

    public async Task<IEnumerable<CategorySummaryModel>> GetCategorySummaryAsync(int month, int year)
    {
        var budget = await _budgetApiClient.GetByMonthAsync(year, month).ConfigureAwait(false);

        if (budget is null)
        {
            return Enumerable.Empty<CategorySummaryModel>();
        }

        var categories = await _categoryApiClient.GetByBudgetIdAsync(budget.Id).ConfigureAwait(false);

        var result = new List<CategorySummaryModel>();

        foreach (var category in categories)
        {
            var expenses = await _expenseApiClient.GetByCategoryIdAsync(category.Id).ConfigureAwait(false);

            var totalExpenses = expenses.Sum(x => x.Amount);

            result.Add(new CategorySummaryModel
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                AllocatedBudget = category.AllocatedBudget,
                TotalExpenses = totalExpenses
            });
        }

        return result;
    }

    public async Task<IEnumerable<RecentExpenseModel>> GetRecentExpensesAsync(int month, int year, int count = 5)
    {
        var budget = await _budgetApiClient.GetByMonthAsync(year, month).ConfigureAwait(false);

        if (budget is null)
        {
            return Enumerable.Empty<RecentExpenseModel>();
        }

        var categories = await _categoryApiClient.GetByBudgetIdAsync(budget.Id).ConfigureAwait(false);

        var allExpenses = new List<RecentExpenseModel>();

        foreach (var category in categories)
        {
            var expenses = await _expenseApiClient.GetByCategoryIdAsync(category.Id).ConfigureAwait(false);

            var recentExpenses = expenses.Select(expense => new RecentExpenseModel
            {
                Id = expense.Id,
                CategoryName = category.Name,
                Amount = expense.Amount,
                ExpenseDate = expense.ExpenseDate,
                Remarks = expense.Remarks
            });

            allExpenses.AddRange(recentExpenses);
        }

        return allExpenses.OrderByDescending(x => x.ExpenseDate).Take(count);
    }

    public async Task<IEnumerable<ExpenseTrendModel>> GetExpenseTrendAsync(int month, int year)
    {
        var budget = await _budgetApiClient.GetByMonthAsync(year, month).ConfigureAwait(false);

        if (budget is null)
        {
            return Enumerable.Empty<ExpenseTrendModel>();
        }

        var categories = await _categoryApiClient.GetByBudgetIdAsync(budget.Id).ConfigureAwait(false);

        var allExpenses = new List<Models.Expense.ExpenseModel>();

        foreach (var category in categories)
        {
            var expenses = await _expenseApiClient.GetByCategoryIdAsync(category.Id).ConfigureAwait(false);

            allExpenses.AddRange(expenses);
        }

        return allExpenses.Where(x => x.ExpenseDate.Month == month && x.ExpenseDate.Year == year)
            .GroupBy(x => x.ExpenseDate.Date)
            .OrderBy(x => x.Key)
            .Select(x => new ExpenseTrendModel
            {
                Date = x.Key,
                TotalAmount = x.Sum(e => e.Amount)
            })
            .ToList();
    }
}
