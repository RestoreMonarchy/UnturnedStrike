using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnturnedStrike.Plugin.Components
{
    public interface IUnturnedStrikePlayer
    {
        Player NativePlayer { get; }
        CSteamID CSteamID { get; }
        bool IsVIP { get; }
        bool IsFreezed { get; }
        void TriggerOnButtonClicked(string buttonName);
    }
}
