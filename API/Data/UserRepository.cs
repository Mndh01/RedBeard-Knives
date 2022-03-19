using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<MemberDto> GetMemberAsync(int id)
        {            
            return await _context.Users
                .Include(u => u.UserAddresses)
                .ThenInclude(ua => ua.Address)
                .Where(x => x.Id == id)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();                
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {            
            return await _context.Users
                .Include(u => u.Photo)
                .Include(u => u.UserAddresses)
                .ThenInclude(ua => ua.Address)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();                
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(p => p.Photo)
                .Include(ua => ua.UserAddresses)
                .ThenInclude(ua => ua.Address)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photo)
                .Include(a => a.UserAddresses)
                .ThenInclude(ua => ua.Address)
                .FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(p => p.Photo)
                .Include(ua => ua.UserAddresses)
                .ThenInclude(ua => ua.Address)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photo)
                .Include(a => a.UserAddresses)
                .ThenInclude(ua => ua.Address)
                .ToListAsync();            
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
                
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}