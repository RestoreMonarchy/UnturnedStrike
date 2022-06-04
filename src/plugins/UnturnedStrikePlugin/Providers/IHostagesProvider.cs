using System.Collections.Generic;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Providers
{
    public interface IHostagesProvider
    {
        IEnumerable<Hostage> Hostages { get; }
        void ReloadHostages();
        void AddHostage(Hostage hostage);
        bool RemoveHostage(int hostageId);
    }
}
