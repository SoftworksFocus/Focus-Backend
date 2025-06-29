using Focus.Domain.Entities;

namespace Focus.Infra.Repositories.Interfaces;

public interface IMediaRepository
{
    Task AddAsync(Media media);
}