using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Effects
{
    public class RoundsEffectComponent : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public GamePlayer Player { get; set; }
        private ITransportConnection CSteamID => Player.TransportConnection;

        public const int Key = 2575;

        void Awake()
        {
            Player = GetComponent<GamePlayer>();
        }

        void Start()
        {
            EffectManager.sendUIEffect(pluginInstance.Configuration.Instance.RoundsEffectId, Key, CSteamID, true);

            EffectManager.sendUIEffectText(Key, CSteamID, true, "csgo_text_teama", pluginInstance.Translate("TT"));
            EffectManager.sendUIEffectText(Key, CSteamID, true, "csgo_text_teamb", pluginInstance.Translate("CT"));

            UpdateAlive(ETeamType.Terrorist, pluginInstance.GameService.CurrentRound?.TeamPlayersAlive[ETeamType.Terrorist]?.Count ?? 0);
            UpdateAlive(ETeamType.CounterTerrorist, pluginInstance.GameService.CurrentRound?.TeamPlayersAlive[ETeamType.CounterTerrorist]?.Count ?? 0);
            UpdateScore(ETeamType.Terrorist, pluginInstance.GameService.TeamScores[ETeamType.Terrorist]);
            UpdateScore(ETeamType.CounterTerrorist, pluginInstance.GameService.TeamScores[ETeamType.CounterTerrorist]);
            UpdateTime("0");

            Player.GameService.OnTimeUpdated += UpdateTime;
            Player.GameService.OnAliveUpdated += UpdateAlive;
            Player.GameService.OnScoreUpdated += UpdateScore;
        }

        public void UpdateScore(ETeamType team, int newScore)
        {
            var childName = team == ETeamType.Terrorist ? "csgo_text_counta" : "csgo_text_countb";
            EffectManager.sendUIEffectText(Key, CSteamID, true, childName, newScore.ToString());
        }

        public void UpdateTime(string time)
        {
            EffectManager.sendUIEffectText(Key, CSteamID, true, "csgo_text_time", time);
        }

        public void UpdateAlive(ETeamType team, int newAlive)
        {
            var childName = team == ETeamType.Terrorist ? "csgo_text_alivea" : "csgo_text_aliveb";
            EffectManager.sendUIEffectText(Key, CSteamID, true, childName, newAlive.ToString());
        }

        void OnDestroy()
        {
            Player.GameService.OnTimeUpdated -= UpdateTime;
            Player.GameService.OnAliveUpdated -= UpdateAlive;
            Player.GameService.OnScoreUpdated -= UpdateScore;
            EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.RoundsEffectId, CSteamID);
        }
    }
}
