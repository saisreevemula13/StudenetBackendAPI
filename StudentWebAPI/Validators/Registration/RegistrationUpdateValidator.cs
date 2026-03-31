using FluentValidation;
using StudentWebAPI.DTO;

namespace StudentWebAPI.Validators.Registration
{
    public class RegistrationUpdateValidator:AbstractValidator<RegistrationUpdateDTO>
    {
        public RegistrationUpdateValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0);

            RuleFor(x => x.EventId)
                .GreaterThan(0);
        }
    }
}
