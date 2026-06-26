using EventKori.Application.DTOs;
using FluentValidation;

namespace EventKori.Application.Validators;

public class CreateEventRequestDtoValidator : AbstractValidator<CreateEventRequestDto>
{
    public CreateEventRequestDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.EventType).NotEmpty();
        RuleFor(x => x.EventDate).GreaterThan(DateTime.UtcNow);
        RuleFor(x => x.Location).NotEmpty();
        RuleFor(x => x.AttendeesCount).GreaterThan(0);
        RuleFor(x => x.Budget).GreaterThanOrEqualTo(0);
    }
}
