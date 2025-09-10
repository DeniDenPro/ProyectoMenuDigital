using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Request;
using Application.Models.Response;

namespace Application.Interfaces.IDish
{
    public interface IUpdateDishUseCase
    {
        // Update
        Task<UpdateDishResult> UpdateDish(Guid id, DishUpdateRequest DishUpdateRequest);
    }
}
