using Tutorial9.Model;

namespace Tutorial9.Services;

public interface IWarehouseService
{
    Task<int> AddProductToWarehouseByProc(ProductWarehouseDTO dto);
    Task<int> AddProductToWarehouseByTrans(ProductWarehouseDTO dto);

}