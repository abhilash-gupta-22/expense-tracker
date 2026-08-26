using System.Net;
using System.Net.Http.Json;
using ExpenseTracker.Web.Models.Category;
using ExpenseTracker.Web.Services.Interfaces;

namespace ExpenseTracker.Web.Services;

public class CategoryApiClient : ICategoryApiClient
{
    private const string BaseUrl = "api/categories";

    private readonly HttpClient _httpClient;

    public CategoryApiClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets all categories.
    /// </summary>
    public async Task<IEnumerable<CategoryModel>> GetAllAsync()
    {
        var categories = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryModel>>(BaseUrl).ConfigureAwait(false);

        return categories ?? Enumerable.Empty<CategoryModel>();
    }

    /// <summary>
    /// Gets a category by its unique identifier.
    /// </summary>
    public async Task<CategoryModel?> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{id}").ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CategoryModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Gets all categories associated with a budget.
    /// </summary>
    public async Task<IEnumerable<CategoryModel>> GetByBudgetIdAsync(Guid budgetId)
    {
        var categories = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryModel>>($"{BaseUrl}/budget/{budgetId}").ConfigureAwait(false);

        return categories ?? Enumerable.Empty<CategoryModel>();
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    public async Task<CategoryModel> CreateAsync(CreateCategoryModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var response = await _httpClient.PostAsJsonAsync(BaseUrl, model).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var category = await response.Content.ReadFromJsonAsync<CategoryModel>().ConfigureAwait(false);

        return category ?? throw new InvalidOperationException("The API returned an empty response.");
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    public async Task UpdateAsync(Guid id, UpdateCategoryModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", model).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Deletes a category.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}").ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
    }
}
