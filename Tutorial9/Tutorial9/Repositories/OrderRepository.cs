using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient;
using Tutorial9.Model;

namespace Tutorial9.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly string _connectionString;

    public OrderRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<int> GetOrderIdByProductAmount(int productId, int amount, DateTime createdAt)
    {
        string query = @"
            SELECT IdOrder
            from [Order]
            where IdProduct = @productId And Amount = @amount And CreatedAt < @createdAt";
        using (var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@createdAt", createdAt);
            await conn.OpenAsync();
            return (int)await cmd.ExecuteScalarAsync();
        }

        return 0;
    }

    public async Task<bool> OrderFulfilled(int id)
    {
        string query = "select count(*) from [Order] where IdOrder = @id and FullfilledAt is not null";
        using(var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            var result = (int)await cmd.ExecuteScalarAsync();
            return result > 0;
        }
    }

    public async Task<bool> Exists(int productId, int amount, DateTime createdAt)
    {
        string query = @"
            SELECT Count(*) 
            from Order
            where ProductId = @productId And Amount = @amount And CreatedAt < @createdAt";
        using (var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@createdAt", createdAt);
            await conn.OpenAsync();
            var result = (int) await cmd.ExecuteScalarAsync();
            return result > 0;
        }
    }

    public async Task UpdateOrderFulfillment(int orderId, DateTime fulfilledAt, SqlTransaction transaction)
    {
        string query = "Update [Order] Set FullfilledAt = @fullfilledAt Where IdOrder = @id";
        using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
        {
            cmd.Parameters.AddWithValue("@filfilledAt", fulfilledAt);
            cmd.Parameters.AddWithValue("@id", orderId);
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task<bool> IsOrderCompleted(int id)
    {
        string query = "Select count(*) from Product_Warehouse where IdOrder = @id";
        using (var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            var result = (int) await cmd.ExecuteScalarAsync();
            return result > 0;
        }
    }
}