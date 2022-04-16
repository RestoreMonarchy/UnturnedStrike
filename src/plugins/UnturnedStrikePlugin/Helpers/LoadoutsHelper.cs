using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Extensions;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Storage;

namespace UnturnedStrike.Plugin.Helpers
{
    public class LoadoutsHelper : MonoBehaviour
    {
        private static UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        private static DataStorage<TeamLoadout[]> TeamLoadoutsDataStorage { get; set; }
        private static TeamLoadout[] TeamLoadoutsData { get; set; }

        public static Dictionary<ETeamType, TeamLoadout> TeamLoadouts { get; private set; }

        void Awake()
        {
            TeamLoadoutsDataStorage = new DataStorage<TeamLoadout[]>(pluginInstance.Directory,
                $"TeamLoadoutsData.{Provider.map.Replace(' ', '_')}.json");
        }

        void Start()
        {
            ReloadLoadouts();
        }

        private static Dictionary<string, Tuple<byte, byte>> SkillsIndex = new Dictionary<string, Tuple<byte, byte>>()
        {
            { "Overkill", new Tuple<byte, byte>(0, 0) },
            { "Sharpshooter", new Tuple<byte, byte>(0, 1) },
            { "Dexterity", new Tuple<byte, byte>(0, 2) },
            { "Cardio", new Tuple<byte, byte>(0, 3) },
            { "Exercise", new Tuple<byte, byte>(0, 4) },
            { "Diving", new Tuple<byte, byte>(0, 5) },
            { "Parkour", new Tuple<byte, byte>(0, 6) },
            { "Sneakybeaky", new Tuple<byte, byte>(1, 0) },
            { "Vitality", new Tuple<byte, byte>(1, 1) },
            { "Immunity", new Tuple<byte, byte>(1, 2) },
            { "Toughness", new Tuple<byte, byte>(1, 3) },
            { "Strength", new Tuple<byte, byte>(1, 4) },
            { "Warmblooded", new Tuple<byte, byte>(1, 5) },
            { "Survival", new Tuple<byte, byte>(1, 6) },
            { "Healing", new Tuple<byte, byte>(2, 0) },
            { "Crafting", new Tuple<byte, byte>(2, 1) },
            { "Outdoors", new Tuple<byte, byte>(2, 2) },
            { "Cooking", new Tuple<byte, byte>(2, 3) },
            { "Fishing", new Tuple<byte, byte>(2, 4) },
            { "Agriculture", new Tuple<byte, byte>(2, 5) },
            { "Mechanic", new Tuple<byte, byte>(2, 6) },
            { "Engineer", new Tuple<byte, byte>(2, 7) }

        };

        private static readonly TeamLoadoutSkill[] DefaultSkills = new TeamLoadoutSkill[]
        {
            new TeamLoadoutSkill("Overkill", 7),
            new TeamLoadoutSkill("Sharpshooter", 7),
            new TeamLoadoutSkill("Dexterity", 5),
            new TeamLoadoutSkill("Cardio", 5),
            new TeamLoadoutSkill("Exercise", 5),
            new TeamLoadoutSkill("Diving", 5),
            new TeamLoadoutSkill("Parkour", 5),
            new TeamLoadoutSkill("Sneakybeaky", 7),
            new TeamLoadoutSkill("Vitality", 5),
            new TeamLoadoutSkill("Immunity", 5),
            new TeamLoadoutSkill("Toughness", 5),
            new TeamLoadoutSkill("Strength", 5),
            new TeamLoadoutSkill("Warmblooded", 5),
            new TeamLoadoutSkill("Survival", 5),
            new TeamLoadoutSkill("Healing", 7),
            new TeamLoadoutSkill("Crafting", 3),
            new TeamLoadoutSkill("Outdoors", 5),
            new TeamLoadoutSkill("Cooking", 3),
            new TeamLoadoutSkill("Fishing", 5),
            new TeamLoadoutSkill("Agriculture", 7),
            new TeamLoadoutSkill("Mechanic", 5),
            new TeamLoadoutSkill("Engineer", 3)
        };

