using System.Net;
using System.Net.Http.Json;
using ExpenseTracker.Web.Models.Budget;
using ExpenseTracker.Web.Services.Interfaces;

namespace ExpenseTracker.Web.Services;

public class BudgetApiClient : IBudgetApiClient
{
    private const string BaseUrl = "api/budgets";

    private readonly HttpClient _httpClient;

    public BudgetApiClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets all budgets.
    /// </summary>
    public async Task<IEnumerable<BudgetModel>> GetAllAsync()
    {
        var budgets = await _httpClient.GetFromJsonAsync<IEnumerable<BudgetModel>>(BaseUrl).ConfigureAwait(false);

        return budgets ?? Enumerable.Empty<BudgetModel>();
    }

    /// <summary>
    /// Gets a budget by its unique identifier.
    /// </summary>
    public async Task<BudgetModel?> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{id}").ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<BudgetModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a budget for the specified month and year.
    /// </summary>
    public async Task<BudgetModel?> GetByMonthAsync(int year, int month)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{year}/{month}").ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<BudgetModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a new budget.
    /// </summary>
    public async Task<BudgetModel> CreateAsync(CreateBudgetModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var response = await _httpClient.PostAsJsonAsync(BaseUrl, model).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var budget = await response.Content.ReadFromJsonAsync<BudgetModel>().ConfigureAwait(false);

        return budget ?? throw new InvalidOperationException("The API returned an empty response.");
    }

    /// <summary>
    /// Updates an existing budget.
    /// </summary>
    public async Task UpdateAsync(Guid id, UpdateBudgetModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", model).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Deletes a budget.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}").ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
    }
}
