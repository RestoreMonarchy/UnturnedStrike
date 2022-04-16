using Rocket.API;
using Rocket.Unturned.Chat;
using System.Collections.Generic;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Providers;

namespace UnturnedStrike.Plugin.Commands.Admin
{
    public class RemoveWeaponCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var provider = pluginInstance.WeaponsProvider as JsonWeaponsProvider;
            if (provider == null)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("AddWeaponCommandDisabled"));
                return;
            }

            if (command.Length < 1 || !int.TryParse(command[0], out int weaponId))
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("RemoveWeaponInvalid"));
                return;
            }

            if (provider.RemoveWeapon(weaponId, out var weapon))
                UnturnedChat.Say(caller, pluginInstance.Translate("RemoveWeaponSuccess", weapon.Name));
            else
                UnturnedChat.Say(caller, pluginInstance.Translate("RemoveWeaponFail", weaponId));
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "removeweapon";

        public string Help => "Removes a weapon with specified ID";

        public string Syntax => "<weaponID>";

        public List<string> Aliases => new List<string>() { "rw" };

        public List<string> Permissions => new List<string>();
    }
}
