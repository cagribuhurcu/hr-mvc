using FluentValidation;
using HRProject.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HRProject.Entities.Validation.IdentificationNumberValidation;
using static HRProject.Entities.Validation.PhoneNumberValidation;

namespace HRProject.Entities.Validation
{
    public class EmployeeValidator: AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(companymanager => companymanager.FirstName).NotEmpty().WithMessage("FirstName cannot be empty").NotNull().WithMessage("FirstName cannot be null");
            RuleFor(companymanager => companymanager.LastName).NotEmpty().WithMessage("LastName cannot be empty").NotNull().WithMessage("LastName cannot be null");
            RuleFor(companymanager => companymanager.FirstName).NotEmpty().WithMessage("FirstName cannot be empty").NotNull().WithMessage("FirstName cannot be null");
            RuleFor(companymanager => companymanager.BirthPlace).NotEmpty().WithMessage("Birth Place cannot be empty").NotNull().WithMessage("Birth Place cannot be null");
            RuleFor(companymanager => companymanager.BirthDate).LessThan(x => x.HireDate.AddYears(-18)).WithMessage("Unable to enter employment before the age of 18").NotNull().WithMessage("Birth Date cannot be null").NotEmpty().WithMessage("Birth Date cannot be empty");
            RuleFor(companymanager => companymanager.IdentificationNumber).Length(11).WithMessage("The length of the Identification number must be 11 digits.");
            //RuleFor(companymanager => companymanager.HireDate).LessThan(x => x.QuitDate).WithMessage("The date of Hire Date must be less than the date of Quit from employment.");
            RuleFor(companymanager => companymanager.QuitDate).GreaterThan(x => x.HireDate).WithMessage("The date of Hire Date must be less than the date of Quit from employment.");
            RuleFor(companymanager => companymanager.Department).NotNull().WithMessage("Departmant cannot be null");
            RuleFor(companymanager => companymanager.JobID).NotNull().WithMessage("JobID cannot be null");
            RuleFor(companymanager => companymanager.Address).NotNull().WithMessage("Address cannot be null").NotEmpty().WithMessage("Address cannot be empty");
            RuleFor(companymanager => companymanager.PhoneNumber).Length(10).WithMessage("The length of the Phone number must be 10 digits.");
            RuleFor(companymanager => companymanager.PhoneNumber).Must(BeAllDigits).WithMessage("The phone number must consist of numeric expressions.");
            RuleFor(companymanager => companymanager.IdentificationNumber).Must(IdentificationNumberVerify).WithMessage("Wrong Identification Number");
            RuleFor(companymanager => companymanager.Address).MaximumLength(100).WithMessage("Address field is cannot more than 100 characters");
            RuleFor(companymanager => companymanager.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).WithMessage("Password must contain at least 8 characters.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
            .WithMessage("The password must contain at least one uppercase letter, one lowercase letter, one number and one special character.");
            RuleFor(companymanager => companymanager.Salary).NotNull().WithMessage("Salary cannot be null").GreaterThan(0).WithMessage("Salary cannot be equal to or less than 0");
        }
    }
}
