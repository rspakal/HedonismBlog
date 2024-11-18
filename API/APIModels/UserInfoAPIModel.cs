namespace API.APIModels
{
    public class UserInfoAPIModel : UserBaseAPIModel
    {
        public string Password { get; set; }
        public RoleAPIModel Role { get; set; }
    }
}
