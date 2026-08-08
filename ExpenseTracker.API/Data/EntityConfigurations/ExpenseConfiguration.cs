using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.API.Data.EntityConfigurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("Expenses");

        // Configure the primary key for Expense entity
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();

        // Configure the properties for the Expense entity
        builder.Property(x => x.BudgetCategoryId).IsRequired();
        builder.Property(x => x.BudgetCategoryName).HasMaxLength(50);
        builder.Property(x => x.Amount).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.ExpenseDate).IsRequired();
        builder.Property(x => x.Remarks).IsRequired().HasMaxLength(250);

        // Configure the relationship between Expense and BudgetCategory entities
        builder.HasOne<BudgetCategory>().WithMany(x => x.Expenses)
            .HasForeignKey(x => x.BudgetCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the index for BudgetCategoryId and ExpenseDate properties
        builder.HasIndex(x => new
        {
            x.BudgetCategoryId,
            x.ExpenseDate
        });
    }
}
