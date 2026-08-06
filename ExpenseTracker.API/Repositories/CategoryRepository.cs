using ExpenseTracker.API.Data;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ExpenseTrackerDbContext _context;

    public CategoryRepository(ExpenseTrackerDbContext context)
    {
        Guard.AgainstNull(context, nameof(context));

        _context = context;
    }

    public async Task<IEnumerable<BudgetCategory>> GetAllAsync()
    {
        return await _context.Categories.AsNoTracking().OrderBy(x => x.Name).ToListAsync().ConfigureAwait(false);
    }

    public async Task<BudgetCategory?> GetByIdAsync(Guid id)
    {
        Guard.AgainstNull(id, nameof(id));

        return await _context.Categories.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task AddAsync(BudgetCategory entity)
    {
        Guard.AgainstNull(entity, nameof(entity));

        await _context.Categories.AddAsync(entity).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateAsync(BudgetCategory entity)
    {
        Guard.AgainstNull(entity, nameof(entity));

        _context.Categories.Update(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAsync(Guid id)
    {
        Guard.AgainstNull(id, nameof(id));

        var category = await _context.Categories.FindAsync(id).ConfigureAwait(false);

        if (category is null)
        {
            return;
        }

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        Guard.AgainstNull(id, nameof(id));

        return await _context.Categories.AnyAsync(x => x.Id == id).ConfigureAwait(false);
    }

    public async Task<IEnumerable<BudgetCategory>> GetCategoriesByBudgetAsync(Guid budgetId)
    {
        Guard.AgainstNull(budgetId, nameof(budgetId));

        return await _context.Categories.Where(x => x.BudgetId == budgetId).OrderBy(x => x.Name)
            .AsNoTracking().ToListAsync().ConfigureAwait(false);
    }

    public async Task<BudgetCategory?> GetCategoryWithExpensesAsync(Guid categoryId)
    {
        Guard.AgainstNull(categoryId, nameof(categoryId));

        return await _context.Categories.Include(x => x.Expenses).AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == categoryId).ConfigureAwait(false);
    }

    public async Task<bool> CategoryExistsAsync(Guid budgetId, string categoryName)
    {
        Guard.AgainstNull(budgetId, nameof(budgetId));
        Guard.AgainstNull(categoryName, nameof(categoryName));

        return await _context.Categories.AnyAsync(x => x.BudgetId == budgetId && x.Name == categoryName).ConfigureAwait(false);
    }
}