        public static void ReloadLoadouts()
        {
            TeamLoadoutsData = TeamLoadoutsDataStorage.Read();
            if (TeamLoadoutsData == null)
            {
                TeamLoadoutsData = new TeamLoadout[]
                {
                    new TeamLoadout()
                    {
                        Team = ETeamType.Terrorist,
                        WeaponID = 1,
                        Clothes = new TeamLoadoutClothing[]
                        {
                            new TeamLoadoutClothing(15422, 0.75f),
                            new TeamLoadoutClothing(15423, 0.75f),
                            new TeamLoadoutClothing(15424, 0.75f),
                            new TeamLoadoutClothing(15429, 0.75f)
                        },
                        Skills = DefaultSkills
                    },
                    new TeamLoadout()
                    {
                        Team = ETeamType.CounterTerrorist,
                        WeaponID = 2,
                        Clothes = new TeamLoadoutClothing[]
                        {
                            new TeamLoadoutClothing(15425, 0.75f),
                            new TeamLoadoutClothing(15426, 0.75f),
                            new TeamLoadoutClothing(15427, 0.75f),
                            new TeamLoadoutClothing(15428, 0.75f)
                        },
                        Skills = DefaultSkills
                    }
                };
                TeamLoadoutsDataStorage.Save(TeamLoadoutsData);
            }
            TeamLoadouts = TeamLoadoutsData.ToDictionary(x => x.Team, x => x);
        }

        void OnDestroy()
        {
            
        }       

        public static bool IsTeamClothing(ushort id, ETeamType team)
        {
            return TeamLoadouts[team].Clothes.ToList().Exists(x => x.ItemId == id);
        }

        public static void GiveLoadout(GamePlayer player)
        {
            var items = new Dictionary<ushort, int>();
            var guns = new List<Item>();
            for (byte page = 0; page < PlayerInventory.PAGES; page++)
            {
                if (page == PlayerInventory.AREA)
                    continue;

                var count = player.NativePlayer.inventory.getItemCount(page);
                for (byte i = 0; i < count; i++)
                {
                    var item = player.NativePlayer.inventory.getItem(page, i);
                    if (item.item.id == pluginInstance.Configuration.Instance.BombItemId || item.item.id == pluginInstance.Configuration.Instance.HostageBackpackId)
                        continue;

                    if (item.item.metadata.Length > 16)
                    {
                        item.item.ReloadMagazine();
                        guns.Add(item.item);                        
                        continue;
                    }

                    if (items.ContainsKey(item.item.id))
                        items[item.item.id]++;
                    else
                        items.Add(item.item.id, 1);
                }
            }

            player.ClearInventory();

            // Give loadout weaponGun
            if (guns.Count == 0)
                pluginInstance.WeaponsProvider.GivePlayerWeapon(player, TeamLoadouts[player.TeamType].WeaponID);

            // Give the previously equipped two guns
            foreach (var gun in guns.Take(2))
                player.NativePlayer.inventory.forceAddItem(gun, true);

            // Give Clothes
            foreach (var clothing in TeamLoadouts[player.TeamType].Clothes)
                player.NativePlayer.inventory.forceAddItem(new Item(clothing.ItemId, true), true);
            
            // Give all previous player inventory items
            foreach (var item in items)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    player.NativePlayer.inventory.forceAddItem(new Item(item.Key, true), false);
                }
            }

            // Give rest of the guns after all other items
            foreach (var gun in guns.Skip(2))
                player.NativePlayer.inventory.forceAddItem(gun, true);

            // Give Skills to player if not null otherwise maxskills
            if (TeamLoadouts[player.TeamType].Skills != null)
            {
                foreach (var skill in TeamLoadouts[player.TeamType].Skills)
                {
                    player.NativePlayer.skills.ServerSetSkillLevel(SkillsIndex[skill.Name].Item1, SkillsIndex[skill.Name].Item2, skill.Level);
                    player.NativePlayer.skills.skills[SkillsIndex[skill.Name].Item1][SkillsIndex[skill.Name].Item2].level = skill.Level;
                }
            } else
            {
                player.MaxSkills();
            }                
        }
    }
}
