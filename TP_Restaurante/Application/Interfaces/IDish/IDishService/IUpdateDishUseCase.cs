using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Request.DishesRequest;
using Application.Models.Response.DishesResponse;

namespace Application.Interfaces.IDish.IDishService
{
    public interface IUpdateDishUseCase
    {
        // Update
        Task<DishResponse> UpdateDish(Guid id, DishUpdateRequest DishUpdateRequest);
    }
}
