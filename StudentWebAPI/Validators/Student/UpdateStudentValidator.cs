using FluentValidation;
using StudentWebAPI.DTO;

namespace StudentWebAPI.Validators.Student
{
    public class UpdateStudentValidator:AbstractValidator<UpdateStudentDto>
    {
        public UpdateStudentValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(50)
                .When(x => x.Name != null);

            RuleFor(x => x.Age)
                .GreaterThan(0)
                .LessThan(100)
                .When(x => x.Age.HasValue);

            RuleFor(x=>x.Email)
                .EmailAddress()
                .When(x=>!string.IsNullOrEmpty(x.Email));

            RuleFor(x=>x.PhoneNumber)
                .Matches(@"^[0-9]{10}$")
                .When(x=>!string.IsNullOrEmpty(x.PhoneNumber));
        }
    }
}
