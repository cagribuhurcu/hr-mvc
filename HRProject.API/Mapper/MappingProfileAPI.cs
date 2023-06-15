using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.UI.Areas.SiteManagement.Models;
using HRProject.UI.Mapper;

namespace HRProject.API.Mapper
{
    public class MappingProfileAPI : Profile
    {
        public MappingProfileAPI()
        {
            CreateMap<CompanyManagerEntity, User>().ReverseMap();
        }
    }
}

