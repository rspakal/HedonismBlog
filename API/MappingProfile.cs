﻿using API.APIModels;
using API.APIModels.Comment;
using API.APIModels.Post;
using API.APIModels.User;
using AutoMapper;
using BlogDALLibrary.Entities;
using BlogDALLibrary.Repositories;
using System.Collections.Generic;
namespace API
{
    public class MappingProfile1 : Profile
    {
        public MappingProfile1()
        {
            CreateMap<User, UserLoginAPIModel>();
            CreateMap<UserLoginAPIModel, User>();
            CreateMap<User, UserRegistrationAPIModel>();
            CreateMap<UserRegistrationAPIModel, User>();
            CreateMap<User, UserAccountAPIModel>();
            CreateMap<UserAccountAPIModel, User>();
            CreateMap<User, UserAssignRoleAPIModel>()
                .AfterMap((src, dest) =>
                {
                    if (src.Role != null && !string.IsNullOrEmpty(src.Role.Name))
                    {
                        dest.Roles.Add(src.Role.Name, true);
                    }
                });
            CreateMap<Post, PostPreviewAPIModel>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));
            CreateMap<PostPreviewAPIModel, Post>();
            CreateMap<Role, RoleAPIModel>();
            CreateMap<RoleAPIModel, Role>();
            CreateMap<Tag, TagAPIModel>();
            CreateMap<TagAPIModel, Tag>();
            CreateMap<Comment, CommentAPIModel>();
            CreateMap<CommentAPIModel, Comment>();
        }
    }
}
