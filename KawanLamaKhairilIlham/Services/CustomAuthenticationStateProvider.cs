using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using KawanLamaKhairilIlham.Services.Interfaces;

namespace KawanLamaKhairilIlham.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IUserService _userService;

        public CustomAuthenticationStateProvider(IUserService userService)
        {
            _userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = await _userService.GetCurrentUserAsync();
            var identity = user != null
                ? new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserName) }, "custom")
                : new ClaimsIdentity();

            var userPrincipal = new ClaimsPrincipal(identity);
            return new AuthenticationState(userPrincipal);
        }
    }
}
