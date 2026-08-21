// AuthService.cs
using ClinicManagement.Shared.DTOs;
using ClinicManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicManagementApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly ClinicDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ClinicDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            if (loginDto.UserType == "Staff")
            {
                // Check Users table
                var user = await _context.Users.SingleOrDefaultAsync(u => u.LoginName == loginDto.LoginName && u.IsActive);
                if (user != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    user.LastLogin = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new AuthResponseDto { IsSuccessful = true, Token = GenerateJwtToken(user.Id.ToString(), user.Name, user.Role) };
                }
            }
            else if (loginDto.UserType == "Doctor")
            {
                // Check Doctors table
                var doctor = await _context.Doctors.SingleOrDefaultAsync(d => d.LoginName == loginDto.LoginName && d.IsActive);
                if (doctor != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, doctor.PasswordHash))
                {
                    return new AuthResponseDto { IsSuccessful = true, Token = GenerateJwtToken(doctor.Id.ToString(), $"Dr. {doctor.FirstName} {doctor.LastName}", "Doctor") };
                }
            }

            return new AuthResponseDto { IsSuccessful = false, ErrorMessage = "Invalid username, password, or login type." };
        }

        private string GenerateJwtToken(string id, string name, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}