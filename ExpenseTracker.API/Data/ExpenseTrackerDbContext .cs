using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Data;

public class ExpenseTrackerDbContext : DbContext
{
    public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the budgets.
    /// </summary>
    public DbSet<Budget> Budgets => Set<Budget>();

    /// <summary>
    /// Gets or sets the budget categories.
    /// </summary>
    public DbSet<BudgetCategory> BudgetCategories => Set<BudgetCategory>();

    /// <summary>
    /// Gets or sets the expenses.
    /// </summary>
    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseTrackerDbContext).Assembly);
    }
}
