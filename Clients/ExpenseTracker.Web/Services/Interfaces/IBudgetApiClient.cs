using ExpenseTracker.Web.Models.Budget;

namespace ExpenseTracker.Web.Services.Interfaces;

public interface IBudgetApiClient
{
    Task<IEnumerable<BudgetModel>> GetAllAsync();

    Task<BudgetModel?> GetByIdAsync(Guid id);

    Task<BudgetModel?> GetByMonthAsync(int year, int month);

    Task<BudgetModel> CreateAsync(CreateBudgetModel model);

    Task UpdateAsync(Guid id, UpdateBudgetModel model);

    Task DeleteAsync(Guid id);
}
