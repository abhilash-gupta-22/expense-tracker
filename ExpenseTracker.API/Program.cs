using ExpenseTracker.API.Data;
using ExpenseTracker.API.Repositories;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("ExpenseTrackerDb"));
});

// Repositories
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();

// Register domain services
builder.Services.AddScoped<IExpenseDomainService, ExpenseDomainService>();
builder.Services.AddScoped<IBudgetDomainService, BudgetDomainService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ensure database is created and migrations are applied on startup (development/local scenarios).
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ExpenseTrackerDbContext>();
    // Applies any pending migrations for the context to the database. Will create the database if it does not already exist.
    db.Database.Migrate();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Database migrations applied successfully.");
    // Verify that required tables exist; if not, create schema from the model
    DbInitializer.EnsureTablesCreated(db, logger);
}
catch (Exception ex)
{
    // If migration fails, log the error and continue so the app can fail-fast during startup in production if desired.
    var logger = app.Services.GetService<ILogger<Program>>();
    logger?.LogError(ex, "An error occurred while migrating or initializing the database.");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();