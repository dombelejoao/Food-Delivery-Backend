using System;
using System.Security.Claims;
using FoodDelivery.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public IActionResult CreateOrder()
    {
        var userId = GetUserId();
        try
        {
            var order = _orderService.CreateOrder(userId);
            return Ok(order);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult GetUserOrders()
    {
        var userId = GetUserId();
        return Ok(_orderService.GetUserOrders(userId));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public IActionResult GetAllOrders([FromServices] ApplicationDbContext context)
    {
        return Ok(context.Orders.ToList());
    }
}
