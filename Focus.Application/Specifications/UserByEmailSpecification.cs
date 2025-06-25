using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Application.Specifications;

public class UserByEmailSpecification : BaseSpecification<User>
{
    public UserByEmailSpecification(string email)
        : base(user => user.Email == email)
    {
    }
}