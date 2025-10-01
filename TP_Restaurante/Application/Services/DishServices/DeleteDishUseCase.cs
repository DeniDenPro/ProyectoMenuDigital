using Application.Exceptions;
using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Interfaces.IDish.IDishService;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrderItem;
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
    public class DeleteDishUseCase : IDeleteDishUseCase
    {
        private readonly IDishCommand _command;
        private readonly IDishQuery _query;
        private readonly ICategoryQuery _categoryQuery;
        private readonly IOrderItemQuery _orderItemQuery;
        private readonly IOrderQuery _orderQuery;
        public DeleteDishUseCase(IDishCommand command, IDishQuery query, ICategoryQuery categoryQuery, IOrderItemQuery orderItemQuery, IOrderQuery orderQuery)
        {
            _command = command;
            _query = query;
            _categoryQuery = categoryQuery;
            _orderItemQuery = orderItemQuery;
            _orderQuery = orderQuery;
        }

        public async Task<DishResponse> DeleteDish(Guid id)
        {
            var dish = await _query.GetDishById(id);
            if (dish == null)
            {
                throw new NotFoundException($"Dish with ID {id} not found.");
            }
            bool usedInOrders = await _orderQuery.IsDishInActiveOrder(id);
            if (usedInOrders)
            {
                throw new ConflictException($"Dish with ID {id} cannot be deleted because it is used in existing orders.");
            }
            dish.Available = false; // Set the dish as inactive before deletion
            await _command.UpdateDish(dish);
            return new DishResponse
            {
                Id = id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse { Id = dish.CategoryId, Name = dish.Category.Name },
                isActive = dish.Available,
                ImageUrl = dish.ImageUrl,
                createdAt = dish.CreateDate,
                updateAt = dish.UpdateDate

            };
        }
    }
}
