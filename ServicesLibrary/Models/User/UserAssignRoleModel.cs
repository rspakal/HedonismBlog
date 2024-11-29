using System.Collections.Generic;

namespace ServicesLibrary.Models.User
{
    public class UserAssignRoleModel
    {
        public string Email { get; set; }
        public Dictionary<string, bool> Roles { get; set; } = new Dictionary<string, bool>();
    }
}
