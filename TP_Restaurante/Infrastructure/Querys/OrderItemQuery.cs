using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.IOrderItem;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Querys
{
    public class OrderItemQuery : IOrderItemQuery
    {
        private readonly AppDbContext _context;
        public OrderItemQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<OrderItem?> GetOrderItemById(int id)
        {
            return await _context.OrderItems.FindAsync(id);
        }
        public async Task<List<OrderItem>> GetAllOrderItems()
        {
            return await _context.OrderItems.ToListAsync();
        }
    }
}
