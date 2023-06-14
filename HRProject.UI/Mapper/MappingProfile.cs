using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.UI.Areas.SiteManagement.Models;

namespace HRProject.UI.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SiteManager, SiteManagerVM>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName}{src.MiddleName}{src.LastName}{src.SecondLastName}"));

            CreateMap<SiteManager, UpdateSiteManagerVM>().ReverseMap();
            CreateMap<Company, Company>().ReverseMap();
            CreateMap<CompanyManagerEntity, CompanyManagerEntity>().ReverseMap();
        }
    }
}
