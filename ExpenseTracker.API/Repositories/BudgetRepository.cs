using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.API.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.API.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly ExpenseTrackerDbContext _context;

    public BudgetRepository(ExpenseTrackerDbContext context)
    {
        Guard.AgainstNull(context, nameof(context));

        _context = context;
    }

    public async Task<IEnumerable<Budget>> GetAllAsync()
    {
        return await _context.Budgets.AsNoTracking()
            .OrderByDescending(x => x.Year)
            .ThenByDescending(x => x.Month)
            .ToListAsync().ConfigureAwait(false);
    }

    public async Task<Budget?> GetByIdAsync(Guid id)
    {
        Guard.AgainstNull(id, nameof(id));

        return await _context.Budgets.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task AddAsync(Budget entity)
    {
        Guard.AgainstNull(entity, nameof(entity));

        await _context.Budgets.AddAsync(entity).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateAsync(Budget entity)
    {
        Guard.AgainstNull(entity, nameof(entity));

        _context.Budgets.Update(entity);

        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAsync(Guid id)
    {
        Guard.AgainstNull(id, nameof(id));

        var budget = await _context.Budgets.FindAsync(id).ConfigureAwait(false);

        if (budget is null)
        {
            return;
        }

        _context.Budgets.Remove(budget);

        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        Guard.AgainstNull(id, nameof(id));

        return await _context.Budgets.AnyAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task<Budget?> GetBudgetAsync(int month, int year)
    {
        Guard.AgainstInvalidMonth(month, nameof(month));
        Guard.AgainstInvalidYear(year, nameof(year));

        return await _context.Budgets.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Month == month && x.Year == year).ConfigureAwait(false);
    }

    public async Task<bool> BudgetExistsAsync(int month, int year)
    {
        Guard.AgainstInvalidMonth(month, nameof(month));
        Guard.AgainstInvalidYear(year, nameof(year));

        return await _context.Budgets.AnyAsync(x => x.Month == month && x.Year == year).ConfigureAwait(false);
    }

    public async Task<Budget?> GetBudgetWithCategoriesAsync(Guid budgetId)
    {
        Guard.AgainstNull(budgetId, nameof(budgetId));

        return await _context.Budgets.Include(x => x.BudgetCategories)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == budgetId).ConfigureAwait(false);
    }

    public async Task<Budget?> GetBudgetCompleteAsync(Guid budgetId)
    {
        Guard.AgainstNull(budgetId, nameof(budgetId));

        return await _context.Budgets.Include(x => x.BudgetCategories)
            .ThenInclude(x => x.Expenses)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == budgetId).ConfigureAwait(false);
    }
}
