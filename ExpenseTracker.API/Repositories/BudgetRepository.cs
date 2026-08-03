using ExpenseTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Repositories
{
    public interface IBudgetRepository
    {
        Task AddAsync(Budget budget);
        Task UpdateAsync(Budget budget);
        Task DeleteAsync(Guid id);
        Task<Budget?> GetByIdAsync(Guid id);
        Task<IEnumerable<Budget>> GetAllAsync();
        Task<bool> ExistsAsync(Guid id);
        Task<Budget?> GetBudgetAsync(int month, int year);
        Task<bool> BudgetExistsAsync(int month, int year);
        Task<Budget?> GetBudgetWithCategoriesAsync(Guid budgetId);
        Task<Budget?> GetBudgetCompleteAsync(Guid budgetId);
    }
}
