using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories.Sql
{
    public class WeaponSkinsRepository : IWeaponSkinsRepository
    {
        private readonly SqlConnection connection;

        public WeaponSkinsRepository(SqlConnection connection)
        {
            this.connection = connection; 
        }

        public async Task<PlayerWeaponSkin> AddPlayerWeaponSkinAsync(int weaponSkinId, string playerId)
        {
            const string sql = "INSERT INTO dbo.PlayerWeaponSkins (PlayerId, WeaponSkinId) VALUES (@PlayerId, @WeaponSkinId);" +
                "SELECT SCOPE_IDENTITY();";
            return await GetPlayerWeaponSkinAsync(await connection.ExecuteScalarAsync<int>(sql, new { PlayerId = playerId, WeaponSkinId = weaponSkinId }));
        }

        public async Task<WeaponSkin> AddWeaponSkinAsync(WeaponSkin weaponSkin)
        {
            const string sql = "INSERT INTO dbo.WeaponSkins (WeaponId, ItemId, Name, Description, ImageFileId, Rarity)" +
                "VALUES (@WeaponId, @ItemId, @Name, @Description, @ImageFileId, @Rarity);" +
                "SELECT SCOPE_IDENTITY();";
            return await GetWeaponSkinAsync(await connection.ExecuteScalarAsync<int>(sql, weaponSkin));
        }

        public async Task<IEnumerable<PlayerWeaponSkin>> GetPlayerEquipedWeaponSkinsAsync(string playerId)
        {
            const string sql = "SELECT pws.*, ws.*, w.* FROM dbo.PlayerWeaponSkins pws JOIN dbo.WeaponSkins ws ON ws.Id = pws.WeaponSkinId " +
                "JOIN dbo.Weapons w ON w.Id = ws.WeaponId WHERE pws.PlayerId = @playerId AND pws.IsEquiped = 1;";
            return await connection.QueryAsync<PlayerWeaponSkin, WeaponSkin, Weapon, PlayerWeaponSkin>(sql, (pws, ws, w) =>
            {
                pws.WeaponSkin = ws;
                pws.WeaponSkin.Weapon = w;
                return pws;
            }, new { playerId });
        }

        public async Task<PlayerWeaponSkin> GetPlayerWeaponSkinAsync(int weaponSkinId)
        {
            const string sql = "SELECT pws.*, ws.*, w.* FROM dbo.PlayerWeaponSkins pws JOIN dbo.WeaponSkins ws ON ws.Id = pws.WeaponSkinId " +
                "JOIN dbo.Weapons w ON w.Id = ws.WeaponId WHERE pws.Id = @weaponSkinId;";
            return (await connection.QueryAsync<PlayerWeaponSkin, WeaponSkin, Weapon, PlayerWeaponSkin>(sql, (pws, ws, w) => 
            {
                pws.WeaponSkin = ws;
                pws.WeaponSkin.Weapon = w;
                return pws;
            }, new { weaponSkinId })).FirstOrDefault();
        }

        public async Task<IEnumerable<PlayerWeaponSkin>> GetPlayerWeaponSkinsAsync(string playerId)
        {
            const string sql = "SELECT pws.*, ws.*, w.* FROM dbo.PlayerWeaponSkins pws JOIN dbo.WeaponSkins ws ON ws.Id = pws.WeaponSkinId " +
                "JOIN dbo.Weapons w ON w.Id = ws.WeaponId WHERE pws.PlayerId = @playerId;";
            return await connection.QueryAsync<PlayerWeaponSkin, WeaponSkin, Weapon, PlayerWeaponSkin>(sql, (pws, ws, w) =>
            {
                pws.WeaponSkin = ws;
                pws.WeaponSkin.Weapon = w;
                return pws;
            }, new { playerId });
        }

        public async Task<WeaponSkin> GetWeaponSkinAsync(int weaponSkinId)
        {
            const string sql = "SELECT ws.*, w.* FROM dbo.WeaponSkins ws JOIN dbo.Weapons w ON w.Id = ws.WeaponId " +
                "WHERE ws.Id = @weaponSkinId;";
            return (await connection.QueryAsync<WeaponSkin, Weapon, WeaponSkin>(sql, (ws, w) => 
            {
                ws.Weapon = w;
                return ws;
            }, new { weaponSkinId })).FirstOrDefault();
        }

        public async Task<IEnumerable<WeaponSkin>> GetWeaponSkinsAsync()
        {
            const string sql = "SELECT ws.*, w.* FROM dbo.WeaponSkins ws JOIN dbo.Weapons w ON w.Id = ws.WeaponId;";
            return await connection.QueryAsync<WeaponSkin, Weapon, WeaponSkin>(sql, (ws, w) =>
            {
                ws.Weapon = w;
                return ws;
            });
        }

        public async Task<int> ToggleEquipWeaponSkinAsync(string playerId, int playerWeaponSkinId)
        {
            const string sql = "dbo.ToggleEquipWeaponSkin";
            return await connection.ExecuteScalarAsync<int>(sql, new { playerId, playerWeaponSkinId }, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateWeaponSkinAsync(WeaponSkin weaponSkin)
        {
            const string sql = "UPDATE dbo.WeaponSkins SET WeaponId = @WeaponId, ItemId = @ItemId, Name = @Name, " +
                "Description = @Description, ImageFileId = @ImageFileId, Rarity = @Rarity WHERE Id = @Id;";
            await connection.ExecuteAsync(sql, weaponSkin);
        }
    }
}
