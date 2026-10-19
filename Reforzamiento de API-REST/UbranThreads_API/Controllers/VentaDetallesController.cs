using AutoMapper;
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
    public class VentaDetallesController : ControllerBase
    {
        private readonly IVentaDetalleRepository? _ventaRepo;
        private readonly IVentaRepository? _ventaRep;
        private readonly ILogger<VentaDetallesController>? _logger;
        private readonly IMapper? _mapper;

        public VentaDetallesController(IVentaDetalleRepository ventaRepo, IVentaRepository ventaRep, ILogger<VentaDetallesController> logger, IMapper mapper)
        {
            _ventaRepo = ventaRepo;
            _ventaRep = ventaRep;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<VentaDetalleDto>>> GetVentaDetalles()
        {
            try
            {
                _logger.LogInformation("Obteniendo los detalles de las ventas");

                var detalles = await _ventaRepo.GetAllAsync();

                return Ok(_mapper.Map<IEnumerable<VentaDetalleDto>>(detalles));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener los detalles: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener los detalles.");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VentaDetalleDto>> GetVentaDetalle(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"ID de detalle no valido {id}");
                return BadRequest("ID de detalle no valido.");
            }

            try
            {
                _logger.LogInformation($"Obteniendo detalle con ID {id}");

                var detalle = await _ventaRepo.GetByIdAsync(id);

                if (detalle == null)
                {
                    _logger.LogWarning($"No se encontro ningun detalle con ID {id}");
                    return NotFound("Detalle no encontrado");
                }

                return Ok(_mapper.Map<VentaDetalleDto>(detalle));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener detalle con ID {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al intentar obtener el detalle de venta");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VentaDetalleDto>> PostVentaDetalle(VentaDetalleCreateDto createDto)
        {
            if (createDto.Equals(null))
            {
                _logger.LogError("Se recibio un detalle nulo en la solicitud.");
                return BadRequest("El detalle no puede ser nulo");
            }

            try
            {
                // verificar la validez del modelo
                if (!ModelState.IsValid)
                {
                    _logger.LogError("El modelo de detalle de venta no es valido.");
                    return BadRequest(ModelState);
                }

                // Crear el nuevo detalle de venta
                var newDetalle = _mapper.Map<VentaDetalle>(createDto);

                await _ventaRepo.CreateAsync(newDetalle);

                _logger.LogInformation($"Nuevo detalle de venta creado.");
                return CreatedAtAction(nameof(GetVentaDetalle), new { id = newDetalle.ID }, newDetalle);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear un nuevo detalle de venta: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al intentar crear un nuevo detalle de venta.");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDetalle(int id)
        {
            try
            {
                _logger.LogInformation($"Eliminando detalle de venta con ID {id}");

                var detalle = await _ventaRepo.GetByIdAsync(id);
                if (detalle == null)
                {
                    _logger.LogWarning($"No se encontro ningun detalle de venta con ID {id}");
                    return NotFound("Detalle de venta no encontrado");
                }

                await _ventaRepo.DeleteAsync(detalle);

                _logger.LogInformation($"Detalle de venta con ID {detalle.VentaID} eliminado correctamente.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el detalle con ID {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Se produjo un error al eliminar el detalle.");
            }
        }
    }
}
