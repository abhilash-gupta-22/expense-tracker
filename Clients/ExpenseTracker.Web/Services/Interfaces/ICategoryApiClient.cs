using ExpenseTracker.Web.Models.Category;

namespace ExpenseTracker.Web.Services.Interfaces;

public interface ICategoryApiClient
{
    /// <summary>
    /// Gets all categories.
    /// </summary>
    Task<IEnumerable<CategoryModel>> GetAllAsync();

    /// <summary>
    /// Gets a category by its unique identifier.
    /// </summary>
    Task<CategoryModel?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all categories associated with a specific budget.
    /// </summary>
    Task<IEnumerable<CategoryModel>> GetByBudgetIdAsync(Guid budgetId);

    /// <summary>
    /// Creates a new category.
    /// </summary>
    Task<CategoryModel> CreateAsync(CreateCategoryModel model);

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    Task UpdateAsync(Guid id, UpdateCategoryModel model);

    /// <summary>
    /// Deletes a category.
    /// </summary>
    Task DeleteAsync(Guid id);
}
