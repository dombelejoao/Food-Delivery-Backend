using FluentValidation;
using FoodDelivery.BusinessLogic.DTOs;

namespace FoodDelivery.BusinessLogic.Validators;

public class RatingValidator : AbstractValidator<RatingDto>
{
    public RatingValidator()
    {
        RuleFor(x => x.Score).InclusiveBetween(1, 5);
        RuleFor(x => x.Comment).MaximumLength(500);
    }
}
