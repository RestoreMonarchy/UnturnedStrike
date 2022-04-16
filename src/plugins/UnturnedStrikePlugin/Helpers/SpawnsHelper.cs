using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Storage;
using Logger = Rocket.Core.Logging.Logger;

namespace UnturnedStrike.Plugin.Helpers
{
    public class SpawnsHelper : MonoBehaviour
    {
        private static UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        private static System.Random random = new System.Random();

        public static Dictionary<EPlayerSpawnType, PlayerSpawn[]> PlayerSpawns { get; set; }

        private static DataStorage<List<PlayerSpawn>> PlayerSpawnsStorage { get; set; }
        private static List<PlayerSpawn> PlayerSpawnsData { get; set; }

        void Awake()
        {
            PlayerSpawnsStorage = new DataStorage<List<PlayerSpawn>>(pluginInstance.Directory, 
                $"SpawnsData.{Provider.map.Replace(' ', '_')}.json");
            randomizedSpawns = new Dictionary<EPlayerSpawnType, List<PlayerSpawn>>()
            {
                { EPlayerSpawnType.Lobby, new List<PlayerSpawn>() },
                { EPlayerSpawnType.Terrorist, new List<PlayerSpawn>() },
                { EPlayerSpawnType.CounterTerrorist, new List<PlayerSpawn>() },
                { EPlayerSpawnType.Warmup, new List<PlayerSpawn>() }
            };
        }

        void Start()
        {
            PlayerSpawnsData = PlayerSpawnsStorage.Read();
            if (PlayerSpawnsData == null)
                PlayerSpawnsData = new List<PlayerSpawn>();
            UpdateSpawns();
        }

        public static void AddSpawn(PlayerSpawn spawn)
        {
            PlayerSpawnsData.Add(spawn);
            PlayerSpawnsStorage.Save(PlayerSpawnsData);
            UpdateSpawns();
        }

        private static void UpdateSpawns()
        {
            PlayerSpawns = new Dictionary<EPlayerSpawnType, PlayerSpawn[]>
            {
                [EPlayerSpawnType.Lobby] = PlayerSpawnsData.Where(x => x.Type == EPlayerSpawnType.Lobby).ToArray(),
                [EPlayerSpawnType.Terrorist] = PlayerSpawnsData.Where(x => x.Type == EPlayerSpawnType.Terrorist).ToArray(),
                [EPlayerSpawnType.CounterTerrorist] = PlayerSpawnsData.Where(x => x.Type == EPlayerSpawnType.CounterTerrorist).ToArray(),
                [EPlayerSpawnType.Warmup] = PlayerSpawnsData.Where(x => x.Type == EPlayerSpawnType.Warmup).ToArray()
            };
        }

        private static Dictionary<EPlayerSpawnType, List<PlayerSpawn>> randomizedSpawns;        

        private static PlayerSpawn GetRandomSpawn(EPlayerSpawnType spawnType)
        {
            if (randomizedSpawns[spawnType].Count() == 0)
            {
                randomizedSpawns[spawnType] = PlayerSpawns[spawnType].OrderBy(x => random.Next()).ToList();
            }

            PlayerSpawn spawn;
            spawn = randomizedSpawns[spawnType].FirstOrDefault();
            randomizedSpawns[spawnType].Remove(spawn);
            return spawn;
        }

        private static void TeleportToSpawn(Player player, PlayerSpawn spawn)
        {
            player.teleportToLocationUnsafe(spawn.Position.ToVector3(), spawn.Rotation);
        }

        public static void TeleportPlayer(Player player, EPlayerSpawnType spawnType)
        {
            if (PlayerSpawns[spawnType].Length == 0)
                throw new Exception($"There isnt any {spawnType} spawn!");

            var spawn = GetRandomSpawn(spawnType);
            TeleportToSpawn(player, spawn);
        }

        public static void TeleportGamePlayers(IEnumerable<GamePlayer> players, EPlayerSpawnType spawnType)
        {
            var spawns = PlayerSpawns[spawnType].OrderByDescending(x => random.Next());
            int index = 0;
            foreach (var player in players)
            {
                if (index >= spawns.Count())
                {
                    Logger.LogWarning($"Not enough spawns for {spawnType}");
                    TeleportToSpawn(player.NativePlayer, spawns.ElementAt(0));
                } else
                {
                    TeleportToSpawn(player.NativePlayer, spawns.ElementAt(index));
                    index++;
                }
            }
        }

        public static PlayerSpawnpoint GetLobbySpawnPoint(bool isAlt)
        {
            var spawn = PlayerSpawns[EPlayerSpawnType.Lobby][random.Next(0, PlayerSpawns[EPlayerSpawnType.Lobby].Length)];
            if (spawn == null)
                return new PlayerSpawnpoint(new Vector3(0, 100, 0), 0, isAlt);
            return new PlayerSpawnpoint(spawn.Position.ToVector3(), spawn.Rotation, isAlt);
        }

        public static PlayerSpawnpoint GetWarmupSpawnPoint(bool isAlt)
        {
            var spawn = GetRandomSpawn(EPlayerSpawnType.Warmup);
            if (spawn == null)
                return GetLobbySpawnPoint(isAlt);
            return new PlayerSpawnpoint(spawn.Position.ToVector3(), spawn.Rotation, isAlt);
        }
    }
}
