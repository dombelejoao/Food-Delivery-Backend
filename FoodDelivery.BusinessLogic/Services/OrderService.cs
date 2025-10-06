using System;
using System.Collections.Generic;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.BusinessLogic.Services;

public class OrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Order> GetUserOrders(Guid userId)
    {
        return _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .Where(o => o.UserId == userId)
            .ToList();
    }

    public Order CreateOrder(Guid userId)
    {
        var cartItems = _context.CartItems
            .Include(c => c.MenuItem)
            .Where(c => c.UserId == userId)
            .ToList();

        if (!cartItems.Any())
            throw new Exception("Cart is empty.");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TotalPrice = cartItems.Sum(c => c.MenuItem.Price * c.Quantity),
            Status = "Pending"
        };

        foreach (var c in cartItems)
        {
            order.OrderItems.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                MenuItemId = c.MenuItemId,
                Quantity = c.Quantity,
                UnitPrice = c.MenuItem.Price
            });
        }

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cartItems);
        _context.SaveChanges();

        return order;
    }
}
