using System;
using System.Collections.Generic;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Entities;

namespace FoodDelivery.BusinessLogic.Services;

public class MenuService
{
    private readonly ApplicationDbContext _context;

    public MenuService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<MenuItem> GetAll() => _context.MenuItems.ToList();

    public MenuItem? GetById(Guid id) => _context.MenuItems.Find(id);

    public MenuItem Add(MenuItem item)
    {
        _context.MenuItems.Add(item);
        _context.SaveChanges();
        return item;
    }

    public bool Delete(Guid id)
    {
        var item = _context.MenuItems.Find(id);
        if (item == null) return false;
        _context.MenuItems.Remove(item);
        _context.SaveChanges();
        return true;
    }
}
