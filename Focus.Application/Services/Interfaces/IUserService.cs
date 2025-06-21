using Focus.Application.DTO.User;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Application.Services.Interfaces;

public interface IUserService : IService<User, GetUserDto, CreateUserDto, UpdateUserDto>;