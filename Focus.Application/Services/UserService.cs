using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Infra.Repositories;

namespace Focus.Application.Services;



public class UserService : IUserService
{
    private readonly UserRepository _userrepository;

    UserService(UserRepository userrepository)
    {
        _userrepository = userrepository;
    }


    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _userrepository.GetByIdAsync(id);


        return user;
    }
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await  _userrepository.GetAllAsync();
        
        return users;
    }
    
    
}