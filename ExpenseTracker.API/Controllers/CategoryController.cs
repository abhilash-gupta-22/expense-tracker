using ExpenseTracker.API.DTOs.Category;
using ExpenseTracker.API.Mapping;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/categories")]
[Produces("application/json")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IBudgetDomainService _budgetService;

    public CategoryController(ICategoryRepository categoryRepository, IBudgetRepository budgetRepository, IBudgetDomainService budgetService)
    {
        Guard.AgainstNull(categoryRepository, nameof(categoryRepository));
        Guard.AgainstNull(budgetRepository, nameof(budgetRepository));
        Guard.AgainstNull(budgetService, nameof(budgetService));

        _categoryRepository = categoryRepository;
        _budgetRepository = budgetRepository;
        _budgetService = budgetService;
    }

    [HttpGet("budget/{budgetId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetByBudgetAsync(Guid budgetId)
    {
        Guard.AgainstNullOrEmptyGuid(budgetId, nameof(budgetId));

        var categories = await _categoryRepository.GetCategoriesByBudgetAsync(budgetId).ConfigureAwait(false);

        return Ok(categories.ToResponse());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryResponse>> GetByIdAsync(Guid id)
    {
        Guard.AgainstNullOrEmptyGuid(id, nameof(id));

        var category = await _categoryRepository.GetCategoryWithExpensesAsync(id).ConfigureAwait(false);

        if (category is null)
        {
            return NotFound();
        }

        return Ok(category.ToResponse());
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoryResponse>> CreateAsync(CreateCategoryRequest request)
    {
        Guard.AgainstNull(request, nameof(request));

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var budget = await _budgetRepository.GetByIdAsync(request.BudgetId).ConfigureAwait(false);

        if (budget is null)
        {
            return BadRequest("Budget not found.");
        }

        var exists = await _categoryRepository.CategoryExistsAsync(request.BudgetId, request.Name).ConfigureAwait(false);

        if (exists)
        {
            return Conflict($"Category '{request.Name}' already exists.");
        }

        var category = new BudgetCategory
        {
            Name = request.Name,
            AllocatedBudget = request.AllocatedBudget
        };

        _budgetService.AddCategory(budget, category);

        var result = _budgetService.AddCategory(budget, category);

        if (result.IsFailure)
        {
            return BadRequest(result.ErrorMessage);
        }

        await _categoryRepository.AddAsync(category).ConfigureAwait(false);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = category.Id }, category.ToResponse());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateCategoryRequest request)
    {
        Guard.AgainstNullOrEmptyGuid(id, nameof(id));
        Guard.AgainstNull(request, nameof(request));

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var category = await _categoryRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (category is null)
        {
            return NotFound();
        }

        category.Name = request.Name;
        category.AllocatedBudget = request.AllocatedBudget;

        await _categoryRepository.UpdateAsync(category).ConfigureAwait(false);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        Guard.AgainstNullOrEmptyGuid(id, nameof(id));

        var exists = await _categoryRepository.ExistsAsync(id).ConfigureAwait(false);

        if (!exists)
        {
            return NotFound();
        }

        await _categoryRepository.DeleteAsync(id).ConfigureAwait(false);

        return NoContent();
    }
}