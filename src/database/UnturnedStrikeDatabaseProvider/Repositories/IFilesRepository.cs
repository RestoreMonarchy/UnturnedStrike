using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories
{
    public interface IFilesRepository
    {
        Task<File> GetFileAsync(int fileId);
        Task<int> AddFileAsync(File file);
    }
}
