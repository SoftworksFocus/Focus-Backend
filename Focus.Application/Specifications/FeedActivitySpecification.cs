using LinqKit;
using System.Linq.Expressions;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace Focus.Application.Specifications
{
    public class FeedActivitySpecification : BaseSpecification<Activity>
    {
        public FeedActivitySpecification(int currentUserId, List<int> memberGroupIds)
            : base(BuildPredicate(currentUserId, memberGroupIds))
        {
            AddInclude(a => a.User);
            AddInclude(a => a.Group);
            AddInclude(a => a.Media);
            ApplyOrderByDescending(a => a.CreatedAt);
        }

        private static Expression<Func<Activity, bool>> BuildPredicate(int currentUserId, List<int> memberGroupIds)
        {
            var predicate = PredicateBuilder.New<Activity>(false);
            predicate = predicate.Or(a => a.UserId == currentUserId);
            if (memberGroupIds.Any())
            {
                predicate = predicate.Or(a => a.GroupId.HasValue && memberGroupIds.Contains(a.GroupId.Value));
            }
            return predicate;
        }

        public Expression<Func<Activity, object>> OrderByDescending { get; private set; }
        protected void ApplyOrderByDescending(Expression<Func<Activity, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }
    }
}