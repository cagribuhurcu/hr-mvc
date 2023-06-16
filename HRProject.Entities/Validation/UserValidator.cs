using FluentValidation;
using HRProject.Entities.Entities;
using static HRProject.Entities.Validation.IdentificationNumberValidation;
using static HRProject.Entities.Validation.PhoneNumberValidation;
using static HRProject.Entities.Validation.HiringAgeValidation;


namespace HRProject.Entities.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.FirstName).NotEmpty().WithMessage("FirstName cannot be empty").NotNull().WithMessage("FirstName cannot be null");
            RuleFor(user => user.LastName).NotEmpty().WithMessage("LastName cannot be empty").NotNull().WithMessage("LastName cannot be null");
            RuleFor(user => user.FirstName).NotEmpty().WithMessage("FirstName cannot be empty").NotNull().WithMessage("FirstName cannot be null");
            RuleFor(user => user.BirthPlace).NotEmpty().WithMessage("Birth Place cannot be empty").NotNull().WithMessage("Birth Place cannot be null");
            RuleFor(user => user.BirthDate).Must((user, birthDate) => HiringAgeValidation.ValidateAge(user.BirthDate,user.HireDate)).WithMessage("Unable to enter employment before the age of 18");
            //RuleFor(user => user.IdentificationNumber).Length(11).WithMessage("The length of the Identification number must be 11 digits.");
            //RuleFor(user => user.HireDate).LessThan(x => x.QuitDate).WithMessage("The date of Hire Date must be less than the date of Quit from employment.");
            //RuleFor(user => user.QuitDate).GreaterThan(x => x.HireDate).WithMessage("The date of Hire Date must be less than the date of Quit from employment.");
            RuleFor(user => user.HireDate).Must((user, hireDate) => CompareHireDateAndQuitDateValidation.Compare(user.HireDate, user.QuitDate)).WithMessage("The date of Hire Date must be less than the date of Quit from employment.");
            RuleFor(user => user.HireDate).Must((user, hireDate) => DateYearControlValidation.Control(user.HireDate)).WithMessage("Hire Date is not valid");
            RuleFor(user => user.QuitDate).Must((user, quitDate) => QuitDateValidation.ValidateQuitDate(user.QuitDate)).WithMessage("Quit Date is not valid");
            RuleFor(user => user.BirthDate).Must((user, birthDate) => DateYearControlValidation.Control(user.BirthDate)).WithMessage("Birth Date is not valid");
            RuleFor(user => user.Department).NotNull().WithMessage("Departmant cannot be null");
            //RuleFor(user => user.JobID).NotNull().WithMessage("JobID cannot be null");
            RuleFor(user => user.Address).NotNull().WithMessage("Address cannot be null");
            RuleFor(user => user.PhoneNumber).Length(10).WithMessage("The length of the Phone number must be 10 digits.");
            RuleFor(user => user.PhoneNumber).Must(BeAllDigits).WithMessage("The phone number must consist of numeric expressions.");
            RuleFor(user => user.IdentificationNumber).Must(IdentificationNumberVerify).WithMessage("Wrong Identification Number");
            RuleFor(user => user.Address).MaximumLength(100).WithMessage("Address field is cannot more than 100 characters");
            RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).WithMessage("Password must contain at least 8 characters.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
            .WithMessage("The password must contain at least one uppercase letter, one lowercase letter, one number and one special character.");
        }
    }
}
