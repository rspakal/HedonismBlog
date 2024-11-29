using AutoMapper;
using BlogDALLibrary.Entities;
using HedonismBlog.ViewModels;

namespace HedonismBlog
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
            CreateMap<PostViewModel, Post>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<CommentViewModel, Comment>();
            CreateMap<TagViewModel, Tag>();
            CreateMap<Tag, TagViewModel>();
            CreateMap<RoleViewModel, Role>();
            CreateMap<Role, RoleViewModel>();
        }
    }
}
