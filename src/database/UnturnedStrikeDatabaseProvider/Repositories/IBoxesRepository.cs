using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories
{
    public interface IBoxesRepository : IRepository
    {
        Task<Box> AddBoxAsync(Box box);
        Task<IEnumerable<Box>> GetBoxesAsync();
        Task<IEnumerable<Box>> GetEnabledBoxesAsync();
        Task<Box> GetWeaponSkinBoxAsync(int boxId);
        Task<IEnumerable<Box>> GetWeaponSkinBoxesAsync();
        Task UpdateBoxAsync(Box box);
    }
}
