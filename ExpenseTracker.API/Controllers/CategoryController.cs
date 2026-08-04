using ExpenseTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IBudgetDomainService _budgetService;

    public CategoryController(ICategoryRepository categoryRepository, IBudgetRepository budgetRepository, IBudgetDomainService budgetService)
    {
        _categoryRepository = categoryRepository;
        _budgetRepository = budgetRepository;
        _budgetService = budgetService;
    }
}
