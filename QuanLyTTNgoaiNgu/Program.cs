using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using QuanLyTTNgoaiNgu.Data;



var builder = WebApplication.CreateBuilder(args);

// 1. Dùng EphemeralDataProtectionProvider để key KHÔNG được lưu ra file
builder.Services.AddDataProtection()
    .UseEphemeralDataProtectionProvider();

// 2. Đăng ký DbContext như trước
builder.Services.AddDbContext<QuanLyTTNgoaiNguContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QuanLyTTNgoaiNguContext")));

// 3. MVC
builder.Services.AddControllersWithViews();

// 4. Cấu hình Cookie Authentication
builder.Services
  .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options =>
  {
      options.LoginPath = "/Account/Login";
      options.AccessDeniedPath = "/Account/AccessDenied";
      // persistent cookie 10 năm, session cookie vẫn là session
      options.ExpireTimeSpan = TimeSpan.FromDays(3650);
      options.SlidingExpiration = true;
  });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
    options.AddPolicy("GiangVienOnly", p => p.RequireRole("GiangVien"));
    options.AddPolicy("HocVienOnly", p => p.RequireRole("HocVien"));
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
