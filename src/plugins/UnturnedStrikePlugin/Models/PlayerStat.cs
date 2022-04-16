using Steamworks;

namespace UnturnedStrike.Plugin.Models
{
    public class PlayerStat
    {
        public PlayerStat(CSteamID steamId, string displayName, ETeamType team)
        {
            SteamId = steamId;
            DisplayName = displayName;
            IconUrl = string.Empty;
            Team = team;
        }

        public CSteamID SteamId { get; set; }
        public string DisplayName { get; set; }
        public string IconUrl { get; set; }
        public ETeamType Team { get; set; }
        public bool IsInTeam { get; set; }

        public short Kills { get; set; }
        public short Deaths { get; set; }
        public byte BombPlants { get; set; }
        public byte BombDefuses { get; set; }
        public byte HostageRescues { get; set; }

        public byte MVPs { get; set; }
        public float KD => Deaths == 0 ? Kills : (float)Kills / (float)Deaths;
        public short Score 
        {
            get
            {
                short total = 0;
                total += (short)(Kills * UnturnedStrikePlugin.Instance.Configuration.Instance.KillScore);
                total += (short)(BombPlants * UnturnedStrikePlugin.Instance.Configuration.Instance.BombPlantScore);
                total += (short)(BombDefuses * UnturnedStrikePlugin.Instance.Configuration.Instance.BombDefuseScore);
                total += (short)(HostageRescues * UnturnedStrikePlugin.Instance.Configuration.Instance.HostageRescueScore);
                total += (short)(MVPs * UnturnedStrikePlugin.Instance.Configuration.Instance.MVPScore);
                return total;
            }
        }
    }
}
