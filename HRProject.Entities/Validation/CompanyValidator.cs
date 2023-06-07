using FluentValidation;
using HRProject.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HRProject.Entities.Validation.PhoneNumberValidation;

namespace HRProject.Entities.Validation
{
    public class CompanyValidator : AbstractValidator<Company>
    {
        public CompanyValidator()
        {
            RuleFor(company => company.CompanyName).NotEmpty().WithMessage("Company Name cannot be empty").NotNull().WithMessage("Company Name cannot be null");
            RuleFor(company => company.Title).NotNull().WithMessage("Company Title cannot be null");
            RuleFor(company => company.MERSISNo).NotEmpty().WithMessage("MERSIS No cannot be empty").NotNull().WithMessage("MERSIS No cannot be null");
            RuleFor(company => company.TaxNumber).NotEmpty().WithMessage("Tax Number cannot be empty").NotNull().WithMessage("Tax Number cannot be null");
            RuleFor(company => company.TaxAdministration).NotEmpty().WithMessage("Tax Administration cannot be empty").NotNull().WithMessage("Tax Administration cannot be null");
            RuleFor(company => company.LogoURL).NotEmpty().WithMessage("Photo URL cannot be empty").NotNull().WithMessage("Photo URL cannot be null");
            RuleFor(company => company.PhoneNumber).NotEmpty().WithMessage("Phone Number cannot be empty").NotNull().WithMessage("Phone Number cannot be null").Length(10).WithMessage("The length of the Phone number must be 10 digits.");
            RuleFor(company => company.PhoneNumber).Must(BeAllDigits).WithMessage("The phone number must consist of numeric expressions.");
            RuleFor(company => company.Address).NotEmpty().WithMessage("Address cannot be empty").NotNull().WithMessage("Address cannot be null");
            RuleFor(company => company.TotalEmployees).GreaterThanOrEqualTo(0).WithMessage("Total number of employees must be greater than or equal to 0").NotEmpty().WithMessage("Total number of employee cannot be empty").NotNull().WithMessage("Total number of employee cannot be null");
            RuleFor(company => company.FoundationDate).LessThanOrEqualTo(DateTime.Now).WithMessage("Founding date must be less than or equal to today").NotEmpty().WithMessage("Foundation Date cannot be empty").NotNull().WithMessage("Foundation Date cannot be null");
            RuleFor(company => company.ContractStartDate).LessThanOrEqualTo(DateTime.Now).WithMessage("Conctract Start date must be less than or equal to today").NotEmpty().WithMessage("Contract Start Date cannot be empty").NotNull().WithMessage("Contract Start Date cannot be null");
            RuleFor(company => company.ContractEndDate).GreaterThan(c => c.ContractStartDate).WithMessage("Contract End Date must be greater than Contract Start Date").NotEmpty().WithMessage("Contract End Date cannot be empty").NotNull().WithMessage("Contract End Date cannot be null");
        }
    }
}
