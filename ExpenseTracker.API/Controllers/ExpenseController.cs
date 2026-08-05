using ExpenseTracker.API.DTOs.Expense;
using ExpenseTracker.API.Mapping;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/expenses")]
[Produces("application/json")]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IExpenseDomainService _expenseService;

    public ExpenseController(IExpenseRepository expenseRepository, ICategoryRepository categoryRepository, IExpenseDomainService expenseService)
    {
        Guard.AgainstNull(expenseRepository, nameof(expenseRepository));
        Guard.AgainstNull(categoryRepository, nameof(categoryRepository));
        Guard.AgainstNull(expenseService, nameof(expenseService));

        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
        _expenseService = expenseService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ExpenseResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ExpenseResponse>>> GetAllAsync()
    {
        var expenses = await _expenseRepository.GetAllAsync().ConfigureAwait(false);

        return Ok(expenses.ToResponse());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ExpenseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExpenseResponse>> GetByIdAsync(Guid id)
    {
        Guard.AgainstNull(id, nameof(id));

        var expense = await _expenseRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (expense is null)
        {
            return NotFound();
        }

        return Ok(expense.ToResponse());
    }

    [HttpGet("category/{categoryId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<ExpenseResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ExpenseResponse>>> GetByCategoryAsync(Guid categoryId)
    {
        Guard.AgainstNull(categoryId, nameof(categoryId));

        var expenses = await _expenseRepository.GetExpensesByCategoryAsync(categoryId).ConfigureAwait(false);

        return Ok(expenses.ToResponse());
    }

    [HttpGet("date-range")]
    [ProducesResponseType(typeof(IEnumerable<ExpenseResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ExpenseResponse>>> GetByDateRangeAsync([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        Guard.AgainstNull(from, nameof(from));
        Guard.AgainstNull(to, nameof(to));

        var expenses = await _expenseRepository.GetExpensesByDateRangeAsync(from, to).ConfigureAwait(false);

        return Ok(expenses.ToResponse());
    }

    [HttpPost]
    [ProducesResponseType(typeof(ExpenseResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ExpenseResponse>> CreateAsync(CreateExpenseRequest request)
    {
        Guard.AgainstNull(request, nameof(request));

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var category = await _categoryRepository.GetCategoryWithExpensesAsync(request.CategoryId).ConfigureAwait(false);

        if (category is null)
        {
            return BadRequest("Category not found.");
        }

        if (!_expenseService.CanAddExpense(category, request.Amount))
        {
            return BadRequest("Category budget exceeded.");
        }

        var expense = new Expense
        {
            BudgetCategoryId = request.CategoryId,
            Amount = request.Amount,
            ExpenseDate = request.ExpenseDate,
            Remarks = request.Remarks ?? string.Empty
        };

        _expenseService.AddExpense(category, expense);

        await _expenseRepository.AddAsync(expense).ConfigureAwait(false);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = expense.Id }, expense.ToResponse());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateExpenseRequest request)
    {
        Guard.AgainstNull(id, nameof(id));
        Guard.AgainstNull(request, nameof(request));

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var expense = await _expenseRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (expense is null)
        {
            return NotFound();
        }

        expense.Amount = request.Amount;
        expense.ExpenseDate = request.ExpenseDate;
        expense.Remarks = request.Remarks ?? string.Empty;

        await _expenseRepository.UpdateAsync(expense).ConfigureAwait(false);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        Guard.AgainstNull(id, nameof(id));

        var exists = await _expenseRepository.ExistsAsync(id).ConfigureAwait(false);

        if (!exists)
        {
            return NotFound();
        }

        await _expenseRepository.DeleteAsync(id).ConfigureAwait(false);

        return NoContent();
    }
}
