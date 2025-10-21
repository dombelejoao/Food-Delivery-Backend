using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodDelivery.BusinessLogic.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get orders for a specific user with pagination
        public IEnumerable<Order> GetUserOrders(Guid userId, int page = 1, int size = 10)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
        }

        //Get all orders (Admin use only)
        public IEnumerable<Order> GetAllOrders(int page = 1, int size = 10)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
        }

        //Create an order from user's cart
        public Order CreateOrder(Guid userId)
        {
            var cartItems = _context.CartItems
                .Include(c => c.MenuItem)
                .Where(c => c.UserId == userId)
                .ToList();

            if (!cartItems.Any())
                throw new InvalidOperationException("Cart is empty, cannot create order.");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    MenuItemId = cartItem.MenuItemId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.MenuItem.Price
                };

                order.OrderItems.Add(orderItem);
                total += cartItem.Quantity * cartItem.MenuItem.Price;
            }

            order.TotalPrice = total;
            _context.Orders.Add(order);

            //Clear cart after creating order
            _context.CartItems.RemoveRange(cartItems);

            _context.SaveChanges();
            return order;
        }

        //Update order status (Admin use)
        public bool UpdateOrderStatus(Guid orderId, string newStatus)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                return false;

            order.Status = newStatus;

            if (newStatus.Equals("Delivered", StringComparison.OrdinalIgnoreCase))
                order.DeliveredAt = DateTime.UtcNow;

            _context.SaveChanges();
            return true;
        }

        //Get single order by ID
        public Order? GetOrderById(Guid orderId)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefault(o => o.Id == orderId);
        }
    }
}
