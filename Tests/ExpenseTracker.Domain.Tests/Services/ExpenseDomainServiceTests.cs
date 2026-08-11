using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Domain.Services;

namespace ExpenseTracker.Domain.Tests.Services;

[TestClass]
public class ExpenseDomainServiceTests
{
    private readonly IExpenseDomainService _expenseDomainService;
    private readonly BudgetCategory _dummyCategory;
    private readonly Expense _dummyExpense;

    public ExpenseDomainServiceTests()
    {
        _expenseDomainService = new ExpenseDomainService();

        _dummyCategory = new BudgetCategory
        {
            BudgetId = Guid.NewGuid(),
            Name = "Food",
            AllocatedBudget = 500,
            Expenses = new List<Expense>()
        };

        _dummyExpense = new Expense
        {
            BudgetCategoryId = _dummyCategory.BudgetId,
            Amount = 50,
            Remarks = "Groceries",
            ExpenseDate = DateTime.UtcNow
        };
    }


    [TestMethod]
    public void AddExpense_ShouldAddExpenseToCategory()
    {
        // Act
        _expenseDomainService.AddExpense(_dummyCategory, _dummyExpense);

        // Assert
        Assert.HasCount(1, _dummyCategory.Expenses);
        Assert.AreEqual(_dummyExpense, _dummyCategory.Expenses.First());
    }

    [TestMethod]
    public void AddExpense_WithNullCategory_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => _expenseDomainService.AddExpense(null, _dummyExpense));
    }

    [TestMethod]
    public void AddExpense_WithNullExpense_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => _expenseDomainService.AddExpense(_dummyCategory, null));
    }

    [TestMethod]
    public void RemoveExpense_ShouldRemoveExpenseFromCategory()
    {
        // Arrange
        _expenseDomainService.AddExpense(_dummyCategory, _dummyExpense);

        // Act
        _expenseDomainService.RemoveExpense(_dummyCategory, _dummyExpense.Id);

        // Assert
        Assert.HasCount(0, _dummyCategory.Expenses);
    }

    [TestMethod]
    public void RemoveExpense_WithNullCategory_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => _expenseDomainService.RemoveExpense(null, _dummyExpense.Id));
    }

    [TestMethod]
    public void RemoveExpense_WithNullExpenseId_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => _expenseDomainService.RemoveExpense(_dummyCategory, Guid.Empty));
    }

    [TestMethod]
    public void GetTotalExpense_ShouldReturnSumOfExpenses()
    {
        // Arrange
        _expenseDomainService.AddExpense(_dummyCategory, _dummyExpense);

        var anotherExpense = new Expense
        {
            BudgetCategoryId = _dummyCategory.BudgetId,
            Amount = 100,
            Remarks = "Dining Out",
            ExpenseDate = DateTime.UtcNow
        };

        _expenseDomainService.AddExpense(_dummyCategory, anotherExpense);

        // Act
        var totalExpense = _expenseDomainService.GetTotalExpense(_dummyCategory).Value;

        // Assert
        Assert.AreEqual(150, totalExpense);
    }

    [TestMethod]
    public void GetTotalExpense_WithNullCategory_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => _expenseDomainService.GetTotalExpense(null));
    }

    [TestMethod]
    public void GetRemainingCategoryBudget_ShouldReturnRemainingBudget()
    {
        // Arrange
        _expenseDomainService.AddExpense(_dummyCategory, _dummyExpense);

        // Act
        var remainingBudget = _expenseDomainService.GetRemainingCategoryBudget(_dummyCategory).Value;

        // Assert
        Assert.AreEqual(450, remainingBudget);
    }

    [TestMethod]
    public void GetRemainingCategoryBudget_WithNullCategory_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => _expenseDomainService.GetRemainingCategoryBudget(null));
    }

    [TestMethod]
    public void IsCategoryLimitExceeded_ShouldReturnTrueIfLimitExceeded()
    {
        // Arrange
        var expensiveExpense = new Expense
        {
            BudgetCategoryId = _dummyCategory.BudgetId,
            Amount = 600,
            Remarks = "Expensive Dinner",
            ExpenseDate = DateTime.UtcNow
        };

        _expenseDomainService.AddExpense(_dummyCategory, expensiveExpense);

        // Act
        var isLimitExceeded = _expenseDomainService.IsCategoryLimitExceeded(_dummyCategory).Value;

        // Assert
        Assert.IsTrue(isLimitExceeded);
    }

    [TestMethod]
    public void IsCategoryLimitExceeded_WithNullCategory_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => _expenseDomainService.IsCategoryLimitExceeded(null));
    }

    [TestMethod]
    public void CanAddExpense_ShouldReturnTrueIfWithinLimit()
    {
        // Act
        var canAddExpense = _expenseDomainService.CanAddExpense(_dummyCategory, 100).Value;

        // Assert
        Assert.IsTrue(canAddExpense);
    }

    [TestMethod]
    public void CanAddExpense_WithNullCategory_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => _expenseDomainService.CanAddExpense(null, 100));
    }

    [TestMethod]
    public void CanAddExpense_WithNegativeAmount_ShouldThrowArgumentOutOfRangeException()
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => _expenseDomainService.CanAddExpense(_dummyCategory, -100));
    }

}
