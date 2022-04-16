using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories.Sql
{
    public class BoxesRepository : IBoxesRepository
    {
        private readonly SqlConnection connection;

        public BoxesRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<Box> AddBoxAsync(Box box)
        {
            const string sql = "INSERT INTO dbo.Boxes (Name, Description, ImageFileId, Price, IsEnabled) " +
                "OUTPUT inserted.Id VALUES (@Name, @Description, @ImageFileId, @Price, @IsEnabled);";

            box.Id = await connection.ExecuteScalarAsync<int>(sql, box);
            await UpdateBoxWeaponSkinsAsync(box);

            return await GetWeaponSkinBoxAsync(box.Id);
        }

        private async Task UpdateBoxWeaponSkinsAsync(Box box)
        {
            const string sql = "DELETE FROM dbo.BoxWeaponSkins WHERE BoxId = @Id;";
            const string sql2 = "INSERT INTO dbo.BoxWeaponSkins (BoxId, WeaponSkinId, Weight) VALUES (@BoxId, @WeaponSkinId, @Weight);";

            await connection.ExecuteAsync(sql, box);

            foreach (var skin in box.WeaponSkins)
            {
                skin.BoxId = box.Id;
                await connection.ExecuteAsync(sql2, skin);
            }
        }


        public async Task UpdateBoxAsync(Box box)
        {
            const string sql = "UPDATE dbo.Boxes SET Name = @Name, Description = @Description, Price = @Price, " +
                "IsEnabled = @IsEnabled WHERE Id = @Id;";
            const string sql2 = "UPDATE dbo.Boxes SET ImageFileId = @ImageFileId WHERE Id = @Id;";

            await connection.ExecuteAsync(sql, box);
            if (box.ImageFileId != null)
                await connection.ExecuteAsync(sql2, box);

            await UpdateBoxWeaponSkinsAsync(box);
        }

        public async Task<IEnumerable<Box>> GetBoxesAsync()
        {
            const string sql = "SELECT * FROM dbo.Boxes;";
            return await connection.QueryAsync<Box>(sql);
        }

        public async Task<IEnumerable<Box>> GetEnabledBoxesAsync()
        {
            const string sql = "SELECT * FROM dbo.Boxes WHERE IsEnabled = 1;";
            return await connection.QueryAsync<Box>(sql);
        }

        public async Task<IEnumerable<Box>> GetWeaponSkinBoxesAsync()
        {
            const string sql = "SELECT b.*, bws.*, ws.* FROM dbo.Boxes b " +
                "LEFT JOIN dbo.BoxWeaponSkins bws ON bws.BoxId = b.Id " +
                "LEFT JOIN dbo.WeaponSkins ws ON ws.Id = bws.WeaponSkinId;";

            List<Box> boxes = new List<Box>();
            await connection.QueryAsync<Box, BoxWeaponSkin, WeaponSkin, Box>(sql,
            (b, bws, ws) =>
            {
                Box box = null;
                if (!boxes.Exists(x => x.Id == b.Id))
                {
                    box = b;
                    boxes.Add(box);
                } else
                {
                    box = boxes.First(x => x.Id == b.Id);
                }

                if (box.WeaponSkins == null)
                    box.WeaponSkins = new List<BoxWeaponSkin>();

                if (bws != null)
                {
                    bws.WeaponSkin = ws;
                    box.WeaponSkins.Add(bws);
                }

                return null;
            }, splitOn: "BoxId,Id");
            return boxes;
        }

        public async Task<Box> GetWeaponSkinBoxAsync(int boxId)
        {
            const string sql = "SELECT b.*, bws.*, ws.* FROM dbo.Boxes b " +
                "LEFT JOIN dbo.BoxWeaponSkins bws ON bws.BoxId = b.Id " +
                "LEFT JOIN dbo.WeaponSkins ws ON ws.Id = bws.WeaponSkinId " +
                "WHERE b.Id = @boxId";

            Box box = null;
            await connection.QueryAsync<Box, BoxWeaponSkin, WeaponSkin, Box>(sql,
            (b, bws, ws) =>
            {
                if (box == null)
                {
                    box = b;
                    box.WeaponSkins = new List<BoxWeaponSkin>();
                }

                if (bws != null)
                {
                    bws.WeaponSkin = ws;
                    box.WeaponSkins.Add(bws);
                }

                return b;
            }, new { boxId }, splitOn: "BoxId,Id");
            return box;
        }
    }
}
