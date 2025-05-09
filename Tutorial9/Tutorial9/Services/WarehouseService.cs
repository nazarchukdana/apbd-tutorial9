using System.Data;
using Microsoft.Data.SqlClient;
using Tutorial9.Model;
using Tutorial9.Repositories;

namespace Tutorial9.Services;

public class WarehouseService : IWarehouseService
{
    private readonly string _connectionString;
    private readonly IProductWarehouseRepository _productWarehouseRepository;
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IOrderRepository _orderRepository;

    public WarehouseService(
        IConfiguration configuration,
        IProductWarehouseRepository productWarehouseRepository,
        IProductRepository productRepository,
        IWarehouseRepository warehouseRepository,
        IOrderRepository orderRepository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _productWarehouseRepository = productWarehouseRepository;
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _orderRepository = orderRepository;
    }

    public async Task<int> AddProductToWarehouseByProc(ProductWarehouseDTO dto)
    {
        if (dto.Amount <= 0) throw new InvalidOperationException("Amount must be greater than 0");
        using (var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand
               {
                   Connection = conn,
                   CommandText = "AddProductToWarehouse",
                   CommandType = CommandType.StoredProcedure
               })
        {
            await conn.OpenAsync();
            cmd.Parameters.AddWithValue("IdProduct", dto.IdProduct);
            cmd.Parameters.AddWithValue("@IdWarehouse", dto.IdWarehouse);
            cmd.Parameters.AddWithValue("@Amount", dto.Amount);
            cmd.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);
            try
            {
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error: {ex.Message}");
            }
        }
    }

    public async Task<int> AddProductToWarehouseByTrans(ProductWarehouseDTO dto)
    {
        if(dto.Amount <= 0) throw new InvalidOperationException("Amount must be greater than 0");
        if(! await _productRepository.Exists(dto.IdProduct))
            throw new InvalidOperationException("Product not found");
        if(! await _warehouseRepository.Exists(dto.IdWarehouse))
            throw new InvalidOperationException("Warehouse not found");
        if(! await _orderRepository.Exists(dto.IdProduct, dto.Amount, dto.CreatedAt))
            throw new InvalidOperationException("Order not found");
        var orderId = await _orderRepository.GetOrderIdByProductAmount(dto.IdProduct, dto.Amount, dto.CreatedAt);
        if(await _orderRepository.OrderFulfilled(orderId) || await _orderRepository.IsOrderCompleted(orderId))
            throw new InvalidOperationException("Order is completed");
        using var conn = new SqlConnection(_connectionString);
        await using var transaction = await conn.BeginTransactionAsync() as SqlTransaction;
        try
        {
            await conn.OpenAsync();
            DateTime currentDate = DateTime.Now;
            decimal price = await _productRepository.GetPrice(dto.IdProduct) * dto.Amount;
            await _orderRepository.UpdateOrderFulfillment(orderId, currentDate, transaction);
            int id = await _productWarehouseRepository.InsertProductWarehouse(transaction, dto.IdWarehouse,
                dto.IdProduct, orderId, dto.Amount, price, currentDate);
            await transaction.CommitAsync();
            return id;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Database Error: {ex.Message}");
        }
        
    }
}