using System.Linq.Expressions;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;
using LinqKit;

namespace Focus.Application.Specifications;

public class UserByEmailSpecification : BaseSpecification<User>
{
    public UserByEmailSpecification(string email)
        : base(BuildPredicate(email))
    {
    }
    
    private static Expression<Func<User, bool>> BuildPredicate(string? email)
    {
        var predicate = PredicateBuilder.New<User>(true);
        if (!string.IsNullOrEmpty(email))
        {
            predicate = predicate.And(u => u.Email.Contains(email));
        }
        return predicate;
    }
    
}