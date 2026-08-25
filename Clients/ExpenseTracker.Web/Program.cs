using ExpenseTracker.Web.Components;
using ExpenseTracker.Web.Services;
using ExpenseTracker.Web.Services.Interfaces;
using ExpenseTracker.Web.State;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()    .AddInteractiveServerComponents();

var app = builder.Build();

builder.Services.AddScoped<BudgetState>();

// Add API clients
builder.Services.AddScoped<IBudgetApiClient, BudgetApiClient>();
builder.Services.AddScoped<ICategoryApiClient, CategoryApiClient>();
builder.Services.AddScoped<IExpenseApiClient, ExpenseApiClient>();
builder.Services.AddScoped<IDashboardApiClient, DashboardApiClient>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
