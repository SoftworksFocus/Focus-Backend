using Focus.Domain.Specifications;
using Focus.Infra.Common;
using Focus.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Focus.Infra.Repositories;
using Focus.Domain.Entities;

public class UserRepository : IUserRepository
{
    private readonly FocusDbContext _context;
    public UserRepository(FocusDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<User>> ListAsync(ISpecification<User>? spec = null)
    {
        return await ApplySpecification(spec).ToListAsync();
    }
    
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task UpdateAsync(int id, User updatedUser)
    {
        var existingUser = await _context.Users.FindAsync(id);

        if (existingUser != null)
        {
            existingUser.Username = updatedUser.Username;
            existingUser.Email = updatedUser.Email;
            existingUser.Description = updatedUser.Description;
            existingUser.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new KeyNotFoundException($"Usuário com o id {id} não foi encontrado para atualização.");
        }
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }
        _context.Users.Remove(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<User?> GetFirstOrDefaultAsync(ISpecification<User> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    private IQueryable<User> ApplySpecification(ISpecification<User> spec)
    {
        return SpecificationEvaluator<User>.GetQuery(_context.Users.AsQueryable(), spec);
    }

    public async Task<User> GetByCredentialsMock(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

        return user!;
    }

}