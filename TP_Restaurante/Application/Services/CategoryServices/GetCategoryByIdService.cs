using Application.Interfaces.ICategory.ICategoryServices;
using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.CategoryServices
{
    public class GetCategoryByIdService : IGetCategoryByIdService
    {
        public Task<CategoryResponse> GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
