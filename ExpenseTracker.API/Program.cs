using ExpenseTracker.API.Data;
using ExpenseTracker.API.Repositories;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Domain.Services;

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();