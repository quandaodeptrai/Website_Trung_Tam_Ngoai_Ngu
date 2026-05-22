using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyTTNgoaiNgu.Data;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;


namespace QuanLyTTNgoaiNgu.Controllers
{

    [NoCache]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AccountController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _ctx;
        private readonly IConfiguration _configuration;
        public AccountController(QuanLyTTNgoaiNguContext ctx, IConfiguration configuration)
        {
            _ctx = ctx;
            _configuration = configuration;
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
        public async Task<IActionResult> QuenMatKhau(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Vui lòng nhập email!";
                return View();
            }

            // Tìm tài khoản qua học viên
            var hocvien = await _ctx.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .FirstOrDefaultAsync(h => h.DANGKYMOI != null && h.DANGKYMOI.Email == email);

            if (hocvien == null)
            {
                ViewBag.Error = "Không tìm thấy email này trong hệ thống!";
                return View();
            }

            var taikhoan = await _ctx.TAIKHOAN
                .FirstOrDefaultAsync(t => t.MaTaiKhoan == hocvien.MaTaiKhoan);

            if (taikhoan == null)
            {
                ViewBag.Error = "Tài khoản liên kết với email không tồn tại!";
                return View();
            }

            // Tạo mật khẩu ngẫu nhiên
            string newPass = GenerateRandomPassword();
            taikhoan.MatKhau = newPass;
            _ctx.Update(taikhoan);
            await _ctx.SaveChangesAsync();

            try
            {
                await SendResetPasswordEmail(email, taikhoan.TenDangNhap, newPass);
                ViewBag.Message = "Mật khẩu mới đã được gửi về email của bạn!";
            }
            catch
            {
                ViewBag.Error = "Gửi email thất bại! Vui lòng thử lại.";
            }

            return View();
        }

        // Hàm random password
        private string GenerateRandomPassword()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Hàm gửi email
        private async Task SendResetPasswordEmail(string toEmail, string username, string newPass)
        {
            var emailConfig = _configuration.GetSection("Smtp");
            var fromEmail = emailConfig["Email"];
            var fromPass = emailConfig["Password"];

            var subject = "Khôi phục mật khẩu - Trung tâm Ngoại ngữ";
            var body = $@"
        <p>Xin chào,</p>
        <p>Bạn vừa yêu cầu khôi phục mật khẩu.</p>
        <p><b>Tên đăng nhập:</b> {username}</p>
        <p><b>Mật khẩu mới:</b> {newPass}</p>
        <p>Vui lòng đăng nhập và đổi mật khẩu ngay sau khi vào hệ thống.</p>
        <p>Trân trọng,<br/>Trung tâm Ngoại ngữ</p>";

            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(fromEmail, fromPass);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
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
