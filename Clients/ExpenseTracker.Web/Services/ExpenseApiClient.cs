using System.Net;
using System.Net.Http.Json;
using ExpenseTracker.Web.Models.Expense;
using ExpenseTracker.Web.Services.Interfaces;

namespace ExpenseTracker.Web.Services;

public class ExpenseApiClient : IExpenseApiClient
{
    private const string BaseUrl = "api/expenses";

    private readonly HttpClient _httpClient;

    public ExpenseApiClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets all expenses.
    /// </summary>
    public async Task<IEnumerable<ExpenseModel>> GetAllAsync()
    {
        var expenses = await _httpClient.GetFromJsonAsync<IEnumerable<ExpenseModel>>(BaseUrl).ConfigureAwait(false);

        return expenses ?? Enumerable.Empty<ExpenseModel>();
    }

    /// <summary>
    /// Gets an expense by its unique identifier.
    /// </summary>
    public async Task<ExpenseModel?> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{id}").ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ExpenseModel>().ConfigureAwait(false);
    }


    /// <summary>
    /// Gets all expenses associated with a specific budget.
    /// </summary>
    /// <param name="budgetId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ExpenseModel>> GetByBudgetIdAsync(Guid budgetId)
    {
        var response = await _httpClient.GetAsync($"api/expenses/budget/{budgetId}");

        if (!response.IsSuccessStatusCode) 
            return Enumerable.Empty<ExpenseModel>();

        return await response.Content.ReadFromJsonAsync<IEnumerable<ExpenseModel>>().ConfigureAwait(false)
               ?? Enumerable.Empty<ExpenseModel>();
    }

    /// <summary>
    /// Gets all expenses for a specific budget category.
    /// </summary>
    public async Task<IEnumerable<ExpenseModel>> GetByCategoryIdAsync(Guid budgetCategoryId)
    {
        var expenses = await _httpClient.GetFromJsonAsync<IEnumerable<ExpenseModel>>($"{BaseUrl}/category/{budgetCategoryId}").ConfigureAwait(false);

        return expenses ?? Enumerable.Empty<ExpenseModel>();
    }

    /// <summary>
    /// Creates a new expense.
    /// </summary>
    public async Task<ExpenseModel> CreateAsync(CreateExpenseModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var response = await _httpClient.PostAsJsonAsync(BaseUrl, model).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var expense = await response.Content.ReadFromJsonAsync<ExpenseModel>().ConfigureAwait(false);

        return expense ?? throw new InvalidOperationException("The API returned an empty response.");
    }

    /// <summary>
    /// Updates an existing expense.
    /// </summary>
    public async Task UpdateAsync(Guid id, UpdateExpenseModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", model).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Deletes an expense.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}").ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
    }
}
