using KawanLamaKhairilIlham.Data;
using KawanLamaKhairilIlham.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserService> _logger;

    public UserService(AppDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<UserService> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public void SaveUserIdToCookie(int userId)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            try
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.Now.AddMinutes(30)
                };

                context.Response.Cookies.Append("UserId", userId.ToString(), cookieOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Terjadi kesalahan saat menyimpan ke cookie.");
            }
        }
        else
        {
            _logger.LogWarning("HttpContext null, tidak dapat menyimpan ke cookie.");
        }
    }


    public int? GetUserIdFromCookie()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            try
            {
                if (context.Request.Cookies.TryGetValue("UserId", out var userIdString))
                {
                    if (int.TryParse(userIdString, out var userId))
                    {
                        return userId;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat membaca dari cookie.");
                return null;
            }
        }
        else
        {
            _logger.LogWarning("HttpContext null, tidak dapat membaca dari cookie.");
            return null;
        }
    }

    public async Task<UserData?> GetCurrentUserAsync()
    {
        var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name;

        if (string.IsNullOrEmpty(userName))
        {
            _logger.LogWarning("Pengguna tidak terautentikasi.");
            return null;
        }

        try
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kesalahan saat mengambil data pengguna {UserName}", userName);
            return null;
        }
    }

    public async Task<UserData?> GetUserByUserNameAsync(string userName)
    {
        try
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kesalahan saat mengambil data pengguna {UserName}", userName);
            return null;
        }
    }

    public async Task<bool> RegisterUserAsync(string userName, string fullName, string password)
    {
        _logger.LogInformation("Memulai proses pendaftaran untuk pengguna {UserName}", userName);

        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);

        if (existingUser != null)
        {
            _logger.LogWarning("Pendaftaran gagal: Pengguna {UserName} sudah ada.", userName);
            return false;
        }

        var user = new UserData
        {
            UserName = userName,
            FullName = fullName,
            PasswordHash = HashPassword(password)
        };

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _logger.LogInformation("Menambahkan pengguna baru {UserName} ke database", userName);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation("Pengguna {UserName} berhasil didaftarkan.", userName);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Kesalahan saat mendaftarkan pengguna {UserName}", userName);
            throw;
        }

        return true;
    }

    public async Task<bool> ValidateUserAsync(string userName, string password)
    {
        try
        {
            var user = await GetUserByUserNameAsync(userName);
            if (user == null)
            {
                _logger.LogWarning("Validasi gagal: Pengguna {UserName} tidak ditemukan.", userName);
                return false;
            }

            var isValid = VerifyPassword(password, user.PasswordHash);
            if (isValid)
            {
                //SaveUserIdToCookie(user.Id); // Pastikan user.Id sudah benar
                _logger.LogInformation("Pengguna {UserName} berhasil divalidasi.", userName);
            }
            else
            {
                _logger.LogWarning("Validasi gagal: Kata sandi salah untuk pengguna {UserName}.", userName);
            }

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kesalahan saat memvalidasi pengguna {UserName}", userName);
            return false;
        }
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
