using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.ICategory.ICategoryServices;
using Application.Interfaces.IDish.IDishService;
using Application.Models.Request;
using Application.Models.Request.DishesRequest;
using Application.Models.Response;
using Application.Models.Response.DishesResponse;
using Application.Services.DishServices;
using Asp.Versioning;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Numerics;
using TP_Restaurante.Exceptions;

namespace TP_Restaurante.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Asp.Versioning.ApiVersion("1.0")]
    public class DishController : ControllerBase
    {
        private readonly ICreateDishUseCase _CreateDish;
        private readonly IUpdateDishUseCase _updateDish;
        private readonly ISearchAsyncUseCase _searchAsync;
        private readonly IGetDishByIdUseCase _GetDishByIdService;
        private readonly ICategoryExistUseCase _categoryExist;
        private readonly IDeleteDishUseCase _DeletedishService;

        public DishController(ICreateDishUseCase createDish, IUpdateDishUseCase UpdateDish, ISearchAsyncUseCase SearchAsync, IGetDishByIdUseCase GetDishByIdService, ICategoryExistUseCase CategoryExist, IDeleteDishUseCase DeletedishService)
        {
            _CreateDish = createDish;
            _updateDish = UpdateDish;
            _searchAsync = SearchAsync;
            _GetDishByIdService = GetDishByIdService;
            _categoryExist = CategoryExist;
            _DeletedishService = DeletedishService; 
        }

        // POST
        /// <summary>
        /// Crear nuevo plato
        /// </summary>
        /// <remarks>
        /// Crea un nuevo plato en el menú del restaurante.
        ///
        /// ## Validaciones:
        /// * El nombre del plato debe ser único
        /// * El precio debe ser mayor a 0
        /// * La categoría debe debe existir en el sistema
        ///
        /// ## Casos de uso:
        /// * Agregar nuevos platos al menú
        /// * Platos estacionales o especiales del chef
        /// </remarks>
        /// <param name="dishRequest">Datos del plato a crear.</param>
        /// <returns>El plato recién creado.</returns>
        [HttpPost]
        [SwaggerOperation(
        Summary = "Crear nuevo plato",
        Description = "Crea un nuevo plato en el menú del restaurante."
        )]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDish([FromBody] DishRequest dishRequest)
        {

            var categoryExists = await _categoryExist.CategoryExist(dishRequest.Category);
            if (!categoryExists)
            {
                throw new NotFoundException($"Category with ID {dishRequest.Category} not found.");
            }
            var createdDish = await _CreateDish.CreateDish(dishRequest);
            
            return CreatedAtAction(nameof(GetDishById), new { id = createdDish.Id }, createdDish);
        }

        //GET
        /// <summary>
        /// Buscar platos
        /// </summary>
        /// <remarks>
        /// Obtiene una lista de platos del menú con opciones de filtrado y ordenamiento.
        /// 
        /// **Filtros disponibles:**
        /// - Por nombre(búsqueda parcial)
        /// - Por categoría
        /// - Solo platos activos/todos
        /// 
        /// **Ordenamiento:**
        /// - Por precio ascendente o descendente
        /// - Sin ordenamiento específico
        /// 
        /// **Casos de uso:**
        ///
        /// - Mostrar menú completo a los clientes
        /// - Buscar platos específicos
        /// - Filtrar por categorías(entrantes, principales, postres)
        /// - Administración del menú(incluyendo platos inactivos)
        /// </remarks>
        /// <param name="name">Buscar platos por nombre (búsqueda parcial).</param>
        /// <param name="category">Filtrar por ID de categoría de plato.</param>
        /// <param name="sortByPrice">Ordenar por precio. Valores permitidos: `asc`, `desc`.</param>
        /// <param name="onlyActive">Filtrar por estado. `true` para solo disponibles, `false` para todos.</param>
        /// <returns>Una lista de platos que coinciden con los criterios.</returns>
        [HttpGet]
        [SwaggerOperation(
        Summary = "Buscar platos",
        Description = "Obtiene una lista de platos del menú con opciones de filtrado y ordenamiento."
        )]
        [ProducesResponseType(typeof(IEnumerable<DishResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Search(
            [FromQuery] string? name,
            [FromQuery] int? category,
            [FromQuery] OrderPrice? sortByPrice = OrderPrice.ASC,
            [FromQuery] bool? onlyActive = null)
        {
            if (category != 0 && category != null)
            {
                var categoryExists = await _categoryExist.CategoryExist(category.Value);
                if (!categoryExists)
                {
                    throw new NotFoundException($"Category with ID {category} not found.");
                }
            }
            var list = await _searchAsync.SearchAsync(name, category, sortByPrice, onlyActive);

            return Ok(list);
        }

        //GET by id
        /// <summary>
        /// Obtiene un plato por su ID.
        /// </summary>
        /// <remarks>
        /// Obtiene los detalles completos de un plato específico.
        ///
        /// **Casos de uso:**
        ///
        /// - Ver detalles de un plato antes de agregarlo a la orden
        /// - Mostrar información detallada en el menú
        /// - Verificación de disponibilidad
        /// <param name="id">ID único del plato</param>
        /// </remarks>
        [HttpGet("{id}")]
        [SwaggerOperation(
        Summary = "Buscar platos por ID",
        Description = "Buscar platos por ID."
        )]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDishById(Guid id)
        {
            var dish = await _GetDishByIdService.GetDishById(id);
            return Ok(dish);
        }

        //PUT
        /// <summary>
        /// Actualizar plato existente
        /// </summary>
        /// <remarks>
        /// Actualiza todos los campos de un plato existente en el menú.
        /// 
        /// ## Validaciones:
        /// * El plato debe existir en el sistema
        /// * Si se cambia el nombre, debe ser único
        /// * El precio debe ser mayor a 0
        /// * La categoría debe existir
        /// 
        /// ## Casos de uso:
        ///
        /// * Actualizar precios de platos
        /// * Modificar descripciones o ingredientes
        /// * Cambiar categorías de platos
        /// * Activar/desactivar platos del menú
        /// * Actualizar imágenes de platos
        /// </remarks>
        /// <param name="id">ID único del plato a actualizar.</param>
        /// <param name="dishRequest">Datos actualizados del plato.</param>
        /// <returns>El plato actualizado.</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(
        Summary = "Actualizar plato existente",
        Description = "Actualiza todos los campos de un plato existente en el menú."
        )]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateDish(Guid id, [FromBody] DishUpdateRequest dishRequest)
        {
            var categoryExists = await _categoryExist.CategoryExist(dishRequest.Category);
            if (!categoryExists)
            {
                throw new NotFoundException($"Category with ID {dishRequest.Category} not found.");
            }
            var result = await _updateDish.UpdateDish(id, dishRequest);

            return Ok(result);
        }

        //DELETE
        /// <summary>
        /// Eliminar plato
        /// </summary>
        /// <remarks>
        /// Elimina un plato del menú del restaurante.
        /// 
        /// ## Importante:
        /// * Solo se pueden eliminar platos que no estén en órdenes activas
        /// * Típicamente se recomienda desactivar (isActive=false) en lugar de eliminar
        /// 
        /// ## Casos de error 409:
        /// * El plato está incluido en órdenes pendientes o en proceso
        /// * El plato tiene dependencias que impiden su eliminación
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteDish(Guid id, [FromServices] IDeleteDishUseCase _deleteDish)
        {
            var result = await _deleteDish.DeleteDish(id);
            return Ok(result);
        }
    }
}
