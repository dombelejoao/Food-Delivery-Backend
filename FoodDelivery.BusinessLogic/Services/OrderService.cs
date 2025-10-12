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

    public IEnumerable<Order> GetUserOrders(Guid userId, int page = 1, int size = 10)
    {
        return _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .Where(o => o.UserId == userId)
            .Skip((page - 1) * size)
            .Take(size)
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

    public bool UpdateOrderStatus(Guid orderId, string status, string? notes = null)
    {
        var order = _context.Orders.Find(orderId);
        if (order == null) return false;

        order.Status = status;
        order.DeliveryNotes = notes ?? order.DeliveryNotes;
        if (status == "Delivered") order.DeliveredAt = DateTime.UtcNow;

        _context.SaveChanges();
        return true;
    }

}
