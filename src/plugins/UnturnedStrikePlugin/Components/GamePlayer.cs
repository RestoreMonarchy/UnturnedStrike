using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using UnityEngine;
using UnturnedStrike.Plugin.Effects;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Services;

namespace UnturnedStrike.Plugin.Components
{
    public class GamePlayer : UnturnedStrikePlayer
    {
        public GameService GameService { get; private set; }

        public ETeamType TeamType { get; internal set; }
        public bool IsAlive { get; set; }
        public GamePlayer LastKiller { get; set; }

        public int Balance { get; private set; }
        private BuyMenuEffectComponent BuyMenuComponent { get; set; }
        public LeaderboardEffectComponent LeaderboardComponent { get; set; }
        public GameWinEffectComponent GameWinComponent { get; set; }
        public WarmupEffectComponent WarmupComponent { get; set; }

        public bool CanToggleCosmetics => !IsAlive && GameService.IsGameStarted;

        public delegate void BalanceUpdated();
        public event BalanceUpdated OnBalanceUpdated;

        public delegate void PluginKeyTicked(uint simulation, byte key, bool state);
        public event PluginKeyTicked OnPluginKeyTicked;

        internal void TriggerOnPluginKeyTick(uint simulation, byte key, bool state)
        {
            OnPluginKeyTicked?.Invoke(simulation, key, state);
        }

        protected override void Awake()
        {
            base.Awake();
            GameService = pluginInstance.GameService;
            Balance = pluginInstance.Configuration.Instance.MoneyStart;
        }

        protected override void Start()
        {
            base.Start();

            gameObject.AddComponent<RoundsEffectComponent>();
            gameObject.AddComponent<RoundWinEffectComponent>();
            gameObject.AddComponent<BalanceEffectComponent>();
            
            WarmupComponent = gameObject.AddComponent<WarmupEffectComponent>();
            GameWinComponent = gameObject.AddComponent<GameWinEffectComponent>();
            BuyMenuComponent = gameObject.AddComponent<BuyMenuEffectComponent>();
            LeaderboardComponent = gameObject.AddComponent<LeaderboardEffectComponent>();

            NativePlayer.inventory.onDropItemRequested += OnDropItemRequested;
            
            TeamsHelper.JoinTeam(NativePlayer, TeamType);
        }

        private void OnDropItemRequested(PlayerInventory inventory, Item item, ref bool shouldAllow)
        {
            ItemAsset asset;
            if ((asset = Assets.find(EAssetType.ITEM, item.id) as ItemAsset) != null)
            {
                if ((asset as ItemClothingAsset) != null)
                {
                    shouldAllow = false;
                }
            }
        }

        public void GiveMoney(int amount)
        {
            if (amount <= 0)
                return;

            if (Balance == pluginInstance.Configuration.Instance.MoneyLimit)
                return;

            if (Balance + amount >= pluginInstance.Configuration.Instance.MoneyLimit)
                Balance = pluginInstance.Configuration.Instance.MoneyLimit;
            else
                Balance += amount;
            Message("GiveMoney", Color.gray, amount);
            OnBalanceUpdated?.Invoke();
        }

        public bool TryTakeMoney(int amount)
        {
            if (Balance - amount < 0)
                return false;
            Balance -= amount;
            OnBalanceUpdated?.Invoke();
            return true;
        }

        public bool TryOpenBuyMenu()
        {
            if (!GameService?.CurrentRound?.IsFreeze ?? true)
                return false;

            BuyMenuComponent.Open();
            return true;
        }

        public void DisableCosmetics()
        {
            if (NativePlayer.clothing.isVisual)
            {
                NativePlayer.clothing.ServerSetVisualToggleState(EVisualToggleType.COSMETIC, false);
            }
        }

        public void EnableCosmetics()
        {
            if (!NativePlayer.clothing.isVisual)
            {
                NativePlayer.clothing.ServerSetVisualToggleState(EVisualToggleType.COSMETIC, true);
            }
        }

        public void MakeAlive()
        {
            HideWaitingUI();
            if (!IsAlive)     
                ClearInventory();

            ForceRespawnPlayer();

            DisableCosmetics();
            RestoreHealth();

            IsAlive = true;
        }

        public void MakeDead(Player killer)
        {
            IsAlive = false;
            EnableCosmetics();              

            if (killer != null && (LastKiller = killer.GetComponent<GamePlayer>()) != null)
            {
                Message("KillerHealth", Color.magenta, LastKiller.DisplayName, LastKiller.NativePlayer.life.health);
            }
            ShowWaitingUI();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            Destroy(GameWinComponent);
            Destroy(LeaderboardComponent);
            Destroy(BuyMenuComponent);
            Destroy(WarmupComponent);

            Destroy(GetComponent<RoundWinEffectComponent>());
            Destroy(GetComponent<RoundsEffectComponent>());
            Destroy(GetComponent<BalanceEffectComponent>());

            NativePlayer.inventory.onDropItemRequested -= OnDropItemRequested;            
            IsAlive = false;

            pluginInstance.TeamsService.RemovePlayer(this);
        }
    }
}
