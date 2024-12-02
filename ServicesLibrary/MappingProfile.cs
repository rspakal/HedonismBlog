using AutoMapper;
using BlogDALLibrary.Entities;
using ServicesLibrary.Models;
using ServicesLibrary.Models.Comment;
using ServicesLibrary.Models.Post;
using ServicesLibrary.Models.User;
namespace ServicesLibrary
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserLoginModel>();
            CreateMap<UserLoginModel, User>();
            CreateMap<User, UserRegistrationModel>();
            CreateMap<UserRegistrationModel, User>();
            CreateMap<User, UserAccountModel>();
            CreateMap<UserAccountModel, User>();
            CreateMap<User, UserPreviewModel>();
            CreateMap<User, UserAssignRoleModel>()
                .ForMember(dest => dest.SelectedRole, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));
            CreateMap<Post, PostPreviewModel>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));
            CreateMap<PostPreviewModel, Post>();
            CreateMap<Post, PostViewModel>();
            CreateMap<PostCreateModel, Post>();
            CreateMap<Post, PostUpdateModel>();
            CreateMap<PostUpdateModel, Post>();
            CreateMap<Role, RoleModel>();
            CreateMap<RoleModel, Role>();
            CreateMap<Tag, TagModel>();
            CreateMap<TagModel, Tag>();
            CreateMap<Comment, CommentModel>();
            CreateMap<CommentModel, Comment>();
        }
    }
}
