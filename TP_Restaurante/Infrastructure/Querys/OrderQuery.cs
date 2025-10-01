using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.IOrder;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Querys
{
    public class OrderQuery : IOrderQuery
    {
        private readonly AppDbContext _context;
        public OrderQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Order?> GetOrderById(long id)
        {
            return await _context.Orders
                        .Include(o => o.OrderItems)
                        .FirstOrDefaultAsync(o => o.OrderId == id);

        }
        public async Task<Order?> GetFullOrderById(long orderId)
        {
            return await _context.Orders
                .Include(o => o.OverallStatus)
                .Include(o => o.DeliveryType)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.StatusId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }
        public async Task<IEnumerable<Order?>> GetOrderFechaStatus(DateTime? from, DateTime? to, int? statusid)
        {
            var query = _context.Orders
                 .Include(o => o.OverallStatus)
                 .Include(o => o.DeliveryType)
                 .Include(o => o.OrderItems)
                 .ThenInclude(oi => oi.Dish)
                 .Include(o => o.OrderItems)
                 .ThenInclude(oi => oi.Status)
             .AsNoTracking().AsQueryable();
            if (from.HasValue)
            {
                query = query.Where(o => o.CreateDate >= from.Value);
            }
            if (to.HasValue)
            {
                query = query.Where(o => o.CreateDate <= to.Value);
            }
            if (statusid.HasValue)
            {
                query = query.Where(o => o.StatusId == statusid.Value);
            }
            return await query.ToListAsync();
        }
        public async Task<bool> IsDishInActiveOrder(Guid dishId)
        {
            var activeStatusIds = new[] { 1, 2 };

            return await _context.OrderItems
                .AnyAsync(orderItem =>
                    orderItem.DishId == dishId &&
                    activeStatusIds.Contains(orderItem.Order.OverallStatus.Id)
                );

        }
    }
}
