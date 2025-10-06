using System;
using System.Collections.Generic;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;

namespace FoodDelivery.BusinessLogic.Services;

public class CartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<CartItem> GetUserCart(Guid userId)
    {
        return _context.CartItems
            .Where(c => c.UserId == userId)
            .ToList();
    }

    public void AddToCart(Guid userId, Guid menuItemId, int quantity)
    {
        var existing = _context.CartItems
            .FirstOrDefault(c => c.UserId == userId && c.MenuItemId == menuItemId);

        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                MenuItemId = menuItemId,
                Quantity = quantity
            };
            _context.CartItems.Add(cartItem);
        }

        _context.SaveChanges();
    }

    public void RemoveFromCart(Guid userId, Guid menuItemId)
    {
        var item = _context.CartItems
            .FirstOrDefault(c => c.UserId == userId && c.MenuItemId == menuItemId);

        if (item != null)
        {
            _context.CartItems.Remove(item);
            _context.SaveChanges();
        }
    }
}
