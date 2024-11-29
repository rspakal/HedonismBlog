namespace ServicesLibrary.Models.User
{
    public class UserAccountModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public RoleModel Role { get; set; }
    }
}
