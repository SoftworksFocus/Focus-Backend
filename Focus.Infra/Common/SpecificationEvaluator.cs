using Microsoft.EntityFrameworkCore;
using Focus.Domain.Specifications;

namespace Focus.Infra.Common
{
    public static class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T>? spec)
        {
            var query = inputQuery;

            query = query.Where(spec!.Criteria);

            query = spec.Includes.Aggregate(query, 
                (current, include) => current.Include(include));
            return query;
        }
    }
}