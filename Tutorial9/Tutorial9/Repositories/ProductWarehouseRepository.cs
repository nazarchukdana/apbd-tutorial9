using Microsoft.Data.SqlClient;

namespace Tutorial9.Repositories;

public class ProductWarehouseRepository : IProductWarehouseRepository
{
    public async Task<int> InsertProductWarehouse(SqlTransaction transaction, int warehouseId, int productId, int orderId, int amount,
        decimal price, DateTime createdAt)
    {
        string query = @"
        insert into Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)
        output inserted.IdProductWarehouse
        values(@idWarehouse, @idProduct, @idOrder, @amount, @price, @createdAt)";
        using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
        {
            cmd.Parameters.AddWithValue("@idWarehouse", warehouseId);
            cmd.Parameters.AddWithValue("@idProduct", productId);
            cmd.Parameters.AddWithValue("@idOrder", orderId);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@createdAt", createdAt);
            var insertedId = await cmd.ExecuteScalarAsync();
            return insertedId != null ? (int)insertedId : 0;
        }
    }
}