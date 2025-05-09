using Microsoft.Data.SqlClient;

namespace Tutorial9.Repositories;

public interface IProductRepository
{
    Task<bool> Exists(int id, SqlTransaction transaction);
    Task<decimal?> GetPrice(int id, SqlTransaction transaction);
}