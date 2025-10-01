using Application.Interfaces.IStatus;
using Application.Interfaces.IStatus.IStatusService;
using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.StatusService
{
    public class GetAllStatusesService : IGetAllStatusesService
    {
        private readonly IStatusQuery _statusQuery;
        public GetAllStatusesService(IStatusQuery statusQuery)
        {
            _statusQuery = statusQuery;
        }
        public async Task<List<StatusResponse>> GetAllStatus()
        {
            return (await _statusQuery.GetAllStatuses()).Select(s => new StatusResponse
            {
                Id = s.Id,
                Name = s.Name,
            }).ToList();
        }
    }
}
