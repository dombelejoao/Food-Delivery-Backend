using System;
using FoodDelivery.BusinessLogic.DTOs;
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

    [HttpGet]
    public IActionResult GetAll() => Ok(_menuService.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var item = _menuService.GetById(id);
        return item == null ? NotFound() : Ok(item);
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
        return Ok(_menuService.Add(newItem));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        return _menuService.Delete(id) ? Ok("Deleted") : NotFound();
    }

    [HttpGet]
    public IActionResult GetFiltered([FromQuery] string? category, [FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var result = _menuService.GetFiltered(category, page, size);
        return Ok(result);
    }

}
