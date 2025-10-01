using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data;
using Application.Interfaces.IStatus;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Querys
{
    public class StatusQuery : IStatusQuery
    {
        private readonly AppDbContext _context;
        public StatusQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Status>> GetAllStatuses()
        {
            var statuses = await _context.Statuses.ToListAsync();
            return statuses;
        }
        public async Task<Status?> GetStatusById(int id)
        {
            return await _context.Statuses.FindAsync(id);
        }
        public async Task<bool> StatusExists(int id)
        {
            return await _context.Statuses.AnyAsync(s => s.Id == id);
        }
    }
}
