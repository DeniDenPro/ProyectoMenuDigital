using Application.Enums;
using Application.Interfaces.IDish;
using Application.Models.Response;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Infrastructure.Querys
{
    public class DishQuery : IDishQuery
    {
        private readonly AppDbContext _context;
        public DishQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dish>> GetAllAsync(string? name = null, int? categoryId = 0, OrderPrice? priceOrder = OrderPrice.ASC, bool? onlyActive = null)
        {
            var query = _context.Dishes.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(d => d.Name.Contains(name));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(d => d.CategoryId == categoryId.Value);
            }

            switch (priceOrder)
            {
                case OrderPrice.ASC:
                    query = query.OrderBy(d => d.Price);
                    break;
                case OrderPrice.DESC:
                    query = query.OrderByDescending(d => d.Price);
                    break;
                default:
                    throw new InvalidOperationException("Valor de ordenamiento inválido");

            }
            switch (onlyActive)
            {
                case true:
                    query = query.Where(d => d.Available == true);
                    break;
            //    case false:
            //        query = query.Where(d => d.Available == false);
            //        break;
                default:
                    break;
            }

            return await query
            .Include(d => d.Category)
            .ToListAsync();
        }

        public async Task<List<Dish>> GetAllDishes()
        {
            return await _context.Dishes.ToListAsync();
        }
        public async Task<Dish?> GetDishById(Guid id)
        {
            return await _context.Dishes
            .Include(d => d.Category)
            .FirstOrDefaultAsync(d => d.DishId == id);
        }

        public async Task<bool> DishExists(string name, Guid? id)
        {
            var query = _context.Dishes.AsQueryable();

            if (id.HasValue)
            {
              
                query = query.Where(d => d.DishId != id.Value);
            }

            return await query.AnyAsync(d => d.Name == name);
        }
        public async Task<List<Dish>> GetDishesByIds(IEnumerable<Guid> ids)
        {
            return await _context.Dishes
            .Where(dish => ids.Contains(dish.DishId) && dish.Available == true)
            .ToListAsync();
        }
    }
}
