using Application.Interfaces.IDeliveryType.IDeliveryTypeService;
using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.DeliveryTypeService
{
    public class GetDeliveryTypesByIdService : IGetDeliveryTypeByIdService
    {
        public Task<DeliveryTypeResponse> GetDeliveryTypeById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
