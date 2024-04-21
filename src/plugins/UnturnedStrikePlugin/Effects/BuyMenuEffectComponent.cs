using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Extensions;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Providers;

namespace UnturnedStrike.Plugin.Effects
{
    public class BuyMenuEffectComponent : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        private IWeaponsProvider WeaponsProvider => pluginInstance.WeaponsProvider;
        private IEnumerable<WeaponModel> Weapons => pluginInstance.WeaponsProvider?.Weapons ?? new WeaponModel[0];
        private string[] Categories => pluginInstance.Configuration.Instance.WeaponCategories;


        public GamePlayer Player { get; set; }
        private ITransportConnection TransportConnection => Player.TransportConnection;

        public const int Key = 2578;
        private int currentCategory = 0;
        private IEnumerable<WeaponModel> currentWeapons;

        public bool IsOpened { get; private set; } = false;

        private IEnumerable<WeaponModel> GetPlayerWeapons()
        {
            return Weapons.Where(x => x.Team == Player.TeamType.GetWeaponTeam() || x.Team == EWeaponTeam.Both);
        }

        private IEnumerable<WeaponModel> GetCategoryWeapons(int categoryNum)
        {
            if (Categories.Length > categoryNum)
                return GetPlayerWeapons().Where(x => x.Category == Categories[categoryNum]);
            else
                return null;
        }

        void Awake()
        {
            Player = GetComponent<GamePlayer>();
        }

        void Start()
        {
            Player.OnPluginKeyTicked += OnPluginKeyTicked;
            Player.OnButtonClicked += OnButtonClicked;
            //Player.GameService.OnFreezeStarted += Open;
            Player.GameService.OnFreezeFinished += Close;
        }

        private void OnPluginKeyTicked(uint simulation, byte key, bool state)
        {
            if (key == 0 && state)
            {
                if (!IsOpened)
                {
                    if (!Player.TryOpenBuyMenu())
                    {
                        Player.Message("BuyOnlyFreeze");
                    }
                }
            }
        }

        public void OnButtonClicked(string buttonName)
        {
            if (buttonName == "csgo_close_button")
            {
                Close();
                return;
            }

            if (!buttonName.StartsWith("csgo_item"))
                return;

            var index = Convert.ToInt32(Regex.Match(buttonName, @"\d+").Value) - 1;

            if (buttonName.EndsWith("category_button"))
            {
                ChangeCategory(index);
            }

            if (buttonName.EndsWith("weapon_button"))
            {
                var weapon = currentWeapons.ElementAtOrDefault(index);

                if (weapon != null)
                {
                    if (weapon.Category == "Grenades" && !InventoryHelper.CanHaveMoreGrenades(Player))
                    {
                        Player.Message("MaxGrenadesCount", pluginInstance.Configuration.Instance.MaxGrenadesCount);
                        return;
                    }

                    if (!Player.TryTakeMoney(weapon.Price))
                    {
                        Player.Message("WeaponCantAfford");
                        return;
                    }

                    WeaponsProvider.GivePlayerWeapon(Player, weapon.Id);
                    Player.SendSoundEffect(pluginInstance.Configuration.Instance.BuyEffectId);
                }
            }
        }

        public void Open()
        {
            IsOpened = true;
            EffectManager.sendUIEffect(pluginInstance.Configuration.Instance.BuyMenuEffectId, Key, TransportConnection, true);
            Player.NativePlayer.enablePluginWidgetFlag(EPluginWidgetFlags.Modal);
            for (int i = 0; i < Categories.Length; i++)
            {
                EffectManager.sendUIEffectText(Key, TransportConnection, true, $"csgo_item_{i + 1}_category", Categories[i]);
            }

            ChangeCategory(0);
        }

        public void Close()
        {
            if (IsOpened)
            {
                Player.NativePlayer.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
                EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.BuyMenuEffectId, TransportConnection);
                IsOpened = false;
            }
        }

        private void ChangeCategory(int categoryNum)
        {
            var weapons = GetCategoryWeapons(categoryNum);
            if (weapons == null)
                return;

            currentCategory = categoryNum;
            currentWeapons = weapons;

            for (int i = 0; i < 6; i++)
            {
                var weapon = weapons.ElementAtOrDefault(i);
                EffectManager.sendUIEffectText(Key, TransportConnection, true, $"csgo_item_{i + 1}_weapon", weapon?.Name ?? string.Empty);
                EffectManager.sendUIEffectText(Key, TransportConnection, true, $"csgo_item_{i + 1}_price", weapon == null ? string.Empty : $"$ {weapon.Price}");
            }            
        }

        void OnDestroy()
        {
            Player.OnButtonClicked -= OnButtonClicked;
            //Player.GameService.OnFreezeStarted -= Open;
            Player.GameService.OnFreezeFinished -= Close;
            Close();
        }
    }
}
