using AutoMapper;
using CPK.Sso.Models;
using CPK.Sso.Models.ManageViewModels;

namespace CPK.Sso.Configuration.AutoMapper
{
    public class UserManagementAutoMapperProfile : Profile
    {
        public UserManagementAutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>()
                .ReverseMap();
            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(dst => dst.UserId, src => src.MapFrom(s => s.Id));
        }
    }
}