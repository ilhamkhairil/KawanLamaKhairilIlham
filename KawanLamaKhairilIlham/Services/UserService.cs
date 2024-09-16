using System;
using System.Threading.Tasks;
using KawanLamaKhairilIlham.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using KawanLamaKhairilIlham.Services.Interfaces;

namespace KawanLamaKhairilIlham.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUserAsync(string userName, string fullName, string password)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (existingUser != null)
                return false; // User already exists

            var user = new UserData
            {
                UserName = userName,
                FullName = fullName,
                PasswordHash = HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserData> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<bool> ValidateUserAsync(string userName, string password)
        {
            var user = await GetUserByUserNameAsync(userName);
            if (user == null)
                return false;

            return user.PasswordHash == HashPassword(password);
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }

}
