using System;

namespace FoodDelivery.BusinessLogic.DTOs;

public class CartItemDto
{
    public Guid MenuItemId { get; set; }
    public int Quantity { get; set; }
}
