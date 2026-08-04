using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public Task AddAsync(BudgetCategory entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CategoryExistsAsync(Guid budgetId, string categoryName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BudgetCategory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BudgetCategory?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BudgetCategory>> GetCategoriesByBudgetAsync(Guid budgetId)
        {
            throw new NotImplementedException();
        }

        public Task<BudgetCategory?> GetCategoryWithExpensesAsync(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(BudgetCategory entity)
        {
            throw new NotImplementedException();
        }
    }
}
