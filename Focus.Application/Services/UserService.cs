using Focus.Application.DTO.Common;
using System.Security.Cryptography;
using Focus.Application.DTO.User;
using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;
using Focus.Infra.Repositories.Interfaces;

namespace Focus.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public UserService(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task<GetUserDto?> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }

        return GetUserDto.FromUser(user);
    }

    public async Task Add(CreateUserDto userDto)
    {
        if (userDto == null)
        {
            throw new ArgumentNullException(nameof(userDto), "User cannot be null.");
        }

        var user = userDto.ToUser();
        user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        user.IsEmailVerified = false;
        user.EmailVerificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        user.EmailVerificationTokenExpiresAt = DateTime.UtcNow.AddHours(24);
        if (!await _userRepository.AddAsync(user))
        {
            throw new Exception("Failed to add user.");
        }

        if (user.EmailVerificationToken != null)
        {
            await _emailService.SendVerificationEmail(user.Email, user.EmailVerificationToken);
        }
    }

    public async Task<PagedResultDto<GetUserDto>> GetAllAsync(ISpecification<User> filterSpec, int pageNumber,
        int pageSize)
    {
        var totalCount = await _userRepository.CountAsync(filterSpec);
        var users = await _userRepository.ListAsync(filterSpec, pageNumber, pageSize);
        var userDtos = users.Select(GetUserDto.FromUser).ToList();
        return new PagedResultDto<GetUserDto>(userDtos, totalCount, pageNumber, pageSize);
    }

    public async Task Update(int id, UpdateUserDto newUserDto)
    {
        if (newUserDto == null)
        {
            throw new ArgumentNullException(nameof(newUserDto), "User DTO cannot be null.");
        }

        var userToUpdate = await _userRepository.GetByIdAsync(id);
        if (userToUpdate == null)
        {
            throw new KeyNotFoundException($"User not found on service.");
        }

        userToUpdate.Username = newUserDto.Username;
        userToUpdate.Email = newUserDto.Email;
        userToUpdate.Description = newUserDto.Description;

        await _userRepository.UpdateAsync(id, userToUpdate);
    }

    public async Task Delete(int id)
    {
        if (!await _userRepository.DeleteAsync(id))
        {
            throw new Exception($"Failed to delete user.");
        }
    }

    public async Task UpdateProfilePicture(int userId, string mediaUrl)
    {
        if (string.IsNullOrEmpty(mediaUrl))
        {
            throw new ArgumentNullException(nameof(mediaUrl), "Media URL cannot be null or empty.");
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User not found.");
        }

        user.ProfilePictureUrl = mediaUrl;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(userId, user);
    }
}