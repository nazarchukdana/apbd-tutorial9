using Microsoft.Data.SqlClient;
using Tutorial9.Model;

namespace Tutorial9.Repositories;

public interface IOrderRepository
{
    Task<int> GetOrderIdByProductAmount(int productId, int amount, DateTime createdAt);
    Task<bool> OrderFulfilled(int id);
    Task<bool> Exists(int productId, int amount, DateTime createdAt);
    Task UpdateOrderFulfillment(int orderId, DateTime fulfilledAt, SqlTransaction transaction);
    Task<bool> IsOrderCompleted(int id);

}