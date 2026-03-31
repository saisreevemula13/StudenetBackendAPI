using FluentValidation;
using StudentWebAPI.DTO;

namespace StudentWebAPI.Validators.Registration
{
    public class RegistrationCreateValidator:AbstractValidator<RegistrationCreateDTO>
    {
        public RegistrationCreateValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0);

            RuleFor(x => x.EventId)
                .GreaterThan(0);
        }
    }
}
