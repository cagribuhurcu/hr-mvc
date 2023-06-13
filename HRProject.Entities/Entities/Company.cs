using HRProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Entities
{
    public class Company : BaseEntity
    {

        public Company()
        {
            companyManagers= new List<CompanyManagerEntity>();    
        }
        public string? CompanyName { get; set; } //,boş geçilemez

        public Titles? Title { get; set; } //boş geçilemez

        public string? MERSISNo { get; set; }
        public string? TaxNumber { get; set; }
        public string? TaxAdministration { get; set; }

        public string? LogoURL { get; set; }

        public string? PhoneNumber { get; set; } //10 hane, ,boş geçilemez

        public string? Address { get; set; } //,boş geçilemez

        public string? EmailAddress { get; set; }

        public int? TotalEmployees { get; set; } //0'dan küçük olamaz,boş geçilemez

        public DateTime? FoundationDate { get; set; } // validation: günümüz tarihinden büyük olamaz,boş geçilemez

        public DateTime? ContractStartDate { get; set; } // validation: günümüz tarihinden büyük olamaz ,boş geçilemez

        public DateTime? ContractEndDate { get; set; }

        public List<CompanyManagerEntity> companyManagers { get; set; }
    }
}
