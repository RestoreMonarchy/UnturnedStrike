using HarmonyLib;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using SDG.Unturned;
using System;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Providers;
using UnturnedStrike.Plugin.Services;
using Logger = Rocket.Core.Logging.Logger;

namespace UnturnedStrike.Plugin
{
    public class UnturnedStrikePlugin : RocketPlugin<UnturnedStrikeConfiguration>
    {
        public static UnturnedStrikePlugin Instance { get; private set; }

        public const string HarmonyInstanceId = "com.unturnedstrike.unturnedstrikeplugin";
        private Harmony HarmonyInstance { get; set; }

        public EGameType GameType => (EGameType)Enum.Parse(typeof(EGameType), Configuration.Instance.GameType);
        public string MapName => Provider.map.Replace(' ', '_');

        public TeamsService TeamsService => GameService?.TeamsService ?? null;
        public GameService GameService { get; private set; }
        public LobbyService LobbyService { get; private set; }
        public GameRulesService GameRulesService { get; private set; }
        public SpectatorService SpectatorService { get; private set; }

        public GameObject UnturnedStrikeGameObject { get; set; }

        public IWeaponsProvider WeaponsProvider { get; set; }
        public IHostagesProvider HostagesProvider { get; set; }

        public delegate void UnturnedStrikePluginLoaded();
        public static event UnturnedStrikePluginLoaded OnUnturnedStrikeLoaded;

        protected override void Load()
        {
            Instance = this;
            HarmonyInstance = new Harmony(HarmonyInstanceId);

            HostagesProvider = new JsonHostagesProvider();
            HostagesProvider.ReloadHostages();

            gameObject.AddComponent<SpawnsHelper>();
            gameObject.AddComponent<TeamsHelper>();
            gameObject.AddComponent<LoadoutsHelper>();
            gameObject.AddComponent<RegionsHelper>();
            gameObject.AddComponent<VIPHelper>();

            R.Plugins.OnPluginsLoaded += OnPluginsLoaded;

            if (!Configuration.Instance.IsSetUpRun)
            {                
                HarmonyInstance.PatchAll();
                LobbyService = gameObject.AddComponent<LobbyService>();
                GameRulesService = gameObject.AddComponent<GameRulesService>();
                SpectatorService = gameObject.AddComponent<SpectatorService>();
                RestartGame();
                EffectManager.onEffectButtonClicked += OnEffectButtonClicked;
                PlayerInput.onPluginKeyTick += OnPluginKeyTick;

                OnUnturnedStrikeLoaded?.Invoke();
            }            

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
        }

        private void OnPluginsLoaded()
        {
            if (WeaponsProvider != null)
            {
                try
                {
                    WeaponsProvider.ReloadWeaponsData();
                }
                catch (Exception e)
                {
                    Logger.LogException(e, "An exception occurated when reloading custom WeaponsProvider, defaulting to JsonWeaponsProvider...");
                    WeaponsProvider = null;
                }
            }

            if (WeaponsProvider == null)
            {
                WeaponsProvider = new JsonWeaponsProvider();
                WeaponsProvider.ReloadWeaponsData();
            }
        }

        private void OnPluginKeyTick(Player player, uint simulation, byte key, bool state)
        {
            if (state)
            {
                Logger.Log($"{player.channel.owner.playerID.playerName} key: {key} | state: {state}");
            }            
            var comp = player.GetComponent<UnturnedStrikePlayer>();
            if (comp != null)
            {
                if (state)
                {
                    Logger.Log($"component not null");
                }
                
                comp.TriggerOnPluginKeyTick(simulation, key, state);
            }
        }

        private void OnEffectButtonClicked(Player player, string buttonName)
        {
            var comp = player.GetComponent<UnturnedStrikePlayer>();
            if (comp != null)
                comp.TriggerOnButtonClicked(buttonName);
        }

        public void RestartGame()
        {
            if (UnturnedStrikeGameObject != null)
                Destroy(UnturnedStrikeGameObject);
            UnturnedStrikeGameObject = new GameObject("UnturnedStrikeGame", typeof(GameService));
            DontDestroyOnLoad(UnturnedStrikeGameObject);
            GameService = UnturnedStrikeGameObject.GetComponent<GameService>();
        }
        
