using Microsoft.Data.SqlClient;

namespace Tutorial9.Repositories;

public class ProductRepository : IProductRepository
{
    public async Task<bool> Exists(int id, SqlTransaction transaction)
    {
        string query = "select count(*) from Product where IdProduct = @id";
        using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
        {
            cmd.Parameters.AddWithValue("@id", id);
            var result = await cmd.ExecuteScalarAsync();
            return result != null && (int)result > 0;
        }
    }

    public async Task<decimal?> GetPrice(int id, SqlTransaction transaction)
    {
        string query = "select Price from Product where IdProduct = @id";
        using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
        {
            cmd.Parameters.AddWithValue("@id", id);
            var result = await cmd.ExecuteScalarAsync();
            return result != null ? (decimal?)result : null;
        }
    }
}