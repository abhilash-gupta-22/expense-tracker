using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Web.Models.Category;

public class UpdateCategoryModel
{
    /// <summary>
    /// Gets or sets the category name.
    /// </summary>
    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the allocated budget amount.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Allocated budget must be greater than zero.")]
    public decimal AllocatedBudget { get; set; }
}
