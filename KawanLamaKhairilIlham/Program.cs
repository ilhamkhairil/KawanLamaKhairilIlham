using Blazored.SessionStorage;
using KawanLamaKhairilIlham.Data;
using KawanLamaKhairilIlham.Services;
using KawanLamaKhairilIlham.Services.Interfaces;
using KhairilKawanLama.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan layanan HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Tambahkan layanan untuk session
builder.Services.AddDistributedMemoryCache(); // Memerlukan memory cache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Waktu sesi
    options.Cookie.HttpOnly = true; // Hanya Http
    options.Cookie.IsEssential = true; // Cookie penting
});

// Konfigurasi DbContext dengan SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Daftarkan layanan kustom
builder.Services.AddScoped<IUserService, UserService>(); // Daftarkan UserService
builder.Services.AddScoped<IToDoService, ToDoService>(); // Daftarkan ToDoService
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// Konfigurasi autentikasi
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
});

builder.Services.AddAuthorization();

// Konfigurasi logging
builder.Logging.AddConsole();

// Tambahkan Razor Pages dan Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredSessionStorage();

// Tambahkan WeatherForecastService sebagai singleton
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Konfigurasi middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Middleware untuk session harus dipanggil sebelum routing
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
