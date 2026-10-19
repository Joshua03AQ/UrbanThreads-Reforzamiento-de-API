using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using UbranThreads_API.Dto;
using UbranThreads_API.Repositories.IRepositories;

namespace UbranThreads_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaRepository? _ventaRepo;
        private readonly ILogger<VentaController>? _logger;
        private readonly IMapper? _mapper;

        public VentaController(IVentaRepository ventaRepo, ILogger<VentaController> logger, IMapper mapper)
        {
            _ventaRepo = ventaRepo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetVentas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentas()
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las ventas...");

                var ventas = await _ventaRepo.GetAllAsync();

                return Ok(_mapper.Map<IEnumerable<VentaDto>>(ventas));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro al obtener las ventas: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener las ventas");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VentaDto>> GetVenta(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("ID de venta no válido");
                return BadRequest("ID de venta no válido");
            }

            try
            {
                _logger.LogInformation($"Obteniendo venta con ID: {id}");

                var venta = await _ventaRepo.GetByIdAsync(id);

                if (venta == null)
                {
                    _logger.LogWarning($"Venta con ID: {id} no encontrada");
                    return NotFound($"Venta con ID: {id} no encontrada");
                }
                return Ok(_mapper.Map<VentaDto>(venta));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener la venta con ID: {id}. Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener la venta");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VentaDto>> PostVenta(VentaCreateDto createDto)
        {
            if (createDto == null)
            {
                _logger.LogError("Se recibio una venta nula en la solicitud");
                return BadRequest("La venta no puede ser nula");
            }

            try
            {
                _logger.LogInformation($"Creando una nueva venta");

                // Verificar la validez del modelo
                if (!ModelState.IsValid)
                {
                    _logger.LogError("El modelo de venta no es valido.");
                    return BadRequest(ModelState);
                }

                // Crear la nueva venta
                var newVenta = _mapper.Map<Venta>(createDto);

                await _ventaRepo.CreateAsync(newVenta);

                _logger.LogInformation($"Nueva venta creada con ID {newVenta.VentaID}");

                var ventaDto = _mapper.Map<VentaDto>(newVenta);

                return CreatedAtAction(nameof(GetVenta), new { id = newVenta.VentaID }, newVenta);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear una nueva venta: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interna al crear una nueva venta. {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Eliminando venta con ID {id}");

                var venta = await _ventaRepo.GetByIdAsync(id);
                if (venta == null)
                {
                    _logger.LogWarning($"No se encontro ninguna venta con ID {id}");
                    return NotFound("Venta no encontrada");
                }

                await _ventaRepo.DeleteAsync(venta);

                _logger.LogInformation($"Venta con ID {id} eliminada correctamente.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la venta con ID {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Se produjo un error interno al intentar eliminar la venta.");
            }
        }


    }
}
