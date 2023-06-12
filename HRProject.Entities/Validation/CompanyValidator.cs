using FluentValidation;
using HRProject.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HRProject.Entities.Validation.PhoneNumberValidation;
using static HRProject.Entities.Validation.TaxNumberValidation;
using static HRProject.Entities.Validation.MersisValidation;

namespace HRProject.Entities.Validation
{
    public class CompanyValidator : AbstractValidator<Company>
    {
        public CompanyValidator()
        {
            RuleFor(company => company.CompanyName).NotNull().WithMessage("Company Name cannot be null");
            RuleFor(company => company.Title).NotNull().WithMessage("Company Title cannot be null");
            RuleFor(company => company.MERSISNo).NotNull().WithMessage("MERSIS No cannot be null").Must(IsMersisNoValid).WithMessage("The MERSIS Number is not valid");
            RuleFor(company => company.TaxNumber).NotNull().WithMessage("Tax Number cannot be null").Must(IsTaxNumber).WithMessage("The tax number is not valid"); ;
            RuleFor(company => company.TaxAdministration).NotNull().WithMessage("Tax Administration cannot be null");
            RuleFor(company => company.LogoURL).NotNull().WithMessage("Photo URL cannot be null");
            RuleFor(company => company.PhoneNumber).NotNull().WithMessage("Phone Number cannot be null").Length(10).WithMessage("The length of the Phone number must be 10 digits.");
            RuleFor(company => company.PhoneNumber).Must(BeAllDigits).WithMessage("The phone number must consist of numeric expressions.");
            RuleFor(company => company.Address).NotNull().WithMessage("Address cannot be null");
            RuleFor(company => company.TotalEmployees).GreaterThanOrEqualTo(0).WithMessage("Total number of employees must be greater than or equal to 0").NotNull().WithMessage("Total number of employee cannot be null");
            RuleFor(company => company.FoundationDate).GreaterThan(new DateTime(1990, 1, 1)).WithMessage("Foundation date cannot be less than 1990").NotNull().WithMessage("Foundation Date cannot be null");
            RuleFor(company => company.ContractStartDate).GreaterThan(new DateTime(1990,1,1)).WithMessage("Contract start date cannot be less than 1990").NotNull().WithMessage("Contract Start Date cannot be null");
            RuleFor(company => company.ContractEndDate).GreaterThan(c => c.ContractStartDate.Value.AddDays(30)).WithMessage("Contract End Date must be greater than Contract Start Date").NotNull().WithMessage("Contract End Date cannot be null");
        }
    }
}
