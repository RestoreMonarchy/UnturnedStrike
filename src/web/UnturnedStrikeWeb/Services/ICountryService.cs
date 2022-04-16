using System.Threading.Tasks;

namespace UnturnedStrikeWeb.Services
{
    public interface ICountryService
    {
        Task<string> GetCountryAsync(string ipAddress);
    }
}