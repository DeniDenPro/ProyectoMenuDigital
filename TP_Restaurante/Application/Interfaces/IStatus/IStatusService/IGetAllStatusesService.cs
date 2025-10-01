using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IStatus.IStatusService
{
    public interface IGetAllStatusesService
    {
        Task<List<StatusResponse>> GetAllStatus();
    }
}
