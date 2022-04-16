using SDG.Unturned;
using Steamworks;
using System;
using System.Linq;
using UnturnedStrike.Plugin.Bombsite;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Services
{
    public class DefuseRoundService : GlobalRoundService
    {
        public BombComponent BombComponent { get; private set; }
        public BombCarrierComponent Bomber { get; set; }

        public bool IsBombPlanted { get; set; }
        public TerroristPlayer BombPlanter { get; set; }
        public CounterTerroristPlayer BombDefuser { get; set; }

        protected override void Initialize()
        {
            IsBombPlanted = false;
        }

        protected override void Begin()
        {
            var bomber = TeamsService.GetRandomPlayer(ETeamType.Terrorist) as TerroristPlayer;
            Bomber = bomber.MakeBomber();

            ItemManager.onTakeItemRequested += OnTakeItemRequested;
            BarricadeDrop.OnSalvageRequested_Global += OnSalvageRequested;
        }

        protected override void End()
        {
            if (Bomber != null)
                Destroy(Bomber);
            if (IsBombPlanted)
                BombComponent.SelfDestroy();

            BombCarrierComponent.ClearBombDroppedEffect();
            ItemManager.onTakeItemRequested -= OnTakeItemRequested;
            BarricadeDrop.OnSalvageRequested_Global -= OnSalvageRequested;
        }

        private void OnSalvageRequested(BarricadeDrop barricade, SteamPlayer instigatorClient, ref bool shouldAllow)
        {
            if (barricade.model.GetComponent<BombComponent>() == null)
                return;

            var comp = instigatorClient.player.GetComponent<CounterTerroristPlayer>();
            if (comp != null)
            {
                BombDefusedWin(comp);
            }
        }

        private void OnTakeItemRequested(Player player, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow)
        {            
            if (itemData.item.id == pluginInstance.Configuration.Instance.BombItemId)
            {
                var comp = player.GetComponent<TerroristPlayer>();
                if (comp != null)
                {
                    Bomber = comp.gameObject.AddComponent<BombCarrierComponent>();
                    BombCarrierComponent.ClearBombDroppedEffect();
                }                    
                else
                    shouldAllow = false;
            }
        }

        protected override void OnPlayerRemoved(GamePlayer player)
        {
            if (Bomber != null && player.GetComponent<BombCarrierComponent>() == Bomber)
            {
                BombCarrierComponent.SendBombDroppedEffect(player.transform.position);
                Destroy(Bomber);
            }
        }

        protected override void OnTimeElapsed()
        {
            if (!IsBombPlanted)
            {
                StartFinishRound(ETeamType.CounterTerrorist, ERounWinType.Time);
                return;
            }

            IsBombPlanted = false;
            BombComponent.DetonateBomb();
            StartFinishRound(ETeamType.Terrorist, ERounWinType.Bomb);
        }

        protected override bool CanWinByKills(ETeamType team)
        {
            if (team == ETeamType.CounterTerrorist && IsBombPlanted)
                return false;
            return true;
        }

        private void CheckForKillWin()
        {
            foreach (var pair in TeamPlayersAlive)
            {
                if (pair.Value.Count == 0)
                {
                    var winner = TeamPlayersAlive.FirstOrDefault(x => x.Value.Count > 0);
                    if (winner.Key == ETeamType.CounterTerrorist && IsBombPlanted)
                        return;
                    StartFinishRound(winner.Key, ERounWinType.Eliminate);
                }
            }
        }

        public void StartBombPlanted(TerroristPlayer comp, BombComponent bombComponent)
        {
            Destroy(Bomber);
            IsBombPlanted = true;
            TimeLeft = pluginInstance.Configuration.Instance.BombTime;
            ShouldShowTimeLeft = false;

            GameService.TriggerOnTimeUpdated(pluginInstance.Translate("BombPlantedTime"));

            BombPlanter = comp;
            BombComponent = bombComponent;

            BombPlanter.GiveMoney(pluginInstance.Configuration.Instance.MoneyRewardBomb);
            TeamsService.PlayEffectToEachPlayer(pluginInstance.Configuration.Instance.BombPlantedEffectId);

            GameService.StatisticsService.TriggerOnBombPlanted(comp);
        }

        public void BombDefusedWin(CounterTerroristPlayer comp)
        {
            if (!Started)
                return;

            BombComponent.StopBombBeepEffect();
            IsBombPlanted = false;
            BombDefuser = comp;

            BombDefuser.GiveMoney(pluginInstance.Configuration.Instance.MoneyRewardDefuse);
            TeamsService.PayEachTeamPlayer(ETeamType.Terrorist, pluginInstance.Configuration.Instance.MoneyRewardBombPlantLose);
            TeamsService.PlayEffectToEachPlayer(pluginInstance.Configuration.Instance.BombDefusedEffectId);
            GameService.StatisticsService.TriggerOnBombDefused(comp);

            StartFinishRound(ETeamType.CounterTerrorist, ERounWinType.Defuse);
        }
    }
}
