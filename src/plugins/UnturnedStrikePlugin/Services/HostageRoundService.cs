using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Extensions;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Utilities;
using Logger = Rocket.Core.Logging.Logger;

namespace UnturnedStrike.Plugin.Services
{
    public class HostageRoundService : GlobalRoundService
    {
        public List<GamePlayer> HostageRescuers { get; private set; }
        public bool IsBeingRescued { get; private set; }

        protected override void Initialize()
        {
            HostageRescuers = new List<GamePlayer>();
            IsBeingRescued = false;
        }

        protected override void Begin()
        {
            foreach (var hostage in pluginInstance.HostagesProvider.Hostages.Take(2))
            {
                var transform = DropHostage(hostage.Position.ToVector3(), hostage.Angles.ToVector3());
            }

            BarricadeManager.onDamageBarricadeRequested += OnDamageBarricadeReqeusted;
            BarricadeDrop.OnSalvageRequested_Global += OnSalvageRequested;
            UnturnedPlayerEvents.OnPlayerUpdatePosition += OnPlayerUpdatePosition;
        }

        protected override void End()
        {
            BarricadeManager.onDamageBarricadeRequested -= OnDamageBarricadeReqeusted;
            BarricadeDrop.OnSalvageRequested_Global -= OnSalvageRequested;
            UnturnedPlayerEvents.OnPlayerUpdatePosition -= OnPlayerUpdatePosition;
        }

        private void OnSalvageRequested(BarricadeDrop barricade, SteamPlayer instigatorClient, ref bool shouldAllow)
        {
            CounterTerroristPlayer player = instigatorClient.player.GetComponent<CounterTerroristPlayer>() ?? null;
            if (player != null && !HostageRescuers.Contains(player))
            {
                BarricadeUtility.DestroyBarricade(barricade);
                TakeHostage(player);
            }
            else
            {
                UnturnedChat.Say(instigatorClient.playerID.steamID, pluginInstance.Translate("AlreadyCarryingHostage"));
            }
            shouldAllow = false;
        }

        protected override bool CanWinByKills(ETeamType team)
        {
            return true;
        }

        protected override void OnPlayerRemoved(GamePlayer player)
        {
            if (HostageRescuers.Contains(player))
            {
                DropHostage(player);
            }            
        }

        protected override void OnTimeElapsed()
        {
            StartFinishRound(ETeamType.Terrorist, ERounWinType.Time);
        }

        private void OnDamageBarricadeReqeusted(CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            BarricadeDrop drop = BarricadeManager.FindBarricadeByRootTransform(barricadeTransform);
            if (drop == null)
                return;

            if (drop.asset.id == pluginInstance.Configuration.Instance.HostageBarricadeId)
            {
                shouldAllow = false;
            }
        }

        private void OnPlayerUpdatePosition(UnturnedPlayer player, Vector3 position)
        {
            if (HostageRescuers != null && HostageRescuers.Exists(x => x.Id == player.Id))
            {
                if (RegionsHelper.IsPointOnRegion(position) && !WasFinished)
                {
                    var rescuer = player.GetComponent<GamePlayer>();
                    DropHostage(rescuer);
                    GameService.StatisticsService.TriggerOnHostageRescued(rescuer);
                    StartFinishRound(ETeamType.CounterTerrorist, ERounWinType.Rescue);
                }
            }
        }

        public Transform DropHostage(GamePlayer player)
        {
            player.NativePlayer.clothing.ForceRemoveBackpack();
            HostageRescuers.Remove(player);

            var rot = new Vector3(135, 135, 0);
            var pos = player.transform.position;
            pos.y = RegionsHelper.GetRegionHeight(pos);

            return DropHostage(pos, rot);
        }

        public Transform DropHostage(Vector3 pos, Vector3 angles)
        {
            return BarricadeUtility.DropBarricade(pluginInstance.Configuration.Instance.HostageBarricadeId, pos, angles,
                pluginInstance.Configuration.Instance.CounterTerroristGroupId);
        }

        private void TakeHostage(GamePlayer rescuer)
        {
            HostageRescuers.Add(rescuer);
            rescuer.GiveItem(pluginInstance.Configuration.Instance.HostageBackpackId);

            if (!IsBeingRescued)
            {
                TeamsService.PlayEffectToEachPlayer(pluginInstance.Configuration.Instance.HostageRescuedEffectId);
                IsBeingRescued = true;
                if ((TimeLeft += pluginInstance.Configuration.Instance.HostageTime) > pluginInstance.Configuration.Instance.RoundDuration)
                    TimeLeft = pluginInstance.Configuration.Instance.RoundDuration;
            }
        }
    }
}
