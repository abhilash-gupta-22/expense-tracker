using ExpenseTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/expenses")]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IExpenseDomainService _expenseService;

    public ExpenseController(IExpenseRepository expenseRepository, ICategoryRepository categoryRepository, IExpenseDomainService expenseService)
    {
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
        _expenseService = expenseService;
    }
}