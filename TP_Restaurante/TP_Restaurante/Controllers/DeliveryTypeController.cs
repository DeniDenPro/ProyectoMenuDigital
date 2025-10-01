using Application.Interfaces.IDeliveryType;
using Application.Interfaces.IDeliveryType.IDeliveryTypeService;
using Application.Models.Response;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MenuDigital.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")]
    public class DeliveryTypeController : ControllerBase
    {
        private readonly IGetAllDeliveryTypesService _getallDeliverys;
        public DeliveryTypeController(IGetAllDeliveryTypesService getallDeliverys)
        {
            _getallDeliverys = getallDeliverys;
        }

        // GET
        /// <summary>
        /// Obtener tipos de entrega
        /// </summary>
        /// <remarks>
        /// Obtiene todos los tipos de entrega disponibles para las órdenes.
        /// 
        /// ## Casos de uso:
        ///
        /// * Mostrar opciones de entrega al cliente durante el pedido
        /// * Configurar diferentes métodos de entrega
        /// * Calcular costos de envío según el tipo
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryTypeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDeliveryTypes()
        {
            var deliveryTypes = await _getallDeliverys.GetAllDeliveryType();
            return Ok(deliveryTypes);
        }
    }
}
