using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Response;

namespace Application.Interfaces.IDish
{
    public interface IGetAllDishesAsyncUseCase
    {
        // List
        Task<List<DishResponse>> GetAllDishesAsync();
    }
}
