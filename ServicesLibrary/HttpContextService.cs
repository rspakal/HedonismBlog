using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    internal static class HttpContextExtensions
    {
        internal enum ClaimTypes
        {
            Email,
            Role
        }
        internal static string GetCurrentUserClaimValue(this HttpContext context, ClaimTypes claimType)
        {
            return context.User?.FindFirst(claimType.ToString())?.Value;
        }

    }
}
