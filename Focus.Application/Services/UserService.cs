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

    public Task Update(int id, UpdateUserDto userDto)
    {
        throw new NotImplementedException("Update method is not implemented yet.");
    }
    
    public async Task UpdateAsync(int userId, int requesterId, UpdateUserDto newUserDto)
    {
        if (newUserDto == null)
        {
            throw new ArgumentNullException(nameof(newUserDto), "User DTO cannot be null.");
        }
        var userToUpdate = await _userRepository.GetByIdAsync(userId);
        if (userToUpdate == null)
        {
            throw new KeyNotFoundException($"User not found on service.");
        }
        if (!await IsSameUser(requesterId, userId))
        {
            throw new UnauthorizedAccessException("You can only update your own profile.");
        }
        userToUpdate.Username = newUserDto.Username;
        userToUpdate.Email = newUserDto.Email;
        userToUpdate.Description = newUserDto.Description;
        await _userRepository.UpdateAsync(userId, userToUpdate);
    }

    public Task Delete(int userId)
    {
        throw new NotImplementedException("Delete method is not implemented yet.");
    }
    
    public async Task DeleteAsync(int userId, int requesterId)
    {
        if (!await IsSameUser(requesterId, userId))
        {
            throw new UnauthorizedAccessException("You can only delete your own account.");
        }
        if (!await _userRepository.DeleteAsync(userId))
        {
            throw new Exception($"Failed to delete user.");
        }
    }

    public async Task UpdateProfilePicture(int userId, int requesterId, string mediaUrl)
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
        if (!await IsSameUser(requesterId, userId))
        {
            throw new UnauthorizedAccessException("You can only update your own profile picture.");
        }
        user.ProfilePictureUrl = mediaUrl;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(userId, user);
    }
    
    private async Task<bool> IsSameUser(int requesterId, int userId)
    {
        var existingUser = await _userRepository.GetByIdAsync(userId);
        return existingUser != null && existingUser.Id == requesterId;
    }
}