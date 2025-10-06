using System;

namespace FoodDelivery.DataAccess.Entities;

public class CartItem
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MenuItemId { get; set; }
    public int Quantity { get; set; }

    // Navigation properties
    public User User { get; set; }
    public MenuItem MenuItem { get; set; }
}
