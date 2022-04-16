using Rocket.Core.Logging;
using SDG.Unturned;
using System;
using System.Linq;
using UnturnedStrike.Plugin.Bombsite;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Components
{
    public class TerroristPlayer : GamePlayer
    {
        protected override void Awake()
        {
            TeamType = ETeamType.Terrorist;
            base.Awake();
        }

        public BombCarrierComponent MakeBomber()
        {
            NativePlayer.inventory.forceAddItem(new Item(pluginInstance.Configuration.Instance.BombItemId, true), true);
            try
            {
                var bombSearch = NativePlayer.inventory.search(pluginInstance.Configuration.Instance.BombItemId, true, true).FirstOrDefault();
                if (bombSearch != null)
                {
                    NativePlayer.equipment.equip(bombSearch.page, bombSearch.jar.y, bombSearch.jar.y);
                }
            } catch (Exception e)
            {
                Logger.LogException(e, $"An exception occurated while trying to force player {DisplayName} equip the bomb!");
            }
            
            return gameObject.AddComponent<BombCarrierComponent>();
        }
    }
}
