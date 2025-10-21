using FoodDelivery.BusinessLogic.Services;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;

public class OrderServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        _service = new OrderService(_context);
    }

    [Fact]
    public void CreateOrder_ShouldThrow_WhenCartIsEmpty()
    {
        var userId = Guid.NewGuid();
        Assert.Throws<InvalidOperationException>(() => _service.CreateOrder(userId));
    }

    [Fact]
    public void CreateOrder_ShouldCalculateTotalCorrectly()
    {
        var userId = Guid.NewGuid();
        var menuItem = new MenuItem { Id = Guid.NewGuid(), Name = "Burger", Price = 5.0m };
        _context.MenuItems.Add(menuItem);
        _context.CartItems.Add(new CartItem { Id = Guid.NewGuid(), UserId = userId, MenuItemId = menuItem.Id, MenuItem = menuItem, Quantity = 2 });
        _context.SaveChanges();

        var order = _service.CreateOrder(userId);

        Assert.Equal(10.0m, order.TotalPrice);
        Assert.Single(order.OrderItems);
    }
}
