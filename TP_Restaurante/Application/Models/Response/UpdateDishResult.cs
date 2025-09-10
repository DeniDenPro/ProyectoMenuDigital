using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models.Response
{
    public class UpdateDishResult
    {
        public bool Success { get; set; }
        public bool NotFound { get; set; }
        public bool NameConflict { get; set; }
        public Dish? UpdatedDish { get; set; }
    }
}
