using AutoMapper;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace API.APIModels
{
    public class UserAssignRoleAPIModel : UserBaseAPIModel
    {
        public Dictionary<string, bool> Roles { get; set; } = new Dictionary<string, bool>();
    }
}
