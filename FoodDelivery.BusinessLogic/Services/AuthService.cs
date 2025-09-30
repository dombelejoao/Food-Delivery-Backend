using System;
using BCrypt.Net;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;

namespace FoodDelivery.BusinessLogic.Services;

public class AuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool Register(string email, string password, string name)
    {
        if (_context.Users.Any(u => u.Email == email))
            return false;

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = hashedPassword,
            Name = name
        };

        _context.Users.Add(user);
        _context.SaveChanges();
        return true;
    }

    public User? ValidateUser(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null) return null;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? user : null;
    }
}
