using LinqKit;
using System.Linq.Expressions;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Focus.Application.Specifications
{
    public class FeedActivitySpecification : BaseSpecification<Activity>
    {
        public FeedActivitySpecification(int currentUserId, List<int> memberGroupIds, DateTime? cursor)
            : base(BuildPredicate(currentUserId, memberGroupIds, cursor))
        {
            AddInclude(a => a.User);
            AddInclude(a => a.Group);
            AddInclude(a => a.Media);
            ApplyOrderByDescending(a => a.CreatedAt);
        }

        private static Expression<Func<Activity, bool>> BuildPredicate(int currentUserId, List<int> memberGroupIds, DateTime? cursor)
        {
            var predicate = PredicateBuilder.New<Activity>(false);
            predicate = predicate.Or(a => a.UserId == currentUserId);
            if (memberGroupIds.Any())
            {
                predicate = predicate.Or(a => a.GroupId.HasValue && memberGroupIds.Contains(a.GroupId.Value));
            }
            if (cursor.HasValue)
            {
                predicate = predicate.And(a => a.CreatedAt < cursor.Value);
            }
            return predicate;
        }
    }
}