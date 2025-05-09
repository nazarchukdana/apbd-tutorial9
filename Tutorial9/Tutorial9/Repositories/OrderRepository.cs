using Microsoft.Data.SqlClient;

namespace Tutorial9.Repositories;

public class OrderRepository : IOrderRepository
{

    public async Task<int?> GetOrderIdByProductAmount(int productId, int amount, DateTime createdAt, SqlTransaction transaction)
    {
        string query = @"
            SELECT IdOrder
            from [Order]
            where IdProduct = @productId And Amount = @amount And CreatedAt < @createdAt and FulfilledAt is null";
        using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
        {
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@createdAt", createdAt);
            var result = await cmd.ExecuteScalarAsync();
            return result != null ? (int?)result : null;
        }
    }

    public async Task<bool> OrderFulfilled(int id, SqlTransaction transaction)
    {
        string query = "select count(*) from [Order] where IdOrder = @id and FulfilledAt is not null";
        using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
        {
            cmd.Parameters.AddWithValue("@id", id);
            var result = await cmd.ExecuteScalarAsync();
            return result != null && (int)result > 0;
        }
    }

    public async Task<bool> Exists(int productId, int amount, DateTime createdAt, SqlTransaction transaction)
    {
        string query = @"
            SELECT Count(*) 
            from [Order]
            where IdProduct = @productId And Amount = @amount And CreatedAt < @createdAt";
        using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
        {
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@createdAt", createdAt);
            var result =await cmd.ExecuteScalarAsync();
            return result != null && (int)result > 0;
        }
    }

    public async Task UpdateOrderFulfillment(int orderId, DateTime fulfilledAt, SqlTransaction transaction)
    {
        string query = "Update [Order] Set FulfilledAt = @fulfilledAt Where IdOrder = @id";
        using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
        {
            cmd.Parameters.AddWithValue("@fulfilledAt", fulfilledAt);
            cmd.Parameters.AddWithValue("@id", orderId);
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task<bool> IsOrderCompleted(int id, SqlTransaction transaction)
    {
        string query = "Select count(*) from Product_Warehouse where IdOrder = @id";
        using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
        {
            cmd.Parameters.AddWithValue("@id", id);
            var result = await cmd.ExecuteScalarAsync();
            return result != null && (int)result > 0;
        }
    }
}