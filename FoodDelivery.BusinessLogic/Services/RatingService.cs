using FoodDelivery.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.BusinessLogic.Services;

public class RatingService
{
    private readonly ApplicationDbContext _context;

    public RatingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool UserOrderedDish(Guid userId, Guid menuItemId)
    {
        return _context.Orders
            .Include(o => o.OrderItems)
            .Any(o => o.UserId == userId && o.OrderItems.Any(oi => oi.MenuItemId == menuItemId));
    }

    public Rating AddRating(Guid userId, RatingDto dto)
    {
        if (!UserOrderedDish(userId, dto.MenuItemId))
            throw new Exception("You can only rate dishes you have ordered.");

        var rating = new Rating
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MenuItemId = dto.MenuItemId,
            Score = dto.Score,
            Comment = dto.Comment
        };

        _context.Ratings.Add(rating);
        _context.SaveChanges();
        return rating;
    }

    public IEnumerable<Rating> GetRatingsForDish(Guid menuItemId)
        => _context.Ratings.Where(r => r.MenuItemId == menuItemId).ToList();
}
