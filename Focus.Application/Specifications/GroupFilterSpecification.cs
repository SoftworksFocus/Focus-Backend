using Focus.Domain.Entities;
using Focus.Domain.Specifications;
using LinqKit;
using System.Linq.Expressions;

namespace Focus.Application.Specifications
{
    public class GroupFilterSpecification : BaseSpecification<Group>
    {
        public GroupFilterSpecification(string? name, string? description)
            : base(BuildPredicate(name, description))
        {
        }

        private static Expression<Func<Group, bool>> BuildPredicate(string? name, string? description)
        {
            var predicate = PredicateBuilder.New<Group>(true);
            if (!string.IsNullOrEmpty(name))
            {
                predicate = predicate.And(g => g.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(description))
            {
                predicate = predicate.And(g => g.Description!.Contains(description)); // Description cannot be null, see the Create method in Group Services
            }
            return predicate;
        }
    }
}
