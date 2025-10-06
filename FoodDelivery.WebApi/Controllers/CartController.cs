using System;
using System.Security.Claims;
using FoodDelivery.BusinessLogic.DTOs;
using FoodDelivery.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public IActionResult GetCart()
    {
        var userId = GetUserId();
        return Ok(_cartService.GetUserCart(userId));
    }

    [HttpPost]
    public IActionResult AddToCart([FromBody] CartItemDto dto)
    {
        var userId = GetUserId();
        _cartService.AddToCart(userId, dto.MenuItemId, dto.Quantity);
        return Ok("Item added to cart.");
    }

    [HttpDelete("{menuItemId}")]
    public IActionResult RemoveFromCart(Guid menuItemId)
    {
        var userId = GetUserId();
        _cartService.RemoveFromCart(userId, menuItemId);
        return Ok("Item removed from cart.");
    }
}
