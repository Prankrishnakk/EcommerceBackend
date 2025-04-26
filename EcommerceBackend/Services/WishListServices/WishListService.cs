using EcommerceBackend.ApiResponse;
using EcommerceBackend.Context;
using EcommerceBackend.Dto;
using EcommerceBackend.Models;
using EcommerceBackend.Services.WishListServices;
using Microsoft.EntityFrameworkCore;

public class WishListServices : IWishListService
{
    private readonly AppDbContext _context;
    public WishListServices(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<string>> AddOrRemove(int u_id, int pro_id)
    {
        try
        {
            var isExists = await _context.wishLists
                .Include(a => a._Products)
                .FirstOrDefaultAsync(b => b.ProductId == pro_id && b.UserId == u_id);

            if (isExists == null)
            {
                var add_wish = new WishList
                {
                    UserId = u_id,
                    ProductId = pro_id,
                };

                _context.wishLists.Add(add_wish);
                await _context.SaveChangesAsync();
                return new ApiResponse<string>(true, "Item added to the wishList", "done", null);
            }
            else
            {
                _context.wishLists.Remove(isExists);
                await _context.SaveChangesAsync();

                return new ApiResponse<string>(true, "Item removed from wishList", "done", null);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while updating the wishlist.", ex);
        }
    }


    public async Task<List<WishListViewDto>> GetAllWishItems(int u_id)
    {
        try
        {
            var items = await _context.wishLists
                .Include(a => a._Products)
                .ThenInclude(b => b._Category)
                .Where(c => c.UserId == u_id)
                .ToListAsync();

            if (items != null)
            {
                var p = items.Select(a => new WishListViewDto
                {
                    Id = a.Id,
                    ProductId = a._Products.Id,
                    ProductName = a._Products.ProductName,
                    ProductDescription = a._Products.ProductDescription,
                    Price = a._Products.ProductPrice,
                    OfferPrice = a._Products.offerPrize,
                    ProductImage = a._Products.ImageUrl,
                    CategoryName = a._Products._Category?.CategoryName
                }).ToList();

                return p;
            }
            else
            {
                return new List<WishListViewDto>();
            }

        }

        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }


    public async Task<ApiResponse<string>> RemoveFromWishList(int u_id, int pro_id)
    {
        try
        {
            var isExists = await _context.wishLists
              .Include(a => a._Products)
              .FirstOrDefaultAsync(b => b.Id == pro_id && b.UserId == u_id);

            if (isExists != null)
            {
                _context.wishLists.Remove(isExists);
                await _context.SaveChangesAsync();
                return new ApiResponse<string>(true, "Item removed from wishList", "done", null);
            }

            return new ApiResponse<string>(false, "Product not found", "", null);


        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }




}