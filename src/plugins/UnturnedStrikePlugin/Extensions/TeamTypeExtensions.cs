using Rocket.Unturned.Chat;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Extensions
{
    public static class TeamTypeExtensions
    {
        public static EPlayerSpawnType GetPlayerSpawnType(this ETeamType team)
        {
            return team == ETeamType.Terrorist ? EPlayerSpawnType.Terrorist : EPlayerSpawnType.CounterTerrorist;
        }

        public static ETeamType GetOppositeTeam(this ETeamType team)
        {
            return team == ETeamType.Terrorist ? ETeamType.CounterTerrorist : ETeamType.Terrorist;
        }

        public static EWeaponTeam GetWeaponTeam(this ETeamType team)
        {
            return team == ETeamType.Terrorist ? EWeaponTeam.Terrorists : EWeaponTeam.CounterTerrorists;
        }

        public static string GetTranslation(this ETeamType team)
        {
            return UnturnedStrikePlugin.Instance.Translate(team.ToString());
        }

        public static string GetColor(this ETeamType team)
        {
            return team == ETeamType.Terrorist ? 
                UnturnedStrikePlugin.Instance.Configuration.Instance.TerroristColor : UnturnedStrikePlugin.Instance.Configuration.Instance.CounterTerroristColor;
        }
    }
}
