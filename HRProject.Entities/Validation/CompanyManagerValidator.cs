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
    public class CompanyManagerValidator: AbstractValidator<CompanyManagerEntity>
    {
        public CompanyManagerValidator()
        {
            RuleFor(companymanager => companymanager.FirstName).NotNull().WithMessage("FirstName cannot be null");
            RuleFor(companymanager => companymanager.LastName).NotNull().WithMessage("LastName cannot be null");
            RuleFor(companymanager => companymanager.FirstName).NotNull().WithMessage("FirstName cannot be null");
            RuleFor(companymanager => companymanager.BirthPlace).NotNull().WithMessage("Birth Place cannot be null");
            RuleFor(companymanager => companymanager.HireDate).NotNull().WithMessage("Hire Date cannot be null");
            RuleFor(companymanager => companymanager.BirthDate).NotNull().WithMessage("Birth Date cannot be null");
            RuleFor(user => user.BirthDate).Must((user, birthDate) => DateYearControlValidation.Control(user.BirthDate)).WithMessage("Birth Date is not valid");
            //RuleFor(companymanager => companymanager.QuitDate).NotNull().WithMessage("Quit Date cannot be null");
            RuleFor(companymanager => companymanager.IdentificationNumber).Length(11).WithMessage("The length of the Identification number must be 11 digits.");
            //RuleFor(companymanager => companymanager.HireDate).LessThan(x => x.QuitDate).WithMessage("The date of Hire Date must be less than the date of Quit from employment.");
            //RuleFor(companymanager => companymanager.QuitDate).GreaterThan(x => x.HireDate).WithMessage("The date of Hire Date must be less than the date of Quit from employment.");
            RuleFor(user => user.HireDate).Must((user, hireDate) => CompareHireDateAndQuitDateValidation.Compare(user.HireDate, user.QuitDate)).WithMessage("The date of Hire Date must be less than the date of Quit from employment.");
            RuleFor(user => user.HireDate).Must((user, hireDate) => DateYearControlValidation.Control(user.HireDate)).WithMessage("Hire Date is not valid");
            RuleFor(user => user.QuitDate).Must((user, quitDate) => QuitDateValidation.ValidateQuitDate(user.QuitDate)).WithMessage("Quit Date is not valid");

            RuleFor(user => user.BirthDate).Must((user, birthDate) => HiringAgeValidation.ValidateAge(user.BirthDate, user.HireDate)).WithMessage("Unable to enter employment before the age of 18");
            RuleFor(companymanager => companymanager.Department).NotNull().WithMessage("Departmant cannot be null");
            RuleFor(companymanager => companymanager.JobID).NotNull().WithMessage("Job cannot be null");
            RuleFor(companymanager => companymanager.Address).NotNull().WithMessage("Address cannot be null");
            RuleFor(companymanager => companymanager.CompanyId).NotNull().WithMessage("Company cannot be null");
            RuleFor(companymanager => companymanager.PhoneNumber).Length(10).WithMessage("The length of the Phone number must be 10 digits.");
            RuleFor(companymanager => companymanager.PhoneNumber).Must(BeAllDigits).WithMessage("The phone number must consist of numeric expressions.");
            RuleFor(companymanager => companymanager.IdentificationNumber).Must(IdentificationNumberVerify).WithMessage("Wrong Identification Number").NotNull().WithMessage("Identification number cannot be null");
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
