using System;

namespace FoodDelivery.DataAccess.Entities;

public class Rating
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MenuItemId { get; set; }
    public int Score { get; set; } // 1–5 stars
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; }
    public MenuItem MenuItem { get; set; }
}
