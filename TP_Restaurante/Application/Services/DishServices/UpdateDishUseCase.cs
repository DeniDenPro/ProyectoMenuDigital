using Application.Exceptions;
using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Interfaces.IDish.IDishService;
using Application.Interfaces.IOrderItem;
using Application.Models.Request.DishesRequest;
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
    public class UpdateDishUseCase : IUpdateDishUseCase
    {
        private readonly IDishCommand _command;
        private readonly IDishQuery _query;
        private readonly ICategoryQuery _categoryQuery;
        private readonly IOrderItemQuery _orderItemQuery;
        public UpdateDishUseCase(IDishCommand command, IDishQuery query, ICategoryQuery categoryQuery, IOrderItemQuery orderItemQuery)
        {
            _command = command;
            _query = query;
            _categoryQuery = categoryQuery;
            _orderItemQuery = orderItemQuery;
        }
        public async Task<DishResponse> UpdateDish(Guid id, DishUpdateRequest DishUpdateRequest)
        {
            var existingDish = await _query.GetDishById(id);

            if (existingDish == null)
            {
                throw new NotFoundException($"Dish with ID {id} not found.");
            }
            var alreadyExist = await _query.DishExists(DishUpdateRequest.Name, id);
            if (alreadyExist)
            {
                throw new ConflictException($"dish {DishUpdateRequest.Name} already exists");
            }
            
            var category = await _categoryQuery.GetCategoryById(DishUpdateRequest.Category);

            existingDish.Name = DishUpdateRequest.Name;
            existingDish.Description = DishUpdateRequest.Description;
            existingDish.Price = DishUpdateRequest.Price;
            existingDish.Available = DishUpdateRequest.IsActive;
            existingDish.CategoryId = DishUpdateRequest.Category;
            existingDish.ImageUrl = DishUpdateRequest.Image;
            existingDish.UpdateDate = DateTime.UtcNow;

            await _command.UpdateDish(existingDish);

            return new DishResponse
            {
                Id = existingDish.DishId,
                Name = existingDish.Name,
                Description = existingDish.Description,
                Price = existingDish.Price,
                Category = new GenericResponse { Id = category.Id, Name = category.Name },
                isActive = existingDish.Available,
                ImageUrl = existingDish.ImageUrl,
                createdAt = existingDish.CreateDate,
                updateAt = existingDish.UpdateDate
            };
        }
    }
}
