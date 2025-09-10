using Application.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Response;

namespace Application.Interfaces.IDish
{
    public interface IDishQuery
    {
        Task<List<Dish>> GetAllDishes();
        Task<Dish?> GetDishById(int id);
        Task<IEnumerable<Dish>> GetAllAsync(string? name = null, int? categoryId = null, OrderPrice? priceOrder = OrderPrice.ASC);

        Task<bool> DishExists(string name);
    }
}
