using ExpenseTracker.Web.Models.Budget;
using ExpenseTracker.Web.Services.Interfaces;

namespace ExpenseTracker.Web.Services
{
    public class BudgetService : IBudgetService
    {
        public Task<BudgetModel> CreateAsync(CreateBudgetModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BudgetModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BudgetModel?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BudgetModel?> GetByMonthAsync(int year, int month)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, UpdateBudgetModel model)
        {
            throw new NotImplementedException();
        }
    }
}
