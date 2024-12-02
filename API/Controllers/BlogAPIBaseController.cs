using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public abstract class BlogAPIBaseController : ControllerBase
    {
        protected string GetClaimValue(string claimType)
        {
            return HttpContext.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
    }
}
