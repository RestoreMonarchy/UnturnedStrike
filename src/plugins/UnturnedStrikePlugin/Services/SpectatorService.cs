using System.Collections.Generic;
using UnityEngine;
using UnturnedStrike.Plugin.Components;

namespace UnturnedStrike.Plugin.Services
{
    public class SpectatorService : MonoBehaviour
    {
        public List<SpectatorPlayer> Spectators { get; } = new();

        public void AddSpectator(SpectatorPlayer spectator)
        {
            Spectators.Add(spectator);
        }

        public void RemoveSpectator(SpectatorPlayer spectator)
        {
            Spectators.Remove(spectator);
        }
    }
}
