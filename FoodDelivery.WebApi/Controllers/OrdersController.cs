using FoodDelivery.BusinessLogic.Models;
using FoodDelivery.BusinessLogic.Services;
using FoodDelivery.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FoodDelivery.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    // ✅ Returns only the authenticated user's orders
    [Authorize]
    [HttpGet]
    public IActionResult GetUserOrders([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var userId = GetUserId();
        var result = _orderService.GetUserOrders(userId, page, size);
        return Ok(ApiResponse<IEnumerable<Order>>.SuccessResponse(result, "User orders retrieved successfully."));
    }

    // ✅ Returns all orders — ADMIN only, with unique route
    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public IActionResult GetAllOrders([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var result = _orderService.GetAllOrders(page, size);
        return Ok(ApiResponse<IEnumerable<Order>>.SuccessResponse(result, "All orders retrieved successfully."));
    }

    // Example POST (if you have it)
    [Authorize]
    [HttpPost]
    public IActionResult CreateOrder()
    {
        var userId = GetUserId();
        var order = _orderService.CreateOrder(userId);
        return Ok(ApiResponse<Order>.SuccessResponse(order, "Order created successfully."));
    }

    // Example helper (make sure this exists)
    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst("id")?.Value;
        return Guid.Parse(userIdClaim ?? throw new Exception("User ID not found in token"));
    }
}
