using System;

namespace FoodDelivery.BusinessLogic.DTOs;

public class RatingDto
{
    public Guid MenuItemId { get; set; }
    public int Score { get; set; }
    public string Comment { get; set; } = string.Empty;
}