        protected override void Unload()
        {
            R.Plugins.OnPluginsLoaded -= OnPluginsLoaded;

            if (!Configuration.Instance.IsSetUpRun)
            {
                EffectManager.onEffectButtonClicked -= OnEffectButtonClicked;
                PlayerInput.onPluginKeyTick -= OnPluginKeyTick;
            }   
            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "AddWeaponInvalid", "You must specify price!" },
            { "AddWeaponSuccess", "Successfully added new weapon {0} {1} with {2} items!" },
            { "RemoveWeaponInvalid", "You must specify weapon ID!" },
            { "RemoveWeaponFail", "Failed to find weapon with {0} ID!" },
            { "RemoveWeaponSuccess", "Successfully removed {0} weapon!" },
            { "ReloadWeaponsSuccess", "Successfully reloaded weapons!" },
            { "SetNodeSuccess", "Successfully set node at your position: {0}" },
            { "MakeRegionInvalid", "You must specify name and valid height!" },
            { "MakeRegionSuccess", "Successfully created bombsite {0}!" },
            { "TeamsDisplay", "Terrorists {0}{1} Counter-Terrorists {2}{3}" },
            { "SetSpawnInvalid", "You must specify spawn type: Lobby, Terrorist or CounterTerrorist" },
            { "SetSpawnSuccess", "Successfully set {0} spawn at pos: {1} rot: {2}" },
            { "GameWaitingWarn", "Game will begin when there will be at least 1 player in each team!" },
            { "GameBeginCountdown", "Game will start in {0} seconds!" },
            { "GameBegin", "Game started!" },
            { "FreezePlayer", "You are freezed until round starts!" },
            { "UnfreezePlayer", "The round started, you are unfreezed!" },
            { "GameStarting", "The new game is getting started!" },
            { "TT", "T" },
            { "CT", "CT" },
            { "Terrorist", "Terrorists" },
            { "CounterTerrorist", "Counter Terrorists" },
            { "RoundWinTitle", "{0} win the round!" },
            { "BombTime", "BOMB {0}" },
            { "NotBombsite", "You can only plant the bomb on bombsite!" },
            { "WinTime", "The time expired!" },
            { "WinBomb", "Bomb exploded!" },
            { "WinDefuse", "Bomb got defused!" },
            { "WinEliminate", "Whole enemy team eliminated!" },
            { "Balance", "$ {0}" },
            { "WeaponCantAfford", "You cannot afford buying this gun!" },
            { "MaxGrenadesCount", "You cannot have more than {0} grenades!" },
            { "BuyOnlyFreeze", "You may open buy menu only while freezed!" },
            { "TerroristPrefix", "[TT] " },
            { "CounterTerroristPrefix", "[CT] " },
            { "GameWinEmpty", "Game won by {0}, because {1} surrended!" },
            { "GameWin", "Game won by {0}!" },
            { "GameDraw", "Game draw!" },
            { "GameRestart", "Game restart!" },
            { "TeamFull", "{0} team is full!" },
            { "TeamAlready", "You are already in team!" },
            { "ChangeTeamInfo", "Use /changeteam command to change your team" },
            { "Discord", "Official Unturned Strike Discord guild" },
            { "MVPEliminations", "MVP: {0} for most eliminations!" },
            { "MVPBombPlant", "MVP: {0} for planting the bomb!" },
            { "MVPBombDefuse", "MVP: {0} for defusing the bomb!" },
            { "BombCarrier", "{0} got the bomb!" },
            { "BombDropped", "{0} dropped the bomb!" },
            { "VIPBonus", "Your team received bonus ${0} money thanks to {1}" },
            { "ChangeTeamFail", "You are not in any team yet! Use /team instead" },
            { "ChangeTeamFull", "You cannot change your team because opposite team is full" },
            { "ChangeTeamSuccess", "Succesfully moved you to {0} team!" },
            { "KillerHealth", "{0} killed you, have {1} health left" },
            { "GiveMoney", "Received +${0} money" },
            { "GameRestartFail", "There isnt any game running!" },
            { "AddWeaponCommandDisabled", "addweapon command can only execute for JsonWeaponsProvider!" },
            { "RemoveWeaponCommandDisabled", "removeweapon command can only execute for JsonWeaponsProvider!" },
            { "AddHostageFail", "You are not looking at any hostage barricade!" },
            { "AddHostageSuccessfull", "Successfully added new hostage ID {0}!" },
            { "AlreadyCarryingHostage", "You are already carrying a hostage!" },
            { "MVPHostageRescued", "MVP: {0} for rescuing a hostage!" },
            { "WarmupTitle", "WARMUP {0}" },
            { "WaitingForPlayersTitle", "WAITING FOR PLAYERS..." },
            { "BombPlantedTime", "" },
            { "RemoveHostageInvalid", "The specified hostage ID is not valid" },
            { "RemoveHostageSuccess", "Successfully removed hostage with ID {0}!" },
            { "RemoveHostageFail", "Failed to find any hostage with ID {0}" },
            { "SpectatorModeOff", "Spectator mode disabled" },
            { "SpectatorModeOn", "Spectator mode enabled" },
            { "SpectatorFail", "You must not be in any team to go in the spectator mode." }
        };
    }
}
