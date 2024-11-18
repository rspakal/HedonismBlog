using BlogDALLibrary.Models;

namespace API.APIModels
{
    public class UserRegistrationAPIModel : UserBaseAPIModel
    {
        public string Password { get; set; }
    }
}
