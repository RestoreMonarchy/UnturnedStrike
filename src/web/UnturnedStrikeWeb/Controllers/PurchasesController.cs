using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnturnedStrikeAPI;
using UnturnedStrikeDatabaseProvider.Repositories;
using UnturnedStrikeWeb.Services;

namespace UnturnedStrikeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchasesRepository purchasesRepository;
        private readonly IPurchasesService purchasesService;

        public PurchasesController(IPurchasesRepository purchasesRepository, IPurchasesService purchasesService)
        {
            this.purchasesRepository = purchasesRepository;
            this.purchasesService = purchasesService;
        }

        [HttpGet("vip")]
        [Authorize]
        public async Task<IActionResult> GetVIPAsync()
        {
            return Ok(await purchasesRepository.GetVIPPurchaseAsync(User.Identity.Name));
        }

        [HttpPost("vip")]
        [Authorize]
        public async Task<IActionResult> PostVIPAsync()
        {
            var purchase = await purchasesRepository.PurchaseVIPAsync(
                new VIPPurchase(User.Identity.Name, purchasesService.VIPPrice));

            switch (purchase.Item1)
            {
                case 0:
                    return Ok(purchase.Item2);
                case 1:
                    return Forbid();
                case 2:
                    return BadRequest("You don't have enough money");
            }

            return BadRequest();
        }
    }
}
