using ExpenseTracker.API.DTOs.Budget;
using ExpenseTracker.API.Mapping;
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
    private readonly ILogger<BudgetController> _logger;

    public BudgetController(IBudgetRepository budgetRepository, IBudgetDomainService budgetService, ILogger<BudgetController> logger)
    {
        Guard.AgainstNull(budgetRepository, nameof(budgetRepository));
        Guard.AgainstNull(budgetService, nameof(budgetService));
        Guard.AgainstNull(logger, nameof(logger));

        _budgetRepository = budgetRepository;
        _budgetService = budgetService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BudgetResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BudgetResponse>>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all budgets.");

        var budgets = await _budgetRepository.GetAllAsync().ConfigureAwait(false);
        var response = budgets.Select(b => b.ToResponse());

        _logger.LogInformation("Retrieved {BudgetCount} budgets.", response.Count());

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
        _logger.LogInformation($"Retrieving budget with Id {id}.");

        Guard.AgainstNullOrEmptyGuid(id, nameof(id));

        var budget = await _budgetRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (budget is null)
        {
            _logger.LogWarning($"Budget with Id {id} was not found.");
            return NotFound();
        }

        return Ok(budget.ToResponse());
    }

    /// <summary>
    /// Returns budget for a given month and year.
    /// </summary>
    [HttpGet("{year:int}/{month:int}")]
    [ProducesResponseType(typeof(BudgetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BudgetResponse>> GetByMonthAsync(int year, int month)
    {
        _logger.LogInformation($"Retrieving budget for {month}/{year}.");

        Guard.AgainstInvalidYear(year, nameof(year));
        Guard.AgainstInvalidMonth(month, nameof(month));

        var budget = await _budgetRepository.GetBudgetAsync(month, year).ConfigureAwait(false);

        if (budget is null)
        {
            _logger.LogWarning($"No budget found for {month}/{year}.");

            return NotFound();
        }

        return Ok(budget.ToResponse());
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
        if (request is null)
        {
            _logger.LogWarning("Create budget request body was null.");
            return BadRequest("Request body is required.");
        }

        Guard.AgainstNull(request, nameof(request));

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid request received while creating budget.");
            return ValidationProblem(ModelState);
        }

        _logger.LogInformation($"Creating budget for {request.Month}/{request.Year} with total amount {request.TotalBudget}.");

        var budgetExists = await _budgetRepository.BudgetExistsAsync(request.Month, request.Year).ConfigureAwait(false);

        if (budgetExists)
        {
            _logger.LogWarning($"Budget already exists for {request.Month}/{request.Year}.");
            return Conflict($"Budget already exists for {request.Month}/{request.Year}.");
        }

        var result = _budgetService.CreateBudget(request.TotalBudget, request.Month, request.Year);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Budget creation failed for {request.Month}/{request.Year}: {result.ErrorMessage}.");
            return BadRequest(result.ErrorMessage);
        }

        var budget = result.Value;

        await _budgetRepository.AddAsync(budget).ConfigureAwait(false);

        _logger.LogInformation($"Budget created successfully with Id {budget.Id}.");

        var response = new BudgetResponse
        {
            Id = budget.Id,
            Month = budget.Month,
            Year = budget.Year,
            TotalBudget = budget.TotalBudget
        };

        // Action names have the "Async" suffix trimmed by the framework when routing.
        // Use the action name without the Async suffix to ensure link generation succeeds.
        return CreatedAtAction(nameof(GetByIdAsync).Replace("Async", string.Empty), new { id = budget.Id }, response);
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
        _logger.LogInformation("Updating budget with Id {BudgetId}.", id);

        Guard.AgainstNullOrEmptyGuid(id, nameof(id));
        Guard.AgainstNull(request, nameof(request));

        if (!ModelState.IsValid)
        {
            _logger.LogWarning($"Invalid request received for budget {id}.");
            return ValidationProblem(ModelState);
        }

        var budget = await _budgetRepository.GetByIdAsync(id).ConfigureAwait(false);

        if (budget is null)
        {
            _logger.LogWarning($"Budget with Id {id} was not found for update.");
            return NotFound();
        }

        // TODO:
        // Prefer adding domain methods such as:
        // budget.UpdateTotalBudget(request.TotalBudget);

        budget.TotalBudget = request.TotalBudget;

        await _budgetRepository.UpdateAsync(budget).ConfigureAwait(false);

        _logger.LogInformation($"Budget with Id {id} updated successfully.");

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
        _logger.LogInformation($"Deleting budget with Id {id}.");

        Guard.AgainstNullOrEmptyGuid(id, nameof(id));

        var exists = await _budgetRepository.ExistsAsync(id).ConfigureAwait(false);

        if (!exists)
        {
            _logger.LogWarning($"Budget with Id {id} was not found for deletion.");
            return NotFound();
        }

        await _budgetRepository.DeleteAsync(id).ConfigureAwait(false);

        _logger.LogInformation($"Budget with Id {id} deleted successfully.");

        return NoContent();
    }
}
