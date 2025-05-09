using Microsoft.Data.SqlClient;

namespace Tutorial9.Repositories;

public interface IProductWarehouseRepository
{
    Task<int> InsertProductWarehouse(SqlTransaction transaction, int warehouseId, int productId, int orderId, int amount, decimal price, DateTime createdAt);
}