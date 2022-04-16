using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories.Sql
{
    public class WeaponsRepository : IWeaponsRepository
    {
        private readonly SqlConnection connection;

        public WeaponsRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<Weapon> AddWeaponAsync(Weapon weapon)
        {
            const string sql = "INSERT INTO dbo.Weapons (ItemId, Name, Description, ImageFileId, Category, Team, Price, " +
                "KillRewardMultiplier, MagazineId, MagazineAmount, IconUnicode, IsEnabled) " +
                "VALUES (@ItemId, @Name, @Description, @ImageFileId, @Category, @Team, @Price, @KillRewardMultiplier, " +
                "@MagazineId, @MagazineAmount, @IconUnicode, @IsEnabled);" +
                "SELECT * FROM dbo.Weapons WHERE Id = SCOPE_IDENTITY();";
            return await connection.QuerySingleOrDefaultAsync<Weapon>(sql, weapon);
        }

        public async Task<Weapon> GetWeaponAsync(int weaponId)
        {
            const string sql = "SELECT * FROM dbo.Weapons WHERE Id = @weaponId;";
            return await connection.QuerySingleOrDefaultAsync<Weapon>(sql, new { weaponId });
        }

        public async Task<IEnumerable<Weapon>> GetWeaponsAsync()
        {
            const string sql = "SELECT * FROM dbo.Weapons;";
            return await connection.QueryAsync<Weapon>(sql);
        }

        public async Task<IEnumerable<Weapon>> GetEnabledWeaponsAsync()
        {
            const string sql = "SELECT * FROM dbo.Weapons WHERE IsEnabled = 1;";
            return await connection.QueryAsync<Weapon>(sql);
        }

        public async Task UpdateWeaponAsync(Weapon weapon)
        {
            const string sql = "UPDATE dbo.Weapons SET ItemId = @ItemId, Name = @Name, Description = @Description, Category = @Category, " +
                "Team = @Team, Price = @Price, MagazineId = @MagazineId, MagazineAmount = @MagazineAmount, " +
                "IconUnicode = @IconUnicode, IsEnabled = @IsEnabled WHERE Id = @Id;";
            const string sql2 = "UPDATE dbo.Weapons SET ImageFileId = @ImageFileId WHERE Id = @Id;";

            await connection.ExecuteAsync(sql, weapon);
            if (weapon.ImageFileId != null)
                await connection.ExecuteAsync(sql2, weapon);
        }
    }
}
