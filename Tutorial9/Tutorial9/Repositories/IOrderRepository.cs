using Microsoft.Data.SqlClient;
using Tutorial9.Model;

namespace Tutorial9.Repositories;

public interface IOrderRepository
{
    Task<int?> GetOrderIdByProductAmount(int productId, int amount, DateTime createdAt, SqlTransaction transaction);
    Task<bool> OrderFulfilled(int id, SqlTransaction transaction);
    Task<bool> Exists(int productId, int amount, DateTime createdAt, SqlTransaction transaction);
    Task UpdateOrderFulfillment(int orderId, DateTime fulfilledAt, SqlTransaction transaction);
    Task<bool> IsOrderCompleted(int id, SqlTransaction transaction);

}