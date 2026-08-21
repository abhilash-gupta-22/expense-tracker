using ExpenseTracker.API.Data;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ExpenseTrackerDbContext _context;

    public ExpenseRepository(ExpenseTrackerDbContext context)
    {
        Guard.AgainstNull(context, nameof(context));

        _context = context;
    }

    public async Task<IEnumerable<Expense>> GetAllAsync()
    {
        return await _context.Expenses.AsNoTracking().OrderByDescending(x => x.ExpenseDate)
            .ToListAsync().ConfigureAwait(false);
    }

    public async Task<Expense?> GetByIdAsync(Guid id)
    {
        return await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Expense entity)
    {
        Guard.AgainstNull(entity, nameof(entity));

        await _context.Expenses.AddAsync(entity).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateAsync(Expense entity)
    {
        Guard.AgainstNull(entity, nameof(entity));

        _context.Expenses.Update(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAsync(Guid id)
    {
        Guard.AgainstNull(id, nameof(id));

        var expense = await _context.Expenses.FindAsync(id).ConfigureAwait(false);

        if (expense is null)
        {
            return;
        }

        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        Guard.AgainstNull(id, nameof(id));

        return await _context.Expenses.AnyAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(Guid categoryId)
    {
        Guard.AgainstNull(categoryId, nameof(categoryId));

        return await _context.Expenses.Where(x => x.BudgetCategoryId == categoryId)
            .OrderByDescending(x => x.ExpenseDate)
            .AsNoTracking()
            .ToListAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<Expense>> GetExpensesByDateRangeAsync(DateTime from, DateTime to)
    {
        Guard.AgainstNull(from, nameof(from));
        Guard.AgainstNull(to, nameof(to));

        return await _context.Expenses.Where(x => x.ExpenseDate >= from && x.ExpenseDate <= to)
            .OrderByDescending(x => x.ExpenseDate)
            .AsNoTracking()
            .ToListAsync().ConfigureAwait(false);
    }

    public async Task<decimal> GetTotalExpenseAsync(Guid categoryId)
    {
        Guard.AgainstNull(categoryId, nameof(categoryId));

        return await _context.Expenses.Where(x => x.BudgetCategoryId == categoryId)
            .SumAsync(x => x.Amount).ConfigureAwait(false);
    }
}
