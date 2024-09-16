using KawanLamaKhairilIlham.Data;

namespace KawanLamaKhairilIlham.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(string userName, string fullName, string password);
        Task<UserData> GetUserByUserNameAsync(string userName);
        Task<bool> ValidateUserAsync(string userName, string password);
        string HashPassword(string password);
    }

}
