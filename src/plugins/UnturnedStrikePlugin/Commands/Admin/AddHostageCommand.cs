using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Utilities;
using Logger = Rocket.Core.Logging.Logger;

namespace UnturnedStrike.Plugin.Commands.Admin
{
    public class AddHostageCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;
            if (!BarricadeUtility.RaycastBarricade(player.Player, out Transform transform) 
                || transform.name != pluginInstance.Configuration.Instance.HostageBarricadeId.ToString())
            {
                Logger.LogWarning($"Transform name: {transform?.name ?? "NULL"}");
                UnturnedChat.Say(caller, pluginInstance.Translate("AddHostageFail"));
                return;
            }

            var eulerAngles = transform.localRotation.eulerAngles;
            var hostage = new Hostage()
            {
                Position = new ConvertableVector3(transform.position),
                Angles = new ConvertableVector3()
                {
                    X = MeasurementTool.angleToByte(Mathf.RoundToInt(eulerAngles.x / 2f) * 2),
                    Y = MeasurementTool.angleToByte(Mathf.RoundToInt(eulerAngles.y / 2f) * 2),
                    Z = MeasurementTool.angleToByte(Mathf.RoundToInt(eulerAngles.z / 2f) * 2)
                }
            };
            pluginInstance.HostagesProvider.AddHostage(hostage);
            UnturnedChat.Say(caller, pluginInstance.Translate("AddHostageSuccessfull", hostage.Id));
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "addhostage";

        public string Help => "Adds a hostage your are lookign at";

        public string Syntax => "";

        public List<string> Aliases => new List<string>() { "ah" };

        public List<string> Permissions => new List<string>();
    }
}
