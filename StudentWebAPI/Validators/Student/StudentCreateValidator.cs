using FluentValidation;
using StudentWebAPI.DTO;

namespace StudentWebAPI.Validators.Student
{
    public class StudentCreateValidator : AbstractValidator<CreateStudentDTO>
    {
        public StudentCreateValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(5)
                .MaximumLength(50)
                .Matches("^[a-zA-Z ]*$")
                .WithMessage("Name should contain only letters");


            RuleFor(x => x.Age)
                .InclusiveBetween(5, 30)
                .WithMessage("Age must be between 5 and 30");


            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();


            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^[0-9]{10}$")
                .WithMessage("Phone must be 10 digits");


            RuleFor(x => x)
                .Must(x => !(x.Age < 10 && x.Name.Length < 5))
                .WithMessage("Young students must have longer names");
        }
    }
}
