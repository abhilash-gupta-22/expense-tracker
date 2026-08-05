using ExpenseTracker.API.DTOs.Budget;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/budgets")]
[Produces("application/json")]
public class BudgetController : ControllerBase
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IBudgetDomainService _budgetService;

    public BudgetController(IBudgetRepository budgetRepository, IBudgetDomainService budgetService)
    {
        Guard.AgainstNull(budgetRepository, nameof(budgetRepository));
        Guard.AgainstNull(budgetService, nameof(budgetService));

        _budgetRepository = budgetRepository;
        _budgetService = budgetService;
    }

    /// <summary>
    /// Returns all budgets.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BudgetResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BudgetResponse>>> GetAllAsync()
    {
        var budgets = await _budgetRepository.GetAllAsync().ConfigureAwait(false);

        // TODO: Map Entity -> DTO
        var response = budgets.Select(b => new BudgetResponse
        {
            Id = b.Id,
            Month = b.Month,
            Year = b.Year,
            TotalBudget = b.TotalBudget
        });

        return Ok(response);
    }

    /// <summary>
    /// Returns a budget by Id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BudgetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BudgetResponse>> GetByIdAsync(Guid id)
    {
        var budget = await _budgetRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (budget is null)
        {
            return NotFound();
        }

        var response = new BudgetResponse
        {
            Id = budget.Id,
            Month = budget.Month,
            Year = budget.Year,
            TotalBudget = budget.TotalBudget
        };

        return Ok(response);
    }

    /// <summary>
    /// Returns budget for a given month and year.
    /// </summary>
    [HttpGet("{year:int}/{month:int}")]
    [ProducesResponseType(typeof(BudgetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BudgetResponse>> GetByMonthAsync(int year, int month)
    {
        var budget = await _budgetRepository.GetBudgetAsync(month, year).ConfigureAwait(false);

        if (budget is null)
        {
            return NotFound();
        }

        var response = new BudgetResponse
        {
            Id = budget.Id,
            Month = budget.Month,
            Year = budget.Year,
            TotalBudget = budget.TotalBudget
        };

        return Ok(response);
    }

    /// <summary>
    /// Creates a new monthly budget.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(BudgetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BudgetResponse>> CreateAsync([FromBody] CreateBudgetRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var budgetExists = await _budgetRepository.BudgetExistsAsync(request.Month, request.Year).ConfigureAwait(false);

        if (budgetExists)
        {
            return Conflict($"Budget already exists for {request.Month}/{request.Year}.");
        }

        var budget = _budgetService.CreateBudget(request.TotalBudget, request.Month, request.Year);

        await _budgetRepository.AddAsync(budget).ConfigureAwait(false);

        var response = new BudgetResponse
        {
            Id = budget.Id,
            Month = budget.Month,
            Year = budget.Year,
            TotalBudget = budget.TotalBudget
        };

        return CreatedAtAction(nameof(GetByIdAsync), new { id = budget.Id }, response);
    }

    /// <summary>
    /// Updates an existing budget.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateBudgetRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var budget = await _budgetRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (budget is null)
        {
            return NotFound();
        }

        // TODO:
        // Prefer adding domain methods such as:
        // budget.UpdateTotalBudget(request.TotalBudget);

        budget.TotalBudget = request.TotalBudget;

        await _budgetRepository.UpdateAsync(budget).ConfigureAwait(false);

        return NoContent();
    }

    /// <summary>
    /// Deletes a budget.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var exists = await _budgetRepository.ExistsAsync(id).ConfigureAwait(false);

        if (!exists)
        {
            return NotFound();
        }

        await _budgetRepository.DeleteAsync(id).ConfigureAwait(false);

        return NoContent();
    }
}
