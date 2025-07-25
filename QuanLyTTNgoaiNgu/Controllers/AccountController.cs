using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using QuanLyTTNgoaiNgu.Data;
using System.Linq;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace QuanLyTTNgoaiNgu.Controllers
{

    [NoCache]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
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
            bool rememberMe,
            string returnUrl = null)
        {
            var user = _ctx.TAIKHOAN.FirstOrDefault(u => u.TenDangNhap == username && u.MatKhau == password);
            if (user == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                return View();
            }

            // Xác định họ tên
            string hoTen = user.TenDangNhap; // mặc định
            if (user.VaiTro == "GiangVien")
            {
                var gv = _ctx.GIANGVIEN.FirstOrDefault(g => g.MaTaiKhoan == user.MaTaiKhoan);
                if (gv != null) hoTen = gv.HoTen;
            }
            else if (user.VaiTro == "HocVien")
            {
                var hv = _ctx.HOCVIEN
    .Include(h => h.DANGKYMOI)
    .FirstOrDefault(h => h.MaTaiKhoan == user.MaTaiKhoan);

                if (hv != null && hv.DANGKYMOI != null) hoTen = hv.DANGKYMOI.HoTen;

            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim(ClaimTypes.Role, user.VaiTro),
                new Claim("FullName", hoTen),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var props = new AuthenticationProperties { IsPersistent = rememberMe };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                props
            );

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return user.VaiTro switch
            {
                "Admin" => RedirectToAction("AdminHome", "Home"),
                "GiangVien" => RedirectToAction("GiangVienHome", "Home"),
                "HocVien" => RedirectToAction("HocVienHome", "Home"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        [HttpGet]
        public IActionResult QuenMatKhau()
        {
            return View();
        }

        [HttpPost]
        public IActionResult QuenMatKhau(string username, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ tên đăng nhập và email!";
                return View();
            }

            var taikhoan = _ctx.TAIKHOAN.FirstOrDefault(t => t.TenDangNhap == username);
            if (taikhoan == null)
            {
                ViewBag.Error = "Không tìm thấy tài khoản này!";
                return View();
            }

            var hocvien = _ctx.HOCVIEN
                .Where(hv => hv.MaTaiKhoan == taikhoan.MaTaiKhoan)
                .FirstOrDefault(hv => hv.DANGKYMOI != null && hv.DANGKYMOI.Email == email);

            if (hocvien == null)
            {
                ViewBag.Error = "Email không khớp với tài khoản!";
                return View();
            }

            taikhoan.MatKhau = "111111";
            _ctx.Update(taikhoan);
            _ctx.SaveChanges();

            ViewBag.Message = "Khôi phục mật khẩu thành công! Mật khẩu mới là: 111111";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Ngăn trình duyệt lưu cache
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToAction("Login");
        }

        public IActionResult LoggedOut()
        {
            return View(); // Trang thông báo "Bạn đã đăng xuất"
        }

    }
}
