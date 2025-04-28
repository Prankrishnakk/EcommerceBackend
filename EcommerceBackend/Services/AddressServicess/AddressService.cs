using AutoMapper;
using EcommerceBackend.Context;
using EcommerceBackend.Dto;
using EcommerceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Services.AddressServicess
{
    public class AddressService : IAddressService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AddressService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> AddnewAddress(int userId, AddNewAddressDto address) 
        {
            try 
            {
                if (address == null) 
                {
                    throw new ArgumentNullException(nameof(address), "Address information required");
                }

                var user = await _context.users.FindAsync(userId);
                if (user == null) 
                {
                    throw new Exception("User not found");
                }

                var Addnew = _mapper.Map<UserAddress>(address);
                Addnew.UserId = userId;

                _context.userAddresses.Add(Addnew);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while saving changes: {ex.InnerException?.Message ?? ex.Message}");

            }
        }
        public async Task<List<GetAddressDto>> GetAddress(int userId) 
        {
            try
            {
                var user = await _context.users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var address = await _context.userAddresses.Where(a => a.UserId == userId).ToListAsync();
                return _mapper.Map<List<GetAddressDto>>(address);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while saving changes: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
        public async Task<bool> RemoveAddress(int addId) 
        {
            try 
            {
                var delete = await _context.userAddresses.FirstOrDefaultAsync(a => a.Id == addId);
                if (delete == null)
                {
                    return false;
                }
                _context.userAddresses.Remove(delete);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }
        
    }
}
