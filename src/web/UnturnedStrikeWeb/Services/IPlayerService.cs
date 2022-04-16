using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeWeb.Services
{
    public interface IPlayerService
    {
        Task<Player> UpdatePlayerAsync(Player player);
    }
}