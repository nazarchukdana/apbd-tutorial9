using Microsoft.Data.SqlClient;

namespace Tutorial9.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration["DefaultConnection"];
    }

    public async Task<bool> Exists(int id)
    {
        string query = "select count(*) from Product where IdProduct = @id";
        using(var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(query, conn))
        {
            await conn.OpenAsync();
            cmd.Parameters.AddWithValue("@id", id);
            var result = (int) await cmd.ExecuteScalarAsync();
            return result > 0;
        }
    }

    public async Task<decimal> GetPrice(int id)
    {
        string query = "select Price from Product where IdProduct = @id";
        using(var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(query, conn))
        {
            await conn.OpenAsync();
            cmd.Parameters.AddWithValue("@id", id);
            var result = (decimal) await cmd.ExecuteScalarAsync();
            return result;
        }
    }
}