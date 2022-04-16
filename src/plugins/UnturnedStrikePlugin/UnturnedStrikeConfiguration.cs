using Rocket.API;

namespace UnturnedStrike.Plugin
{
    public class UnturnedStrikeConfiguration : IRocketPluginConfiguration
    {
        public string GameType { get; set; }
        public bool IsSetUpRun { get; set; }
        public int FreezeTime { get; set; }
        public int RoundDuration { get; set; }
        public int RoundsToWin { get; set; }
        public int MaxRounds { get; set; }

        public ulong TerroristGroupId { get; set; }
        public ulong CounterTerroristGroupId { get; set; }
        public string TerroristColor { get; set; }
        public string CounterTerroristColor { get; set; }

        public int MaxTeamMembers { get; set; }
        public int MinTeamMembers { get; set; }
        public int MaxTeamMembersDifference { get; set; }

        public int GameStartDelay { get; set; }
        public int GameWaitingWarnRateSeconds { get; set; }
        public float NewRoundStartDelay { get; set; }
        public float NewGameStartDelay { get; set; }

        public ushort WaitingUIEffectId { get; set; }
        public ushort RoundsEffectId { get; set; }
        public ushort WinEffectId { get; set; }

        public int BombTime { get; set; }
        public int BombDamage { get; set; }
        public int BombDamageRadius { get; set; }
        public ushort BombItemId { get; set; }
        public ushort BombExplodeEffectId { get; set; }
        public ushort BombPlantedEffectId { get; set; }
        public ushort BombDefusedEffectId { get; set; }
        public float BombExplodeEffectRadius { get; set; }  
        public float BombSalvageTime { get; set; }
        
        public ushort BombBeepEffectId { get; set; }
        public float BombBeepEffectRadius { get; set; }
        public ushort BombDroppedEffectId { get; set; }

        public ushort BalanceEffectId { get; set; }
        public ushort BuyMenuEffectId { get; set; }
        public ushort TeamsEffectId { get; set; }
        public ushort BuyEffectId { get; set; }

        public int MoneyStart { get; set; }
        public int MoneyLimit { get; set; }
        public int MoneyRewardKill { get; set; }
        public int MoneyRewardWin { get; set; }
        public int MoneyRewardLose { get; set; }
        public int MoneyRewardBomb { get; set; }
        public int MoneyRewardDefuse { get; set; }
        public int MoneyRewardBombPlantLose { get; set; }

        public byte KillScore { get; set; }
        public byte BombPlantScore { get; set; }
        public byte BombDefuseScore { get; set; }
        public byte HostageRescueScore { get; set; }
        public byte MVPScore { get; set; }
        public ushort LeaderboardEffectId { get; set; }
        public ushort GameWinEffectId { get; set; }
        public ushort RoundTickEffectId { get; set; }

        public int BonusVIPMoney { get; set; }
        public string ChatVIPColor { get; set; }

        public int MaxGrenadesCount { get; set; }
        public string[] WeaponCategories { get; set; }

        public ushort HostageBarricadeId { get; set; }
        public ushort HostageBackpackId { get; set; }
        public ushort HostageTime { get; set; }
        public ushort HostageRescuedEffectId { get; set; }
        public float HostageSalvageTime { get; set; }

        public ushort TerroristsWinEffectId { get; set; }
        public ushort CounterTerroristsWinEffectId { get; set; }
        
        public ushort WarmupEffectId { get; set; }
        public int WarmupTime { get; set; }


        public void LoadDefaults()
        {
            GameType = "Defuse";
            IsSetUpRun = false;
            FreezeTime = 15;
            RoundDuration = 120;
            RoundsToWin = 8;
            MaxRounds = 15;

            TerroristGroupId = 1;
            CounterTerroristGroupId = 2;
            TerroristColor = "#FF8000";
            CounterTerroristColor = "#2FB0FA";

            MaxTeamMembers = 10;
            MinTeamMembers = 5;
            MaxTeamMembersDifference = 1;

            GameStartDelay = 15;
            GameWaitingWarnRateSeconds = 15;
            NewRoundStartDelay = 5;
            NewGameStartDelay = 10;

            WaitingUIEffectId = 4691;
            RoundsEffectId = 4682;
            WinEffectId = 4683;

            BombTime = 40;
            BombDamage = 500;
            BombDamageRadius = 1500;
            BombItemId = 13661;
            BombExplodeEffectId = 13663;
            BombExplodeEffectRadius = 50000;
            BombPlantedEffectId = 13664;
            BombDefusedEffectId = 13662;
            BombSalvageTime = 5;

            BombBeepEffectId = 13661;
            BombBeepEffectRadius = 800;
            BombDroppedEffectId = 120;

            BalanceEffectId = 4680;
            BuyMenuEffectId = 4681;
            TeamsEffectId = 4684;
            BuyEffectId = 13665;

            MoneyStart = 800;
            MoneyLimit = 10000;
            MoneyRewardKill = 300;
            MoneyRewardWin = 2700;
            MoneyRewardLose = 2000;
            MoneyRewardBomb = 300;
            MoneyRewardDefuse = 300;
            MoneyRewardBombPlantLose = 200;

            KillScore = 2;
            BombPlantScore = 1;
            BombDefuseScore = 1;
            HostageRescueScore = 1;
            MVPScore = 1;
            LeaderboardEffectId = 4685;
            GameWinEffectId = 4686;
            RoundTickEffectId = 13667;

            BonusVIPMoney = 300;
            ChatVIPColor = "#FFD700";

            MaxGrenadesCount = 3;

            WeaponCategories = new string[]
            {
                "Rifles",
                "Pistols",
                "SMGs",
                "Heavy",                
                "Grenades",
                "Extras"
            };

            HostageBarricadeId = 50011;
            HostageBackpackId = 50012;
            HostageTime = 75;
            HostageRescuedEffectId = 13670;
            HostageSalvageTime = 2;

            TerroristsWinEffectId = 13672;
            CounterTerroristsWinEffectId = 13671;

            WarmupEffectId = 4692;
            WarmupTime = 120;
        }
    }
}