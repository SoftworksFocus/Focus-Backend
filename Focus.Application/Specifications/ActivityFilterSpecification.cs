using Focus.Domain.Entities;
using Focus.Domain.Specifications;
using LinqKit;
using System.Linq.Expressions;

namespace Focus.Application.Specifications
{
    public class ActivityFilterSpecification : BaseSpecification<Activity>
    {
        public ActivityFilterSpecification(int? ownerId, string? ownerUsername, int? groupId)
            : base(BuildPredicate(ownerId, ownerUsername, groupId))
        {
            AddInclude(a => a.User);
            AddInclude(a => a.Group);
            AddInclude(a => a.Media);
        }

        // public ActivityFilterSpecification(int ownerId) : base(BuildPredicate(ownerId))
        // {
        //     throw new NotImplementedException();
        // }

        private static Expression<Func<Activity, bool>> BuildPredicate(int ownerId)
        {
            var predicate = PredicateBuilder.New<Activity>(true);
            if (ownerId > 0)
            {
                predicate = predicate.And(a => a.UserId == ownerId);
            }
            return predicate;
        }

        private static Expression<Func<Activity, bool>> BuildPredicate(int? ownerId, string? ownerUsername, int? groupId)
        {
            var predicate = PredicateBuilder.New<Activity>(true);
            if (ownerId.HasValue)
            {
                predicate = predicate.And(a => a.UserId == ownerId.Value);
            }

            if (!string.IsNullOrEmpty(ownerUsername))
            {
                predicate = predicate.And(a => a.User.Username.Contains(ownerUsername));
            }
            if (groupId.HasValue)
            {
                predicate = predicate.And(a => a.GroupId == groupId.Value);
            }
            return predicate;
        }
    }
}
