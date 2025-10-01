using Application.Interfaces.ICategory;
using Application.Interfaces.ICategory.ICategoryServices;
using Application.Models.Response;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MenuDigital.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")]
    public class CategoryController : ControllerBase
    {
        private readonly IGetAllCategoriesService _getAllCategoryService;

        public CategoryController(IGetAllCategoriesService getAllCategoryService)
        {
            _getAllCategoryService = getAllCategoryService;
        }

        // GET
        /// <summary>
        /// Obtener categorias de platos
        /// </summary>
        /// <remarks>
        /// Obtiene todas las categorías disponibles para clasificar platos.
        /// 
        /// ## Casos de uso:
        ///
        /// * Mostrar categorías en formularios de creación/edición de platos
        /// * Filtros de búsqueda en el menú
        /// * Organización del menú por secciones
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryTypeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _getAllCategoryService.GetAllCategory();
            return Ok(categories);
        }
    }
}
