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
public class RatingsController : ControllerBase
{
    private readonly RatingService _ratingService;

    public RatingsController(RatingService ratingService)
    {
        _ratingService = ratingService;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public IActionResult AddRating([FromBody] RatingDto dto)
    {
        try
        {
            var userId = GetUserId();
            var result = _ratingService.AddRating(userId, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("dish/{menuItemId}")]
    public IActionResult GetRatingsForDish(Guid menuItemId)
    {
        return Ok(_ratingService.GetRatingsForDish(menuItemId));
    }
}
