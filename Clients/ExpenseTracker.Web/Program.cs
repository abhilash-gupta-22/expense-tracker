using ExpenseTracker.Web.Components;
using ExpenseTracker.Web.Services;
using ExpenseTracker.Web.Services.Interfaces;
using ExpenseTracker.Web.State;

var builder = WebApplication.CreateBuilder(args);

// Add Razor components.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Register application state.
builder.Services.AddScoped<BudgetState>();

// Get API base URL from configuration.
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? throw new InvalidOperationException("API base URL is not configured.");

// Register API clients.
builder.Services.AddHttpClient<IBudgetApiClient, BudgetApiClient>(
    client =>
    {
        client.BaseAddress = new Uri(apiBaseUrl);
    });

builder.Services.AddHttpClient<ICategoryApiClient, CategoryApiClient>(
    client =>
    {
        client.BaseAddress = new Uri(apiBaseUrl);
    });

builder.Services.AddHttpClient<IExpenseApiClient, ExpenseApiClient>(
    client =>
    {
        client.BaseAddress = new Uri(apiBaseUrl);
    });

// Register Dashboard API client.
// DashboardApiClient uses the other API clients internally.
builder.Services.AddScoped<IDashboardApiClient, DashboardApiClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
