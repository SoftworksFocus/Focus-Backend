using Focus.Domain.Entities;

namespace Focus.Infra.Repositories.Interfaces;

public interface IUserTokenRepository
{
    Task AddAsync(UserToken userToken);
    Task<UserToken?> GetByRefreshTokenHashAsync(string refreshTokenHash);
    Task UpdateAsync(UserToken userToken);
}