using Rocket.Unturned.Chat;
using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;
using System.Linq;
using UnityEngine;
using UnturnedStrike.Plugin.Helpers;

namespace UnturnedStrike.Plugin.Components
{
    public class UnturnedStrikePlayer : MonoBehaviour, IUnturnedStrikePlayer
    {
        protected UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public Player NativePlayer { get; private set; }
        public string DisplayName { get; private set; }
        public string PrivateName { get; private set; }

        public ITransportConnection TransportConnection => NativePlayer.channel.GetOwnerTransportConnection();

        public CSteamID CSteamID => NativePlayer.channel.owner.playerID.steamID;
        public string Id => CSteamID.m_SteamID.ToString();

        public bool IsVIP => VIPHelper.IsVIP(this);

        public bool IsFreezed { get; private set; }

        public delegate void ButtonClicked(string buttonName);
        public event ButtonClicked OnButtonClicked;

        public void TriggerOnButtonClicked(string buttonName)
        {
            OnButtonClicked?.Invoke(buttonName);
        }

        protected virtual void Awake()
        {
            NativePlayer = GetComponent<Player>();
            DisplayName = NativePlayer.channel.owner.playerID.characterName;
            PrivateName = NativePlayer.channel.owner.playerID.nickName;
            IsFreezed = false;
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        public void ShowWaitingUI()
        {
            EffectManager.sendUIEffect(pluginInstance.Configuration.Instance.WaitingUIEffectId, 2691, TransportConnection, true);
        }

        public void HideWaitingUI()
        {
            EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.WaitingUIEffectId, TransportConnection);
        }

        public void FreezePlayer()
        {
            NativePlayer.movement.sendPluginSpeedMultiplier(0);
            NativePlayer.movement.sendPluginJumpMultiplier(0);
            IsFreezed = true;
            Message("FreezePlayer");
        }

        public void UnfreezePlayer()
        {
            NativePlayer.movement.sendPluginSpeedMultiplier(1);
            NativePlayer.movement.sendPluginJumpMultiplier(1);
            IsFreezed = false;
            Message("UnfreezePlayer");
        }

        public void ForceRespawnPlayer()
        {
            NativePlayer.life.ReceiveRespawnRequest(false);
        }

        public void GiveItem(ushort itemId)
        {
            NativePlayer.inventory.forceAddItem(new Item(itemId, true), true);
        }

        public void RemoveItem(ushort itemId)
        {
            var searches = NativePlayer.inventory.search(itemId, true, true);
            foreach (var search in searches)
            {
                NativePlayer.inventory.removeItem(search.page, NativePlayer.inventory.getIndex(search.page, search.jar.x, search.jar.y));
            }
        }

        public void ClearInventory()
        {
            InventoryHelper.ClearPlayerInventory(NativePlayer);
        }

        public void SelfKill()
        {
            NativePlayer.life.askDamage(101, Vector3.up * 101f, EDeathCause.SUICIDE, ELimb.SKULL, CSteamID, out _);
        }

        public void RestoreHealth()
        {
            NativePlayer.life.askHeal(100, NativePlayer.life.isBleeding, NativePlayer.life.isBroken);
        }

        public void MaxSkills()
        {
            for (int i = 0; i < NativePlayer.skills.skills.Length; i++)
            {
                var skills = NativePlayer.skills.skills[i];
                for (int j = 0; j < skills.Length; j++)
                {
                    var skill = skills[j];
                    NativePlayer.skills.ServerSetSkillLevel(i, j, skill.max);
                }
            }
        }

        public void Message(string key, params object[] args)
        {
            UnturnedChat.Say(CSteamID, pluginInstance.Translate(key, args));
        }

        public void Message(string key, Color color, params object[] args)
        {
            UnturnedChat.Say(CSteamID, pluginInstance.Translate(key, args), color);
        }

        public void SendSoundEffect(ushort effectId)
        {
            EffectManager.sendEffect(effectId, TransportConnection, transform.position);
        }
    }
}
