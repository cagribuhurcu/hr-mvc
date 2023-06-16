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
            RuleFor(employee => employee.FirstName).NotNull().WithMessage("FirstName cannot be null");
            RuleFor(employee => employee.LastName).NotNull().WithMessage("LastName cannot be null");
            RuleFor(employee => employee.FirstName).NotNull().WithMessage("FirstName cannot be null");
            RuleFor(employee => employee.BirthPlace).NotNull().WithMessage("Birth Place cannot be null");
            RuleFor(employee => employee.HireDate).NotNull().WithMessage("Hire Date cannot be null");
            RuleFor(employee => employee.BirthDate).NotNull().WithMessage("Birth Date cannot be null");
            RuleFor(employee => employee.HireDate).Must((employee, hireDate) => CompareHireDateAndQuitDateValidation.Compare(employee.HireDate, employee.QuitDate)).WithMessage("The date of Hire Date must be less than the date of Quit from employment."); 
            RuleFor(employee => employee.BirthDate).Must((employee, birthDate) => DateYearControlValidation.Control(employee.BirthDate)).WithMessage("Birth Date is not valid");
            RuleFor(employee => employee.BirthDate).Must((employee, birthDate) => HiringAgeValidation.ValidateAge(employee.BirthDate, employee.HireDate)).WithMessage("Unable to enter employment before the age of 18");
            RuleFor(employee => employee.IdentificationNumber).Length(11).WithMessage("The length of the Identification number must be 11 digits.");
            RuleFor(employee => employee.HireDate).Must((employee, hireDate) => DateYearControlValidation.Control(employee.HireDate)).WithMessage("Hire Date is not valid");
            RuleFor(employee => employee.QuitDate).Must((employee, quitDate) => QuitDateValidation.ValidateQuitDate(employee.QuitDate)).WithMessage("Quit Date is not valid");

            //RuleFor(employee => employee.HireDate).LessThan(x => x.QuitDate).WithMessage("The date of Hire Date must be less than the date of Quit from employment.");
            //RuleFor(employee => employee.QuitDate).GreaterThan(x => x.HireDate).WithMessage("The date of Hire Date must be less than the date of Quit from employment.");
            RuleFor(employee => employee.Department).NotNull().WithMessage("Departmant cannot be null");
            RuleFor(employee => employee.JobID).NotNull().WithMessage("Job cannot be null");
            RuleFor(employee => employee.Address).NotNull().WithMessage("Address cannot be null").NotEmpty().WithMessage("Address cannot be empty");
            RuleFor(employee => employee.CompanyId).NotNull().WithMessage("Company cannot be null");
            RuleFor(employee => employee.PhoneNumber).Length(10).WithMessage("The length of the Phone number must be 10 digits.");
            RuleFor(employee => employee.PhoneNumber).Must(BeAllDigits).WithMessage("The phone number must consist of numeric expressions.");
            RuleFor(employee => employee.IdentificationNumber).Must(IdentificationNumberVerify).WithMessage("Wrong Identification Number");
            RuleFor(employee => employee.Address).MaximumLength(100).WithMessage("Address field is cannot more than 100 characters");
            RuleFor(employee => employee.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).WithMessage("Password must contain at least 8 characters.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
            .WithMessage("The password must contain at least one uppercase letter, one lowercase letter, one number and one special character.");
            RuleFor(employee => employee.Salary).NotNull().WithMessage("Salary cannot be null").GreaterThan(0).WithMessage("Salary cannot be equal to or less than 0");
        }
    }
}
