using Microsoft.AspNetCore.Components.Authorization;

namespace KawanLamaKhairilIlham.Services.Interfaces
{
    public interface IAuthenticationStateProvider
    {
        Task<AuthenticationState> GetAuthenticationStateAsync();
    }

}
