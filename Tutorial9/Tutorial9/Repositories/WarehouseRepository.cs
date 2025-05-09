using Microsoft.Data.SqlClient;
namespace Tutorial9.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    public async Task<bool> Exists(int id, SqlTransaction transaction)
    {
        string query = "select count(*) from Warehouse where IdWarehouse = @id";
        using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
        {
            cmd.Parameters.AddWithValue("@id", id);
            var result = await cmd.ExecuteScalarAsync();
            return result != null && (int)result > 0;
        }
    }
}