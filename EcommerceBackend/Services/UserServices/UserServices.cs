using AutoMapper;
using EcommerceBackend.Context;
using EcommerceBackend.Dto;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserServices(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserViewDto>> ListUsers()
        {
            try
            {
                var users = await _context.users
                    .Where(c => c.Role != "Admin").ToListAsync();
                var user = _mapper.Map<List<UserViewDto>>(users);

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserViewDto> GetUser(int id)
        {
            try
            {
                var user = await _context.users
                    .Where(c => c.Role != "Admin").FirstOrDefaultAsync(a => a.Id == id);
                var use = _mapper.Map<UserViewDto>(user);

                return use;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BlockUnblockDto> BlockUnblockUser(int id)
        {
            var user = await _context.users.FirstOrDefaultAsync(a => a.Id == id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.IsBlocked = !user.IsBlocked;
            await _context.SaveChangesAsync();

            return new BlockUnblockDto
            {
                isBlocked = user.IsBlocked,
                Msg = user.IsBlocked ? "User is blocked" : "User unblocked"
            };
        }
    }
}

