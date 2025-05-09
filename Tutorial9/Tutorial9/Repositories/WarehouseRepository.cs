using Microsoft.Data.SqlClient;
using Tutorial9.Model;

namespace Tutorial9.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly string _connectionString;

    public WarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<bool> Exists(int id)
    {
        string query = "select count(*) from Warehouse where IdWarehouse = @id";
        using(var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(query, conn))
        {
            await conn.OpenAsync();
            cmd.Parameters.AddWithValue("@id", id);
            var result = (int) await cmd.ExecuteScalarAsync();
            return result > 0;
        }
    }
}