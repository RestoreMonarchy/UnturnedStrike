using Rocket.API;
using Rocket.Unturned.Chat;
using System.Collections.Generic;
using UnturnedStrike.Plugin.Providers;

namespace UnturnedStrike.Plugin.Commands.Admin
{
    public class RemoveHostageCommand : IRocketCommand
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            IHostagesProvider provider = pluginInstance.HostagesProvider;

            if (command.Length < 1 || !int.TryParse(command[0], out int hostageId))
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("RemoveHostageInvalid"));
                return;
            }

            if (provider.RemoveHostage(hostageId))
                UnturnedChat.Say(caller, pluginInstance.Translate("RemoveHostageSuccess", hostageId));
            else
                UnturnedChat.Say(caller, pluginInstance.Translate("RemoveHostageFail", hostageId));
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "removehostage";

        public string Help => "Removes a hostage with specified ID";

        public string Syntax => "<hostageId>";

        public List<string> Aliases => new List<string>() { "rh" };

        public List<string> Permissions => new List<string>();
    }
}
