using System;
using System.Collections.Generic;
using FoodDelivery.BusinessLogic.DTOs;
using FoodDelivery.BusinessLogic.Models; // Needed for ApiResponse<T>
using FoodDelivery.BusinessLogic.Services;
using FoodDelivery.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly MenuService _menuService;

    public MenuController(MenuService menuService)
    {
        _menuService = menuService;
    }

    // Unified endpoint for all menu items (with optional pagination & category filter)
    [HttpGet]
    public IActionResult GetAll([FromQuery] string? category, [FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var items = _menuService.GetFiltered(category, page, size);
        return Ok(ApiResponse<IEnumerable<MenuItem>>.SuccessResponse(items, "Menu items retrieved successfully."));
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var item = _menuService.GetById(id);
        return item == null
            ? NotFound(ApiResponse<string>.ErrorResponse("Menu item not found."))
            : Ok(ApiResponse<MenuItem>.SuccessResponse(item, "Menu item retrieved successfully."));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Add(MenuItemDto dto)
    {
        var newItem = new MenuItem
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Category = dto.Category
        };

        var added = _menuService.Add(newItem);
        return Ok(ApiResponse<MenuItem>.SuccessResponse(added, "Menu item added successfully."));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var deleted = _menuService.Delete(id);
        return deleted
            ? Ok(ApiResponse<string>.SuccessResponse("Deleted", "Menu item deleted successfully."))
            : NotFound(ApiResponse<string>.ErrorResponse("Menu item not found."));
    }

    // Optional: Explicit “filtered” route (kept only if you want a separate endpoint)
    [HttpGet("filtered")]
    public IActionResult GetFiltered([FromQuery] string? category, [FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var result = _menuService.GetFiltered(category, page, size);
        return Ok(ApiResponse<IEnumerable<MenuItem>>.SuccessResponse(result, "Filtered menu items retrieved successfully."));
    }
}
