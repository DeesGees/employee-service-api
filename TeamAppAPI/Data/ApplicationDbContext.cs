using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TeamAppAPI.Data
{
    public class ApplicationDbContext
    {
        private readonly IConfiguration _config;

        public ApplicationDbContext(IConfiguration config)
        {
            _config = config;
        }


        public async Task<IEnumerable<T>> LoadDataAsync<T>(string sql, object param)
        {
            IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.QueryAsync<T>(sql, param);
        }

        public async Task<IEnumerable<T>> SelectAllAsync<T>(string sql)
        {
            IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.QueryAsync<T>(sql);
        }

        public async  Task<T> LoadSingleDataAsync<T>(string sql, object param)
        {
            IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
        }

        public async Task<int> ExecuteQueryAsync(string sql, object param)
        {
            IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.ExecuteAsync(sql, param);
        }
    }
}
