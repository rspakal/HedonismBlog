using AutoMapper;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Generic;

namespace API.APIModels.User
{
    public class UserAssignRoleAPIModel
    {
        public string Email { get; set; }
        public Dictionary<string, bool> Roles { get; set; } = new Dictionary<string, bool>();
    }
}
