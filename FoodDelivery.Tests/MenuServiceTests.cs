using FoodDelivery.BusinessLogic.Services;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;

public class MenuServiceTests
{
    private readonly MenuService _service;
    private readonly ApplicationDbContext _context;

    public MenuServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        _service = new MenuService(_context);
    }

    [Fact]
    public void AddMenuItem_ShouldAddItemToDatabase()
    {
        var item = new MenuItem { Id = Guid.NewGuid(), Name = "Test Pizza", Price = 12.5m, Category = "Pizza" };
        _service.Add(item);

        var saved = _context.MenuItems.FirstOrDefault(x => x.Name == "Test Pizza");
        Assert.NotNull(saved);
        Assert.Equal(12.5m, saved.Price);
    }
}
