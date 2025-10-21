using FluentValidation;
using FoodDelivery.BusinessLogic.DTOs;

namespace FoodDelivery.BusinessLogic.Validators;

public class MenuItemValidator : AbstractValidator<MenuItemDto>
{
    public MenuItemValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Category).NotEmpty();
    }
}
