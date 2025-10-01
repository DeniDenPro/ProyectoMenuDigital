using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Response.DishesResponse;

namespace Application.Interfaces.IDish.IDishService
{
    public interface IDeleteDishUseCase
    {
        // Delete
        Task<DishResponse> DeleteDish(Guid id);
    }
}
