using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeWeb.Services
{
    public interface ITransactionsService
    {
        string PayPalEmail { get; }
        string Currency { get; set; }
        string PayPalUrl { get; }

        Task PayPalVerifyAsync(HttpRequest request);
    }
}