using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HedonismBlog.Controllers
{
    public class BlogMVCBaseController : Controller
    {
        protected string GetClaimValue(string claimType)
        {
            return HttpContext.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
    }
}
