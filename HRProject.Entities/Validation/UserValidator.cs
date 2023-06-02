using FluentValidation;
using HRProject.Entities.Entities;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HRProject.Entities.Validation.IdentificationNumberValidation;
using static HRProject.Entities.Validation.PhoneNumberValidation;

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
            RuleFor(user => user.BirthDate).LessThan(x => x.HireDate.AddYears(-18)).WithMessage("Unable to enter employment before the age of 18").NotNull().WithMessage("Birth Date cannot be null").NotEmpty().WithMessage("Birth Date cannot be empty");
            RuleFor(user => user.IdentificationNumber).Length(11).WithMessage("The length of the Identification number must be 11 digits.");
            //RuleFor(user => user.HireDate).LessThan(x => x.QuitDate).WithMessage("The date of Hire Date must be less than the date of Quit from employment.");
            RuleFor(user => user.QuitDate).GreaterThan(x => x.HireDate).WithMessage("The date of Hire Date must be less than the date of Quit from employment.");
            RuleFor(user => user.Department).NotNull().WithMessage("Departmant cannot be null");
            RuleFor(user => user.JobID).NotNull().WithMessage("JobID cannot be null");
            RuleFor(user => user.Address).NotNull().WithMessage("Address cannot be null").NotEmpty().WithMessage("Address cannot be empty");
            RuleFor(user => user.PhoneNumber).Length(10).WithMessage("The length of the Phone number must be 10 digits.").Must(BeAllDigits).WithMessage("The phone number must consist of numeric expressions.");
            RuleFor(user => user.IdentificationNumber).Must(IdentificationNumberVerify).WithMessage("Wrong Identification Number");
            RuleFor(user => user.Address).MaximumLength(100).WithMessage("Address field is cannot more than 100 characters");
        }
    }
}
