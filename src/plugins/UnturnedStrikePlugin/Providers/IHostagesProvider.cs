using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Providers
{
    public interface IHostagesProvider
    {
        IEnumerable<Hostage> Hostages { get; }
        void ReloadHostages();
        void AddHostage(Hostage hostage);
    }
}
