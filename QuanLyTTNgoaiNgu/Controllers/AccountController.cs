using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using QuanLyTTNgoaiNgu.Data;
using System.Linq;
using System.Security.Claims;
using System;
using System.Threading.Tasks;

namespace QuanLyTTNgoaiNgu.Controllers
{
    public class AccountController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _ctx;
        public AccountController(QuanLyTTNgoaiNguContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(
            string username,
            string password,
            bool rememberMe,         // <- bind checkbox
            string returnUrl = null)
        {
            var user = _ctx.TAIKHOAN
                .FirstOrDefault(u => u.TenDangNhap == username && u.MatKhau == password);
            if (user == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                return View();
            }

            // Tạo claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim(ClaimTypes.Role, user.VaiTro)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Chỉ cần IsPersistent: nếu true thì cookie sẽ dùng ExpireTimeSpan từ Program.cs
            var props = new AuthenticationProperties
            {
                IsPersistent = rememberMe
            };

            // Sign in
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                props
            );

            // Redirect về returnUrl nếu có
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // Ngược lại điều hướng theo role
            return user.VaiTro switch
            {
                "Admin" => RedirectToAction("AdminHome", "Home"),
                "GiangVien" => RedirectToAction("GiangVienHome", "Home"),
                "HocVien" => RedirectToAction("HocVienHome", "Home"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
            => View();
    }
}
