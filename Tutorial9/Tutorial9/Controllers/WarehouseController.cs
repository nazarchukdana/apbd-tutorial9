using Microsoft.AspNetCore.Mvc;
using Tutorial9.Model;
using Tutorial9.Services;

namespace Tutorial9.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost("proc")]
    public async Task<IActionResult> AddProductToWarehouseByProcedure([FromBody] ProductWarehouseDTO dto)
    {
        try
        {
            
            var id = await _warehouseService.AddProductToWarehouseByProc(dto);
            return Ok(id);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [HttpPost]
    public async Task<IActionResult> AddProductToWarehouseByTransaction([FromBody] ProductWarehouseDTO dto)
    {
        try
        {
            
            var id = await _warehouseService.AddProductToWarehouseByTrans(dto);
            return Ok(id);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}