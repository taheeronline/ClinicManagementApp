// ClinicManagementApp/Services/UserService.cs
using ClinicManagement.Shared.DTOs;
using ClinicManagementApp.Data;
using ClinicManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly ClinicDbContext _context;

        public UserService(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    LoginName = u.LoginName,
                    Address = u.Address,
                    Phone = u.Phone,
                    Email = u.Email,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    LastLogin = u.LastLogin
                })
                .ToListAsync();
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new InvalidOperationException($"User with ID {id} not found.");

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                LoginName = user.LoginName,
                Address = user.Address,
                Phone = user.Phone,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin
            };
        }

        public async Task<UserDto> CreateUserAsync(UserDto userDto)
        {
            if (await _context.Users.AnyAsync(u => u.LoginName == userDto.LoginName) ||
                await _context.Doctors.AnyAsync(d => d.LoginName == userDto.LoginName))
                throw new InvalidOperationException("This Login Username is already taken.");

            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new InvalidOperationException("Password is required when creating a new user.");

            var user = new User
            {
                Name = userDto.Name,
                LoginName = userDto.LoginName,
                Address = userDto.Address,
                Phone = userDto.Phone,
                Email = userDto.Email,
                Role = userDto.Role,
                IsActive = true,
                CreatedAt = DateTime.Now,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            userDto.Id = user.Id;
            return userDto;
        }

        public async Task UpdateUserAsync(int id, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new InvalidOperationException($"User with ID {id} not found.");

            if (await _context.Users.AnyAsync(u => u.LoginName == userDto.LoginName && u.Id != id) ||
                await _context.Doctors.AnyAsync(d => d.LoginName == userDto.LoginName))
                throw new InvalidOperationException("This Login Username is already taken by another user.");

            user.Name = userDto.Name;
            user.LoginName = userDto.LoginName;
            user.Address = userDto.Address;
            user.Phone = userDto.Phone;
            user.Email = userDto.Email;
            user.Role = userDto.Role;
            user.IsActive = userDto.IsActive;

            if (!string.IsNullOrWhiteSpace(userDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new InvalidOperationException($"User with ID {id} not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}