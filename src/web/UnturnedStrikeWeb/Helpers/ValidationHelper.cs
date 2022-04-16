using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UnturnedStrikeAPI.Constants;

namespace UnturnedStrikeWeb.Helpers
{
    public class ValidationHelper
    {
        private const int SteamIdStartIndex = 37; 

        public static Task Validate(CookieValidatePrincipalContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            string steamId = context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value.Substring(SteamIdStartIndex);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, steamId));

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            if (configuration.GetSection("Admins").Get<string[]>().Contains(steamId))
                claims.Add(new Claim(ClaimTypes.Role, RoleConstants.AdminRoleId));
            else
                claims.Add(new Claim(ClaimTypes.Role, RoleConstants.GuestRoleId));

            context.ReplacePrincipal(new ClaimsPrincipal(new ClaimsIdentity(claims, "SteamAuth")));
            return Task.CompletedTask;
        }
    }
}
