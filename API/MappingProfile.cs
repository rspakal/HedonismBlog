using API.APIModels;
using AutoMapper;
using BlogDALLibrary.Models;
using BlogDALLibrary.Repositories;
using System.Collections.Generic;
namespace API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserLoginAPIModel>();
            CreateMap<UserLoginAPIModel, User>();
            CreateMap<User, UserRegistrationAPIModel>();
            CreateMap<UserRegistrationAPIModel, User>();
            CreateMap<User, UserInfoAPIModel>();
            CreateMap<UserInfoAPIModel, User>();
            CreateMap<User, UserAssignRoleAPIModel>()
                .AfterMap((src, dest) =>
                {
                    if (src.Role != null && !string.IsNullOrEmpty(src.Role.Name))
                    {
                        dest.Roles.Add(src.Role.Name, true);
                    }
                });
            CreateMap<Role, RoleAPIModel>();
            CreateMap<RoleAPIModel, Role>();
            CreateMap<Tag, TagAPIModel>();
            CreateMap<TagAPIModel, Tag>();
        }
    }
}
