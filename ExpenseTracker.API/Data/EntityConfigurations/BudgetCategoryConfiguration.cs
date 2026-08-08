using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.API.Data.EntityConfigurations;

public class BudgetCategoryConfiguration : IEntityTypeConfiguration<BudgetCategory>
{
    public void Configure(EntityTypeBuilder<BudgetCategory> builder)
    {
        builder.ToTable("BudgetCategories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.BudgetId).IsRequired();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(DomainConstants.MaxCategoryNameLength);

        builder.Property(x => x.AllocatedBudget).IsRequired().HasPrecision(18, 2);

        builder.HasIndex(x => new
        {
            x.BudgetId,
            x.Name
        }).IsUnique();
    }
}