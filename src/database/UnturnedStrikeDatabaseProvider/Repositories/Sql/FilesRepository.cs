using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories.Sql
{
    public class FilesRepository : IFilesRepository
    {
        private readonly SqlConnection connection;

        public FilesRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<int> AddFileAsync(File file)
        {
            const string sql = "INSERT INTO dbo.Files (Name, MimeType, Data, Size) VALUES (@Name, @MimeType, @Data, @Size);" +
                "SELECT SCOPE_IDENTITY();";
            return await connection.ExecuteScalarAsync<int>(sql, file);
        }

        public async Task<File> GetFileAsync(int fileId)
        {
            const string sql = "SELECT Id, Name, MimeType, Data, Size, CreateDate FROM dbo.Files WHERE Id = @fileId;";
            return await connection.QuerySingleOrDefaultAsync<File>(sql, new { fileId });
        }
    }
}
