using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs.Category;

public class UpdateCategoryRequest
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Range(1, double.MaxValue)]
    public decimal AllocatedBudget { get; set; }
}
