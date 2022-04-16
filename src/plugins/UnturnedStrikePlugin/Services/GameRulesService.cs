using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Extensions;
using UnturnedStrike.Plugin.Helpers;

namespace UnturnedStrike.Plugin.Services
{
    public class GameRulesService : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        void Start()
        {
            DamageTool.damagePlayerRequested += DamagePlayerRequested;
            ItemManager.onTakeItemRequested += OnTakeItemRequested;
            ItemManager.onServerSpawningItemDrop += OnServerSpawningItemDrop;
            PlayerVoice.onRelayVoice += OnRelayVoice;
        }

        void OnDestroy()
        {
            DamageTool.damagePlayerRequested -= DamagePlayerRequested;
            ItemManager.onTakeItemRequested -= OnTakeItemRequested;
            ItemManager.onServerSpawningItemDrop -= OnServerSpawningItemDrop;
            PlayerVoice.onRelayVoice -= OnRelayVoice;
        }

        private void OnRelayVoice(PlayerVoice speaker, bool wantsToUseWalkieTalkie, ref bool shouldAllow, ref bool shouldBroadcastOverRadio, ref PlayerVoice.RelayVoiceCullingHandler cullingHandler)
        {
            var player = speaker.GetComponent<GamePlayer>();
            if (player != null && player.IsAlive)
                cullingHandler = new PlayerVoice.RelayVoiceCullingHandler((speaker2, listener) => TeamVoice(player, listener));
        }

        private bool TeamVoice(GamePlayer speaker, PlayerVoice listener)
        {
            var player = listener.GetComponent<GamePlayer>();
            if (player != null && player.TeamType == speaker.TeamType && player.IsAlive)
                return true;
            return false;
        }

        private void OnServerSpawningItemDrop(Item item, ref Vector3 location, ref bool shouldAllow)
        {
            if ((Assets.find(EAssetType.ITEM, item.id) as ItemClothingAsset) != null)
            {
                shouldAllow = false;
            }
        }

        private void OnTakeItemRequested(Player player, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow)
        {
            var comp = player.GetComponent<GamePlayer>();

            if (comp != null && itemData.item.IsThrowable() && !InventoryHelper.CanHaveMoreGrenades(comp))
            {
                comp.Message("MaxGrenadesCount", pluginInstance.Configuration.Instance.MaxGrenadesCount);
                shouldAllow = false;
            }
        }

        private void DamagePlayerRequested(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
            // Disable damage for players in lobby
            var comp = parameters.player.GetComponent<GamePlayer>();
            if (comp == null || (!comp.IsAlive && comp.GameService.IsGameStarted))
            {
                shouldAllow = false;
                return;
            }

            // Disable friendly fire from grenades
            if (parameters.cause == EDeathCause.GRENADE)
            {
                var killerPlayer = PlayerTool.getPlayer(parameters.killer)?.GetComponent<GamePlayer>() ?? null;
                if (killerPlayer.TeamType == comp.TeamType)
                {
                    shouldAllow = false;
                    return;
                }
            }

            // Disable damage while freezed
            if (comp.IsFreezed)
            {
                shouldAllow = false;
            }
        }
    }
}
