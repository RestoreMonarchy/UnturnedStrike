using KillFeedPlugin.Models;
using Rocket.API;
using System.Collections.Generic;

namespace KillFeedPlugin
{
    public class KillFeedConfiguration : IRocketPluginConfiguration
    {        
        public ushort KillEffectId { get; set; }
        public int MaxNameLength { get; set; }
        public float ShowDuration { get; set; }
        public string BombUnicode { get; set; }
        public string HeadshotUnicode { get; set; }
        public string VehicleExplosionUnicode { get; set; }
        public string VehicleRoadKillUnicode { get; set; }
        public string PunchKillUnicode { get; set; }
        public string BurningKillUnicode { get; set; }
        public List<WeaponUnicode> WeaponUnicodes { get; set; }

        public void LoadDefaults()
        {            
            KillEffectId = 23425;
            MaxNameLength = 10;
            ShowDuration = 5;
            BombUnicode = "E031";
            HeadshotUnicode = "E0AA";
            VehicleExplosionUnicode = "E0A7";
            VehicleRoadKillUnicode = "E0A8";
            PunchKillUnicode = "E0A1";
            BurningKillUnicode = "E02E";
            WeaponUnicodes = new List<WeaponUnicode>()
            {
                //new WeaponUnicode(23003, "E008"), // Aug
                //new WeaponUnicode(23025, "E00E"), // M4A1
                //new WeaponUnicode(23020, "E004"), // glock 18
                //new WeaponUnicode(23032, "E013"), // p2000
                //new WeaponUnicode(23043, "E01E"), // tec 9
                //new WeaponUnicode(23013, "E003"), // five seven
                //new WeaponUnicode(23034, "E040"), // r8 revolver
                //new WeaponUnicode(23036, "E01D"), // sawed off
                //new WeaponUnicode(23026, "E011"), // Mac-10
                //new WeaponUnicode(23028, "E01B"), // Mag-7
                //new WeaponUnicode(23009, "E01A"), // pp bizon
                //new WeaponUnicode(23030, "E024"), // P90
                //new WeaponUnicode(23018, "E00D"), // Gali
                //new WeaponUnicode(23022, "E010"), // M4 s
                //new WeaponUnicode(23040, "E028"), // ssg 08
                //new WeaponUnicode(23006, "E009"), // awp
                //new WeaponUnicode(23015, "E00B"), // g3sg1
                //new WeaponUnicode(23037, "E026"), // scar 20
                //new WeaponUnicode(23011, "E00A"), // famas
                //new WeaponUnicode(23001, "E007"), // ak 47
                new WeaponUnicode(254, "E02C"), // grenade
                new WeaponUnicode(346, "E071"), // crossbow
                new WeaponUnicode(109, "E072"), // hawkhound
                new WeaponUnicode(101, "E073"), // schofield
                new WeaponUnicode(484, "E074"), // sportshot
                new WeaponUnicode(1143, "E075"), // sawed-off
                new WeaponUnicode(380, "E076"), // masterkey
                new WeaponUnicode(1436, "E077"), // quadbarrel
                new WeaponUnicode(1041, "E078"), // yuri
                new WeaponUnicode(1165, "E079"), // nailgun
                new WeaponUnicode(490, "E07A"), // chainsaw 
                new WeaponUnicode(1475, "E07B"), // jackhammer  
                new WeaponUnicode(1037, "E07C"), // heartbraker
                new WeaponUnicode(1039, "E07D"), // kryzkarek  
                new WeaponUnicode(107, "E07E"), // ace  
                new WeaponUnicode(1027, "E07F"), // viper  
                new WeaponUnicode(1024, "E080"), // peacemaker
                new WeaponUnicode(1021, "E081"), // avenger
                new WeaponUnicode(1018, "E082"), // sabertooth
                new WeaponUnicode(112, "E083"), // bluntforce
                new WeaponUnicode(116, "E084"), // honeybadger
                new WeaponUnicode(122, "E085"), // zubeknakov
                new WeaponUnicode(126, "E086"), // nykorev
                new WeaponUnicode(129, "E087"), // snayperskya
                new WeaponUnicode(132, "E088"), // dragonfang
                new WeaponUnicode(1337, "E089"), // paintball
                new WeaponUnicode(1360, "E08A"), // teklowvka
                new WeaponUnicode(1362, "E08B"), // augewehr
                new WeaponUnicode(1364, "E08C"), // hells fury
                new WeaponUnicode(1447, "E08D"), // scalar
                new WeaponUnicode(1379 , "E08E"), // calling card
                new WeaponUnicode(1377, "E08F"), // nightraider
                new WeaponUnicode(1375, "E090"), // fusilaut
                new WeaponUnicode(1244, "E091"), // neutral sentry
                new WeaponUnicode(1372, "E091"), // friendly sentry
                new WeaponUnicode(1373, "E091"), // hostile sentry
                new WeaponUnicode(1369, "E092"), // bulldog
                new WeaponUnicode(1366, "E093"), // vonya
                new WeaponUnicode(1476, "E094"), // luger
                new WeaponUnicode(1477 , "E095"), // mashinegewehr
                new WeaponUnicode(1480, "E096"), // deminator
                new WeaponUnicode(1481, "E097"), // empire
                new WeaponUnicode(1484, "E098"), // devils bane
                new WeaponUnicode(1488, "E099"), // swissgewehr
                new WeaponUnicode(18, "E09A"), // timberwolf
                new WeaponUnicode(363, "E09B"), // maplestrike
                new WeaponUnicode(4, "E09C"), // eaglefire
                new WeaponUnicode(474, "E09E"), // maple rifle
                new WeaponUnicode(479, "E09E"), // birch rifle
                new WeaponUnicode(480, "E09E"), // pine rifle
                new WeaponUnicode(519, "E09F"), // rocket launcher
                new WeaponUnicode(1441, "E0A2"), // mk2
                new WeaponUnicode(1382, "E0A4"), // ekho
                new WeaponUnicode(297, "E0A4"), // grizzly
                new WeaponUnicode(300, "E0A5"), // shadowstalker
                new WeaponUnicode(488, "E001"), // desert falcon (temp)
            };
        }
    }
}
