using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.API.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        public Task AddAsync(Expense entity)
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

        public Task<IEnumerable<Expense>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Expense?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetTotalExpenseAsync(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Expense entity)
        {
            throw new NotImplementedException();
        }
    }
}
