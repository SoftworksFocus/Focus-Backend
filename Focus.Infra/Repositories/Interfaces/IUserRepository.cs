using Focus.Domain.Entities;

namespace Focus.Infra.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByCredentialsMock(string email, string password);
    
}
