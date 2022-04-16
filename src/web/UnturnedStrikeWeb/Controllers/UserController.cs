using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeDatabaseProvider.Repositories;

namespace UnturnedStrikeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IPlayersRepository playersRepository;

        public UserController(IPlayersRepository playersRepository)
        {
            this.playersRepository = playersRepository;
        }

        [HttpGet]
        public async Task<UserInfo> GetUserAsync()
        {
            UserInfo userInfo = new UserInfo();
            if (User.Identity.IsAuthenticated)
            {
                userInfo.IsAuthenticated = true;
                userInfo.SteamID = User.Identity.Name;
                userInfo.Role = User.FindFirstValue(ClaimTypes.Role);
                userInfo.Player = await playersRepository.GetPlayerAsync(User.Identity.Name);                
            } else
            {
                userInfo.IsAuthenticated = false;
            }

            return userInfo;
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpGet("~/signin"), HttpPost("~/signin")]
        public IActionResult SignIn([FromQuery] string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "Steam");
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpGet("~/signout"), HttpPost("~/signout")]
        public IActionResult SignOut()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
