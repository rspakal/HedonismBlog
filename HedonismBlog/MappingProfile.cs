using AutoMapper;
using BlogDALLibrary.Entities;
using HedonismBlog.ViewModels;
using ServicesLibrary.Models;

namespace HedonismBlog
{
    public class MappingProfile1 : Profile
    {
        public MappingProfile1()
        {
            //CreateMap<User, UserViewModel>();
            //CreateMap<UserViewModel, User>();
            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
            CreateMap<PostViewModel, Post>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<CommentViewModel, Comment>();
            CreateMap<TagModel, Tag>();
            CreateMap<Tag, TagModel>();
            CreateMap<RoleViewModel, Role>();
            CreateMap<Role, RoleViewModel>();
        }
    }
}
