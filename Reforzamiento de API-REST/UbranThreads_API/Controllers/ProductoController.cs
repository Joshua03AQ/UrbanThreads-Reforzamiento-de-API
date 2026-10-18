using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using UbranThreads_API.Dto;
using UbranThreads_API.Repositories;
using UbranThreads_API.Repositories.IRepositories;

namespace UbranThreads_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepository? _productoRepo;
        private readonly ILogger<ProductoController>? _logger;
        private readonly IMapper? _mapper;
        public ProductoController(IProductoRepository productoRepo, ILogger<ProductoController> logger, IMapper mapper)
        {
            _productoRepo = productoRepo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los productos...");

                var productos = await _productoRepo.GetAllAsync();

                return Ok(_mapper.Map<IEnumerable<ProductoDto>>(productos));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener los productos: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener los productos.");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"El ID del producto proporcionado es inválido: {id}");
                return BadRequest("El ID del producto debe ser mayor que cero.");
            }

            try
            {
                _logger.LogError($"Obteniendo el producto con ID: {id}");

                var producto = await _productoRepo.GetByIdAsync(id);

                if (producto == null)
                {
                    _logger.LogWarning($"Producto con ID {id} no encontrado.");
                    return NotFound($"Producto con ID {id} no encontrado.");
                }

                return Ok(_mapper.Map<ProductoDto>(producto));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al obtener el producto con ID {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al obtener el producto con ID {id}.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductoDto>> PostProducto(ProductoCreateDto createDto)
        {
            if (createDto == null)
            {
                _logger.LogWarning("El producto proporcionado es nulo.");
                return BadRequest("El producto ingresado no puede ser nulo.");
            }

            try
            {
                _logger.LogInformation("Creando un nuevo producto...");

                //// Verificar si el producto ya existe
                //var existenciaProducto = await _productoRepo.GetAsync(p => p.ProductID.ToString() != null && p.ProductID == id);

                //if (existenciaProducto != null)
                //{
                //    _logger.LogInformation($"El producto con ID {id} ya existe.");
                //    ModelState.AddModelError("ProductoExiste", "El producto con este ID ya existe.");
                //    return BadRequest(ModelState);
                //}

                var productoExistente = await _productoRepo.GetAsync(p => p.Tipo == createDto.Tipo && p.Marca == createDto.Marca && p.Talla == createDto.Talla && p.Color == createDto.Color && p.Genero == createDto.Genero);

                if (productoExistente != null)
                {
                    return Conflict("Ya existe un producto con estas características.");
                }

                // Verificar la validez del modelo
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("El modelo del producto no es válido.");
                    return BadRequest(ModelState);
                }

                // Crear el nuevo Producto
                var newProduct = _mapper.Map<Producto>(createDto);

                await _productoRepo.CreateAsync(newProduct);

                _logger.LogInformation($"Producto con ID {newProduct.ProductID} creado exitosamente.");
                return CreatedAtAction(nameof(GetProducto), new { id = newProduct.ProductID }, _mapper.Map<ProductoDto>(newProduct));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al crear el producto: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al crear el producto.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutProducto(int id, ProductoUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.ProductoID)
            {
                //return BadRequest($"Los datos de entrada no son validos o el ID del producto no coincide con el ID {productDto.ProductoId}");
                return BadRequest($"Los datos de entrada no son validos o el ID del producto no coincide con el ID {id}");
            }

            try
            {
                _logger.LogInformation($"Actualizando el producto con ID: {id}");

                var existenciaProducto = await _productoRepo.GetByIdAsync(id);
                if (existenciaProducto == null)
                {
                    _logger.LogError($"No se encontro ningun producto con ID {id}");
                    return NotFound("El producto no existe");
                }

                // Actualizar solo las propiedades necesarias del producto existene
                _mapper.Map(updateDto, existenciaProducto);

                await _productoRepo.SaveChangesAsync();

                _logger.LogInformation($"Producto con ID {id} actualizado correctamente");

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await _productoRepo.ExistAsync(p => p.ProductID == id))
                {
                    _logger.LogInformation($"No se encontro ningun producto con ID {id}");
                    return BadRequest("El producto no se encontro durante la actualizacion.");
                }
                else
                {
                    _logger.LogError($"Error de concurrencia al actualizar el producto con ID {id}");
                    return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo actualizar el producto");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el producto con ID {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo actualizar el producto. Error interno en el serividor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            try
            {
                _logger.LogInformation($"Eliminando el producto...");

                var producto = await _productoRepo.GetByIdAsync(id);
                if (producto == null)
                {
                    _logger.LogWarning($"No se encontro ningun producto con ID {id}");
                    return NotFound("Producto no encontrado.");
                }

                await _productoRepo.DeleteAsync(producto);

                _logger.LogInformation($"Producto con ID {id} eliminado correctamente.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el producto con ID {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Se produjo un error interno al intentar eliminar el producto.");
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchProducto(int id, JsonPatchDocument<ProductoUpdateDto> patchDto)
        {
            if (id <= 0)
            {
                return BadRequest("ID de producto no valido.");
            }

            try
            {
                _logger.LogInformation($"Aplicando el parche al producto con ID {id}");

                var producto = await _productoRepo.GetByIdAsync(id);
                if (producto == null)
                {
                    _logger.LogWarning($"No se encontro ningun producto con ID {id}");
                    return NotFound("El producto no se encontro.");
                }

                var productoDto = _mapper.Map<ProductoUpdateDto>(producto);

                //pathcDto.ApplyTo(productoDto, ModelState);
                patchDto.ApplyTo(productoDto, ModelState);

                if (!ModelState.IsValid)
                {
                    _logger.LogError($"El modelo de producto despues de aplicar el parche no es valido.");
                    return BadRequest(ModelState);
                }

                _mapper.Map(productoDto, producto); // Aplicar cambios al objeto principal

                using (var transaction = await _productoRepo.BeginTransactionAsync())
                {
                    try
                    {
                        await _productoRepo.SaveChangesAsync();
                        transaction.Commit();
                        _logger.LogInformation($"Parche aplicado al producto con ID {id}");
                        return NoContent();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        if (!await _productoRepo.ExistAsync(p => p.ProductID == id))
                        {
                            _logger.LogWarning($"No se encontro ningun producto con ID {id}");
                            return NotFound();
                        }
                        else
                        {
                            _logger.LogError($"Erro de concurrencia al aplicar el parche al producto con ID {id}");
                            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al aplicar el parche al producto.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error al aplicar el parche al producto con ID {id}: {ex.Message}");
                        transaction.Rollback();
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al aplicar el parche al producto");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al aplicar el parche al producto con ID {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al aplicar el parche al producto.");
            }
        }
    }
}
