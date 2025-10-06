using System;
using System.Collections.Generic;

namespace FoodDelivery.DataAccess.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending";

    // Navigation
    public User User { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
