using KillFeedPlugin.Models;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Core.Utils;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnturnedStrike.Plugin;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Extensions;
using UnturnedStrike.Plugin.Providers;

namespace KillFeedPlugin
{
    public class KillFeedPlugin : RocketPlugin<KillFeedConfiguration>
    {
        public const short Key = 23425;

        public List<KillFeedItem> KillFeedItems { get; set; }

        public IWeaponsProvider WeaponsProvider => UnturnedStrikePlugin.Instance.WeaponsProvider;

        protected override void Load()
        {
            KillFeedItems = new List<KillFeedItem>();
            U.Events.OnPlayerConnected += OnPlayerConnected;
            PlayerLife.onPlayerDied += OnPlayerDeath;
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            var transportConnection = player.Player.channel.GetOwnerTransportConnection();
            EffectManager.sendUIEffect(Configuration.Instance.KillEffectId, Key, transportConnection, false);
            EffectManager.sendUIEffectVisibility(Key, transportConnection, false, "0", false);
            EffectManager.sendUIEffectVisibility(Key, transportConnection, false, "1", false);
            EffectManager.sendUIEffectVisibility(Key, transportConnection, false, "2", false);
            EffectManager.sendUIEffectVisibility(Key, transportConnection, false, "3", false);
            EffectManager.sendUIEffectVisibility(Key, transportConnection, false, "4", false);
        }

        private void OnPlayerDeath(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator)
        {
            GamePlayer victim = sender.player.GetComponent<GamePlayer>();
            GamePlayer killer = PlayerTool.getPlayer(instigator)?.GetComponent<GamePlayer>() ?? null;

            if (victim == null)
                return;

            if (cause == EDeathCause.CHARGE) 
            {
                AddToKillFeed(VictimKillText(victim, UnicodeToChar(Configuration.Instance.BombUnicode)));
                return;
            }

            if (cause == EDeathCause.VEHICLE)
            {
                AddToKillFeed(VictimKillText(victim, UnicodeToChar(Configuration.Instance.VehicleExplosionUnicode)));
                return;
            }            

            if (cause == EDeathCause.BURNING)
            {
                AddToKillFeed(VictimKillText(victim, UnicodeToChar(Configuration.Instance.BurningKillUnicode)));
                return;
            }

            if (killer != null)
            {
                if (cause == EDeathCause.GUN)
                {
                    var unicode = WeaponsProvider.GetWeaponUnicode(killer.NativePlayer.equipment.itemID);
                    if (unicode == null)
                        unicode = WeaponUnicode(killer.NativePlayer.equipment.itemID);
                    else
                        unicode = UnicodeToChar(unicode);
                    AddToKillFeed(KillText(killer, victim, unicode + HeadshotUnicode(limb)));
                }
                else if (cause == EDeathCause.GRENADE)
                    AddToKillFeed(KillText(killer, victim, WeaponUnicode(254))); // temporary solution for grenade kills, always fragmention grenade
                else if (cause == EDeathCause.PUNCH)
                    AddToKillFeed(KillText(killer, victim, UnicodeToChar(Configuration.Instance.PunchKillUnicode) + HeadshotUnicode(limb)));
                else if (cause == EDeathCause.ROADKILL)
                    AddToKillFeed(KillText(killer, victim, UnicodeToChar(Configuration.Instance.VehicleRoadKillUnicode)));
            }
        }

        private string HeadshotUnicode(ELimb limb)
        {
            if (limb == ELimb.SKULL)
            {
                return UnicodeToChar(Configuration.Instance.HeadshotUnicode);
            }
            return string.Empty;
        }

        [RocketCommand("addtokillfeed", "")]
        public void AddToKillFeedCommand(IRocketPlayer caller, string[] args)
        {
            if (args.Length > 0)
                AddToKillFeed(string.Join(" ", args));
        }

        private void AddToKillFeed(string text)
        {
            var newItem = new KillFeedItem(text);
            KillFeedItems.Insert(0, newItem);

            TaskDispatcher.QueueOnMainThread(() =>
            {
                if (KillFeedItems.IndexOf(newItem) <= 4)
                {                    
                    foreach (var player in UnturnedStrikePlugin.Instance.TeamsService.Players)
                        EffectManager.sendUIEffectVisibility(Key, player.TransportConnection, false, KillFeedItems.IndexOf(newItem).ToString(), false);
                }
                KillFeedItems.Remove(newItem);
            }, Configuration.Instance.ShowDuration);

            foreach (var player in UnturnedStrikePlugin.Instance.TeamsService.Players)
                EffectManager.sendUIEffectVisibility(Key, player.TransportConnection, false, (KillFeedItems.Count - 1).ToString(), true);

            for (int i = 0; i < System.Math.Min(KillFeedItems.Count, 5); i++)
            {
                foreach (var player in UnturnedStrikePlugin.Instance.TeamsService.Players)
                    EffectManager.sendUIEffectText(Key, player.TransportConnection, false, $"Text{i}", KillFeedItems[i].Text);   
            }
        }

        private string GetShortName(string name)
        {
            if (name.Length > Configuration.Instance.MaxNameLength)
                return name.Substring(0, Configuration.Instance.MaxNameLength);
            else
                return name;
        }

        private string KillText(GamePlayer killer, GamePlayer victim, string weaponCharacter) => Translate("KillText",
            killer.TeamType.GetColor(),
            GetShortName(killer.DisplayName),
            weaponCharacter,
            victim.TeamType.GetColor(),
            GetShortName(victim.DisplayName));

        private string VictimKillText(GamePlayer victim, string weaponChar) => Translate("VictimKillText",
            weaponChar,
            victim.TeamType.GetColor(),
            GetShortName(victim.DisplayName));

        private string WeaponUnicode(ushort weaponId)
        {
            var weapon = Configuration.Instance.WeaponUnicodes.FirstOrDefault(x => x.WeaponId == weaponId);
            if (weapon != null)
                return UnicodeToChar(weapon.Unicode);
            else
                return string.Empty;
        }

        private string UnicodeToChar(string unicode) => char.ConvertFromUtf32(int.Parse(unicode, NumberStyles.HexNumber));

        protected override void Unload()
        {
            PlayerLife.onPlayerDied -= OnPlayerDeath;
            U.Events.OnPlayerConnected -= OnPlayerConnected;
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "KillText", "<color={0}>{1}</color> {2} <color={3}>{4}</color>" },
            { "VictimKillText", "{0} <color={1}>{2}</color>" }
        };
    }
}
