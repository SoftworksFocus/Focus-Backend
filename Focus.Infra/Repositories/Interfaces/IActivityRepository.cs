using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Infra.Repositories.Interfaces;

public interface IActivityRepository : IRepository<Activity>
{
    Task<List<Activity>> ListAsync(ISpecification<Activity> spec, int take);
    Task<Activity?> AddActivityAsync(Activity activity);
}