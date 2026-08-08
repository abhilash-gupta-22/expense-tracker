using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.API.Data.EntityConfigurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable("Budgets");

        // Configure the primary key for the Budget entity
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();

        // Configure the properties for the Budget entity
        builder.Property(x => x.Month).IsRequired();
        builder.Property(x => x.Year).IsRequired();
        builder.Property(x => x.TotalBudget).IsRequired().HasPrecision(18, 2);

        // Configure the unique index for the combination of Month and Year
        builder.HasIndex(x => new
        {
            x.Month,
            x.Year
        }).IsUnique();

        // Configure the relationship between Budget and BudgetCategory entities
        builder.HasMany(x => x.BudgetCategories)
            .WithOne()
            .HasForeignKey(x => x.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
