using Tutorial9.Model;

namespace Tutorial9.Repositories;

public interface IProductRepository
{
    Task<bool> Exists(int id);
    Task<decimal> GetPrice(int id);
}