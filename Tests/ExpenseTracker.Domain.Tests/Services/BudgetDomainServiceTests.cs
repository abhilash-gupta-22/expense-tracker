using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Domain.Services;

namespace ExpenseTracker.Domain.Tests.Services;

[TestClass]
public class BudgetDomainServiceTests
{
    private readonly IBudgetDomainService _budgetDomainService;
    private Budget? _dummyBudget;

    public BudgetDomainServiceTests()
    {
        _budgetDomainService = new BudgetDomainService();
    }

    [TestInitialize]
    public void TestInitialize()
    {
        _dummyBudget = _budgetDomainService.CreateBudget(1000, 5, 2026).Value;
    }

    [TestMethod]
    public void CreateBudget_ShouldCreateBudgetWithValidParameters()
    {
        // Arrange
        var totalBudget = 1000;
        var month = 5;
        var year = 2026;

        // Act
        var budget = _budgetDomainService.CreateBudget(totalBudget, month, year).Value;

        // Assert
        Assert.IsNotNull(budget);
        Assert.AreEqual(totalBudget, budget.TotalBudget);
        Assert.AreEqual(month, budget.Month);
        Assert.AreEqual(year, budget.Year);
    }

    [TestMethod]
    [DataRow(0, 5, 2026)]
    [DataRow(1000, 13, 2026)]
    [DataRow(1000, 5, 0)]
    public void CreateBudget_WithInvalidTotalBudget_ThrowsArgumentOutOfRangeException(int totalBudget, int month, int year)
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => _budgetDomainService.CreateBudget(totalBudget, month, year));
    }

    [TestMethod]
    public void UpdateBudget_ShouldUpdateBudgetWithValidParameters()
    {
        // Arrange
        var newTotalBudget = 1500;

        // Act
        _budgetDomainService.UpdateBudget(_dummyBudget, newTotalBudget);

        // Assert
        Assert.AreEqual(newTotalBudget, _dummyBudget.TotalBudget);
    }

    [TestMethod]
    public void UpdateBudget_WithInvalidParameters_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => _budgetDomainService.UpdateBudget(null, 1500));
    }

    [TestMethod]
    public void UpdateBudget_WithInvalidTotalBudget_ThrowsArgumentOutOfRangeException()
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => _budgetDomainService.UpdateBudget(_dummyBudget, -100));
    }

    [TestMethod]
    public void AddCategory_ShouldAddCategorytWithValidParameters()
    {
        // Arrange
        var budgetCategory = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Food",
            AllocatedBudget = 200
        };

        // Act
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory);

        // Assert
        Assert.IsNotNull(_dummyBudget);
        Assert.IsNotEmpty(_dummyBudget.BudgetCategories);
        Assert.AreEqual("Food", _dummyBudget.BudgetCategories.First().Name);
        Assert.AreEqual(200, _dummyBudget.BudgetCategories.First().AllocatedBudget);
    }

    [TestMethod]
    public void AddCategory_WithInvalidParameters_ThrowsArgumentNullException()
    {
        // Arrange
        var budgetCategory = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Food",
            AllocatedBudget = 200
        };

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => _budgetDomainService.AddCategory(null, budgetCategory));
        Assert.ThrowsExactly<ArgumentNullException>(() => _budgetDomainService.AddCategory(_dummyBudget, null));
    }

    [TestMethod]
    public void UpdateCategoryAllocation_ShouldUpdateCategoryAllocationWithValidParameters()
    {
        // Arrange
        var budgetCategory = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Food",
            AllocatedBudget = 200
        };
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory);

        // Act
        _budgetDomainService.UpdateCategoryAllocation(_dummyBudget, budgetCategory.Id, 300);

        // Assert
        Assert.AreEqual(300, _dummyBudget.BudgetCategories.First().AllocatedBudget);
    }

    [TestMethod]
    public void UpdateCategoryAllocation_WithInvalidParameters_ThrowsArgumentNullException()
    {
        // Arrange
        var budgetCategory = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Food",
            AllocatedBudget = 200
        };
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => _budgetDomainService.UpdateCategoryAllocation(null, budgetCategory.Id, 300));
        Assert.ThrowsExactly<ArgumentException>(() => _budgetDomainService.UpdateCategoryAllocation(_dummyBudget, Guid.Empty, 300));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => _budgetDomainService.UpdateCategoryAllocation(_dummyBudget, budgetCategory.Id, -1));
    }

    [TestMethod]
    public void RemoveCategory_ShouldRemoveCategoryWithValidParameters()
    {
        // Arrange
        var budgetCategory = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Food",
            AllocatedBudget = 200
        };
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory);

        // Act
        _budgetDomainService.RemoveCategory(_dummyBudget, budgetCategory.Id);

        // Assert
        Assert.DoesNotContain(budgetCategory, _dummyBudget.BudgetCategories);
    }

    [TestMethod]
    public void RemoveCategory_WithInvalidParameters_ThrowsArgumentNullException()
    {
        // Arrange
        var budgetCategory = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Food",
            AllocatedBudget = 200
        };
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => _budgetDomainService.RemoveCategory(null, budgetCategory.Id));
        Assert.ThrowsExactly<ArgumentException>(() => _budgetDomainService.RemoveCategory(_dummyBudget, Guid.Empty));
    }

    [TestMethod]
    public void GetAllocatedAmount_ShouldReturnCorrectAllocatedAmount()
    {
        // Arrange
        var budgetCategory1 = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Food",
            AllocatedBudget = 200
        };

        var budgetCategory2 = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Transport",
            AllocatedBudget = 100
        };

        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory1);
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory2);

        // Act
        var allocatedAmount = _budgetDomainService.GetAllocatedAmount(_dummyBudget);

        // Assert
        Assert.AreEqual(300, allocatedAmount.Value);
    }

    [TestMethod]
    public void GetRemainingBudget_ShouldReturnCorrectRemainingBudget()
    {
        // Arrange
        var budgetCategory1 = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Food",
            AllocatedBudget = 200,
            Expenses = new List<Expense>
            {
                new Expense { Amount = 50 },
                new Expense { Amount = 30 }
            }
        };

        var budgetCategory2 = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Transport",
            AllocatedBudget = 100,
            Expenses = new List<Expense>
            {
                new Expense { Amount = 20 }
            }
        };

        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory1);
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory2);

        // Act
        var remainingBudget = _budgetDomainService.GetRemainingBudget(_dummyBudget);

        // Assert
        Assert.AreEqual(1000 - (50 + 30 + 20), remainingBudget.Value);
    }

    [TestMethod]
    public void GetRemainingBudget_WithNoExpenses_ShouldReturnTotalBudget()
    {
        // Arrange
        var budgetCategory1 = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Food",
            AllocatedBudget = 200
        };
        var budgetCategory2 = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Transport",
            AllocatedBudget = 100
        };
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory1);
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory2);

        // Act
        var remainingBudget = _budgetDomainService.GetRemainingBudget(_dummyBudget);

        // Assert
        Assert.AreEqual(1000, remainingBudget.Value);
    }

    [TestMethod]
    public void CanAllocateBudget_ShouldReturnTrueWhenTotalBudgetNotExceeded()
    {
        // Arrange
        var budgetCategory1 = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Food",
            AllocatedBudget = 600
        };
        var budgetCategory2 = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Transport",
            AllocatedBudget = 300
        };
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory1);
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory2);

        // Act
        var canAllocateMore = _budgetDomainService.CanAllocateBudget(_dummyBudget, 100);

        // Assert
        Assert.IsTrue(canAllocateMore.Value);
    }

    [TestMethod]
    public void CanAllocateBudget_ShouldReturnFalseWhenTotalBudgetExceeded()
    {
        // Arrange
        var budgetCategory1 = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Food",
            AllocatedBudget = 600
        };
        var budgetCategory2 = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Transport",
            AllocatedBudget = 400
        };
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory1);
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory2);

        // Act
        var canAllocateMore = _budgetDomainService.CanAllocateBudget(_dummyBudget, 100);

        // Assert
        Assert.IsFalse(canAllocateMore.Value);
    }

    [TestMethod]
    public void IsBudgetAllocationValid_ShouldReturnTrueForValidAllocation()
    {
        // Arrange
        var budgetCategory1 = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Food",
            AllocatedBudget = 600
        };

        var budgetCategory2 = new BudgetCategory
        {
            BudgetId = _dummyBudget.Id,
            Name = "Transport",
            AllocatedBudget = 400
        };

        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory1);
        _budgetDomainService.AddCategory(_dummyBudget, budgetCategory2);

        // Act
        var isValidAllocation = _budgetDomainService.IsBudgetAllocationValid(_dummyBudget);

        // Assert
        Assert.IsTrue(isValidAllocation.Value);
    }
}
