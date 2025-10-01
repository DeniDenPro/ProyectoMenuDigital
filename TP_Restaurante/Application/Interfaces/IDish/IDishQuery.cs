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
        Task<Dish?> GetDishById(Guid id);
        Task<IEnumerable<Dish>> GetAllAsync(string? name = null, int? categoryId = null, OrderPrice? priceOrder = OrderPrice.ASC, bool? onlyActive = true);

        Task<bool> DishExists(string name, Guid? id);
        Task<List<Dish>> GetDishesByIds(IEnumerable<Guid> ids);
    }
}
