using LinqKit;
using System.Linq.Expressions;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Application.Specifications
{
    public class UserFilterSpecification : BaseSpecification<User>
    {
        public UserFilterSpecification(string? username, string? email)
            : base(BuildPredicate(username, email))
        {
        }   

        private static Expression<Func<User, bool>> BuildPredicate(string? username, string? email)
        {
            var predicate = PredicateBuilder.New<User>(true);
            if (!string.IsNullOrEmpty(username))
            {
                predicate = predicate.And(u => u.Username.Contains(username));
            }
            if (!string.IsNullOrEmpty(email))
            {
                predicate = predicate.And(u => u.Email.Contains(email));
            }
            return predicate;
        }
    }
}