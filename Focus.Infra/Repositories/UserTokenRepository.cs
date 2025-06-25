using Focus.Domain.Entities;
using Focus.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Focus.Infra.Repositories;

    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly FocusDbContext _context;

        public UserTokenRepository(FocusDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserToken userToken)
        {
            await _context.UserTokens.AddAsync(userToken);
            await _context.SaveChangesAsync();
        }

        public async Task<UserToken?> GetByRefreshTokenHashAsync(string refreshTokenHash)
        {
            return await _context.UserTokens
                .Include(ut => ut.User)
                .FirstOrDefaultAsync(ut => ut.RefreshTokenHash == refreshTokenHash);
        }
        
        public async Task UpdateAsync(UserToken userToken)
        {
            _context.UserTokens.Update(userToken);
            await _context.SaveChangesAsync();
        }
    }
