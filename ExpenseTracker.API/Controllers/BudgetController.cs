using ExpenseTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/budgets")]
public class BudgetController : ControllerBase
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IBudgetDomainService _budgetService;

    public BudgetController(IBudgetRepository budgetRepository, IBudgetDomainService budgetService)
    {
    }
}
