namespace API.APIModels.User
{
    public class UserAccountAPIModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public RoleAPIModel Role { get; set; }
    }
}
