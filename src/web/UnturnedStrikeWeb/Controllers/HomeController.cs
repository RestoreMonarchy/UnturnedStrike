using Microsoft.AspNetCore.Mvc;

namespace UnturnedStrikeWeb.Controllers
{
    [Controller]
    public class HomeController : ControllerBase
    {
        [HttpGet("discord")]
        public IActionResult Discord()
        {
            return Redirect("https://discord.gg/kGYv7mK");
        }

        [HttpGet("modpack")]
        public IActionResult Modpack()
        {
            return Redirect("https://steamcommunity.com/sharedfiles/filedetails/?id=2106687860");
        }
    }
}
