using Application.Interfaces.IDeliveryType;
using Application.Interfaces.IDeliveryType.IDeliveryTypeService;
using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.DeliveryTypeService
{
    public class GetAllDeliveryTypesService : IGetAllDeliveryTypesService
    {
        private readonly IDeliveryTypeQuery _deliveryTypeQuery;
        public GetAllDeliveryTypesService(IDeliveryTypeQuery deliveryTypeQuery)
        {
            _deliveryTypeQuery = deliveryTypeQuery;
        }
        public async Task<List<DeliveryTypeResponse>> GetAllDeliveryType()
        {
            var deliveryTypes = await _deliveryTypeQuery.GetAllDeliveryTypes();
            var deliveryTypeResponses = deliveryTypes.Select(d => new DeliveryTypeResponse
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();
            return deliveryTypeResponses;
        }
    }
}
