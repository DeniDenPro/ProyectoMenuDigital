using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Interfaces.IDish.IDishService;
using Application.Models.Response;
using Application.Models.Response.DishesResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_Restaurante.Exceptions;

namespace Application.Services.DishServices
{
    public class GetDishByIdUseCase : IGetDishByIdUseCase
    {
        private readonly IDishCommand _command;
        private readonly IDishQuery _query;
        private readonly ICategoryQuery _categoryQuery;
        public GetDishByIdUseCase(IDishCommand command, IDishQuery query, ICategoryQuery categoryQuery)
        {
            _command = command;
            _query = query;
            _categoryQuery = categoryQuery;
        }
        public async Task<DishResponse?> GetDishById(Guid id)
        {
            var dish = await _query.GetDishById(id);
            if (dish == null)
            {
                throw new NotFoundException($"El plato con la id {id} no fue encontrado.");
            }

            return new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse { Id = dish.CategoryId, Name = dish.Category?.Name },
                isActive = dish.Available,
                ImageUrl = dish.ImageUrl,
                createdAt = dish.CreateDate,
                updateAt = dish.UpdateDate
            };
        }
    }
}
