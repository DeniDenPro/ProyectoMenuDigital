using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Enums;
using Application.Models.Response;

namespace Application.Interfaces.IDish
{
    public interface ISearchAsyncUseCase
    {
        // Search filter
        Task<IEnumerable<DishResponse?>> SearchAsync(string? name, int? categoryId, OrderPrice? priceOrder, bool? onlyActive);
    }
}
