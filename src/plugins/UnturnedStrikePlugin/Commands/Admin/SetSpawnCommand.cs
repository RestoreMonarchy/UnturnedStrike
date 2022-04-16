using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Commands.Admin
{
    public class SetSpawnCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command != null && command.Length == 0 || !Enum.TryParse<EPlayerSpawnType>(command[0], out var spawnType))
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("SetSpawnInvalid"));
                return;
            }
            var player = (UnturnedPlayer)caller;
            var spawn = new PlayerSpawn()
            {
                Type = spawnType,
                Position = new ConvertableVector3(player.Position),
                Rotation = player.Rotation
            };
            SpawnsHelper.AddSpawn(spawn);
            UnturnedChat.Say(caller, pluginInstance.Translate("SetSpawnSuccess", spawn.Type, spawn.Position, spawn.Rotation)); 

        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "setspawn";

        public string Help => "Sets a player spawn in your current poistion";

        public string Syntax => "<spawnType>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
