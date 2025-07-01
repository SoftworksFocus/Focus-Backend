using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Infra.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByCredentialsMock(string email, string password);
}
