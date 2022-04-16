using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnturnedStrikeDatabaseProvider.Repositories;
using UnturnedStrikeWeb.Services;

namespace UnturnedStrikeWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService transactionsService;
        private readonly ITransactionsRepository transactionsRepository;

        public TransactionsController(ITransactionsService transactionsService, ITransactionsRepository transactionsRepository)
        {
            this.transactionsService = transactionsService;
            this.transactionsRepository = transactionsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            await transactionsService.PayPalVerifyAsync(Request);
            return Ok();
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetTransactionsAsync()
        {
            return Ok(await transactionsRepository.GetPlayerTransactionsAsync(User.Identity.Name));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get([FromQuery] decimal amount)
        {
            if (amount < 10)
            {
                return BadRequest();
            }

            var dict = new Dictionary<string, string>()
            {
                { "cmd", "_xclick" },
                { "business", transactionsService.PayPalEmail },
                { "item_name", $"Add {amount} {transactionsService.Currency} to balance" },
                { "item_number", "1" },
                { "custom", User.Identity.Name },
                { "amount", amount.ToString() },
                { "currency_code", transactionsService.Currency },
                { "no_shipping", "1" },
                { "no_note", "1" }
            };

            string url = QueryHelpers.AddQueryString(transactionsService.PayPalUrl, dict);
            return Redirect(url);
        }
    }
}
