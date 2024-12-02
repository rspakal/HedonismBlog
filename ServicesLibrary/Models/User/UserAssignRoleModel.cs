using System.Collections.Generic;

namespace ServicesLibrary.Models.User
{
    public class UserAssignRoleModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string SelectedRole { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
