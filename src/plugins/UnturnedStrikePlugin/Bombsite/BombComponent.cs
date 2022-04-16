using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Utilities;

namespace UnturnedStrike.Plugin.Bombsite
{
    public class BombComponent : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        void Start()
        {
            EffectManager.sendEffectReliable(pluginInstance.Configuration.Instance.BombBeepEffectId,
                pluginInstance.Configuration.Instance.BombBeepEffectRadius, transform.position);
        }

        public void StopBombBeepEffect()
        {
            foreach (var client in Provider.clients)
            {
                EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.BombBeepEffectId, client.transportConnection);
            }
        }

        public void DetonateBomb()
        {
            EffectManager.sendEffect(pluginInstance.Configuration.Instance.BombExplodeEffectId,
                pluginInstance.Configuration.Instance.BombExplodeEffectRadius, transform.position);
            List<EPlayerKill> list;
            DamageTool.explode(base.transform.position, pluginInstance.Configuration.Instance.BombDamageRadius,
                EDeathCause.CHARGE, CSteamID.Nil, pluginInstance.Configuration.Instance.BombDamage, 0, 0, pluginInstance.Configuration.Instance.BombDamage,
                pluginInstance.Configuration.Instance.BombDamage, 0, 0, pluginInstance.Configuration.Instance.BombDamage,
                out list, EExplosionDamageType.CONVENTIONAL, 90f, true, true, EDamageOrigin.Charge_Explosion, ERagdollEffect.NONE);
            BarricadeManager.damage(transform, 5f, 1f, false, CSteamID.Nil, EDamageOrigin.Charge_Self_Destruct);
        }

        public void SelfDestroy()
        {
            StopBombBeepEffect();
            BarricadeUtility.DestroyBarricade(transform);
        }

        void OnDestroy()
        {
        }
    }
}