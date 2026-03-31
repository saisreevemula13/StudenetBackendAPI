using FluentValidation;
using StudentWebAPI.DTO;

namespace StudentWebAPI.Validators.Events
{
    public class CreateEVentValidatorcs:AbstractValidator<CreateEventDTO>
    {
        public CreateEVentValidatorcs()
        {
            RuleFor(x => x.Title)
           .NotEmpty()
           .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(200)
                .When(x => x.Description != null);

            RuleFor(x => x.EventDate)
                .GreaterThan(DateTime.Now)
                .WithMessage("Event date must be in the future");

            RuleFor(x => x.Capacity)
                .GreaterThan(0)
                .LessThanOrEqualTo(1000);
        }
    }
}
