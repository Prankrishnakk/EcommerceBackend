using AutoMapper;
using EcommerceBackend.ApiResponse;
using EcommerceBackend.Context;
using EcommerceBackend.Dto;
using EcommerceBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EcommerceBackend.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetCategories()
        {
            var getall = await _context.categories.ToListAsync();

            var res = _mapper.Map<List<CategoryDto>>(getall);
            return res;
        }


        public async Task<ApiResponse<CatAddDto>> AddCategory(CatAddDto Addcategories)
        {
            var IsExist = await _context.categories.FirstOrDefaultAsync(a => a.CategoryName == Addcategories.CategoryName);
            if (IsExist != null)
            {
                return new ApiResponse<CatAddDto>(false, "category already exists", null, "add another category");
            }

            var res = _mapper.Map<Category>(Addcategories);
            _context.categories.Add(res);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<CatAddDto>(res);

            return new ApiResponse<CatAddDto>(true, "new category added to database", response, null);

        }


        public async Task<ApiResponse<string>> RemoveCategory(int id)
        {
            try
            {
                var res = await _context.categories.FirstOrDefaultAsync(a => a.Id == id);
                var pro = await _context.products.Where(a => a.CategoryId == id).ToListAsync();

                if (res == null)
                {

                    return new ApiResponse<string>(false, "Category not found", null, "Please check the category ID and try again");
                }
                else
                {
                    _context.products.RemoveRange(pro);
                    _context.categories.Remove(res);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<string>(true, "done", "category deleted", null);

                }


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured while saving changes:{ex.InnerException?.Message ?? ex.Message} ");
            }
        }

                
    
    }
}