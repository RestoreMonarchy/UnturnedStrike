using SDG.Unturned;
using System;
using System.Linq;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Bombsite
{
    public class BombCarrierComponent : MonoBehaviour
    {
        private static UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;
        public TerroristPlayer Player { get; private set; }

        void Awake()
        {
            Player = GetComponent<TerroristPlayer>();            
        }

        void Start()
        {            
            Player.NativePlayer.inventory.onDropItemRequested += OnDropItemRequested;

            foreach (var player in Player.GameService.CurrentRound.TeamPlayersAlive[ETeamType.Terrorist])
                player.Message("BombCarrier", Color.red, Player.PrivateName);

            StartCheckAFK();
        }

        private void StartCheckAFK()
        {
            InvokeRepeating("CheckAFK", 0, 0.1f);
        }

        private Vector3 lastPosition;
        private float lastRotation;

        private int afkTimes = 0;

        private void CheckAFK()
        {
            if (Player.GameService.CurrentRound?.IsFreeze ?? true)
                return;

            if (lastRotation == Player.NativePlayer.transform.rotation.eulerAngles.y && 
                lastPosition == Player.NativePlayer.transform.position)
            {
                if (++afkTimes == 150)
                {
                    var result = Player.NativePlayer.inventory.search(pluginInstance.Configuration.Instance.BombItemId, true, true).FirstOrDefault();
                    if (result != null)
                        Player.NativePlayer.inventory.sendDropItem(result.page, result.jar.x, result.jar.y);
                }
            } else
            {
                lastRotation = Player.NativePlayer.transform.rotation.eulerAngles.y;
                lastPosition = Player.NativePlayer.transform.position;
                afkTimes = 0;
            }
        }

        private void OnDropItemRequested(PlayerInventory inventory, Item item, ref bool shouldAllow)
        {
            if (item.id == pluginInstance.Configuration.Instance.BombItemId)
            {
                foreach (var player in Player.GameService.CurrentRound.TeamPlayersAlive[ETeamType.Terrorist])
                    player.Message("BombDropped", Color.red, Player.DisplayName);

                SendBombDroppedEffect(Player.transform.position);
                Destroy(this);
            }
        }

        void OnDestroy()
        {
            Player.NativePlayer.inventory.onDropItemRequested -= OnDropItemRequested;
        }

        public static void SendBombDroppedEffect(Vector3 position)
        {
            foreach (var player in pluginInstance.TeamsService.TeamPlayers[ETeamType.Terrorist])
            {
                EffectManager.sendEffect(pluginInstance.Configuration.Instance.BombDroppedEffectId, player.TransportConnection, position);
            }
        }

        public static void ClearBombDroppedEffect()
        {
            EffectManager.ClearEffectByID_AllPlayers(pluginInstance.Configuration.Instance.BombDroppedEffectId);
        }
    }
}
