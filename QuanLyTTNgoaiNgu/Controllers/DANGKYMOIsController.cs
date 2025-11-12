using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyTTNgoaiNgu.Data;
using QuanLyTTNgoaiNgu.Models;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;

namespace QuanLyTTNgoaiNgu.Controllers
{
    [NoCache]
    public class DANGKYMOIsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;
        private readonly IConfiguration _configuration;

        public DANGKYMOIsController(QuanLyTTNgoaiNguContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Danh sách tất cả đăng ký
        public async Task<IActionResult> Index()
        {
            var list = await _context.DANGKYMOI.ToListAsync();
            return View(list);
        }

        // GET: Pending - danh sách đăng ký chưa duyệt
        public async Task<IActionResult> Pending()
        {
            var list = await _context.DANGKYMOI
                                     .Include(d => d.HOCVIEN)
                                     .Where(d => d.HOCVIEN == null)
                                     .ToListAsync();
            return View(list);
        }

        // GET: Approve - hiển thị form duyệt
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null) return NotFound();

            var dky = await _context.DANGKYMOI.FindAsync(id);
            if (dky == null) return NotFound();

            var vm = new ApproveViewModel
            {
                MaDangKy = dky.MaDangKy,
                HoTen = dky.HoTen,
                NgaySinh = dky.NgaySinh,
                SoDienThoai = dky.SoDienThoai,
                DiaChi = dky.DiaChi,
                Email = dky.Email,
                GeneratedUsername = dky.Email
            };
            return View(vm);
        }

        // POST: Approve - xác nhận duyệt
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int MaDangKy)
        {
            var dky = await _context.DANGKYMOI.FindAsync(MaDangKy);
            if (dky == null) return NotFound();
            var existingHocVien = await _context.HOCVIEN
                                    .FirstOrDefaultAsync(h => h.MaDangKy == dky.MaDangKy);
            if (existingHocVien != null)
            {
                // Chỉ cập nhật cột DaDuyet
                dky.DaDuyet = true;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
         
            // Tạo tên đăng nhập
                var username = dky.Email;

                // 1️⃣ Tạo tài khoản mới
                var newTk = new TAIKHOAN
                {
                    TenDangNhap = username,
                    MatKhau = dky.SoDienThoai,
                    VaiTro = "HocVien"
                };
                _context.TAIKHOAN.Add(newTk);
                await _context.SaveChangesAsync();

            dky.DaDuyet = true; 
            await _context.SaveChangesAsync();

            // 2️⃣ Tạo học viên liên kết
            _context.HOCVIEN.Add(new HOCVIEN
                {
                    MaDangKy = dky.MaDangKy,
                    MaTaiKhoan = newTk.MaTaiKhoan
                });

                // 3️⃣ Gán admin duyệt (nếu có)
                var adminUser = User.Identity?.Name;
                if (!string.IsNullOrEmpty(adminUser))
                {
                    var adminTk = await _context.TAIKHOAN.FirstOrDefaultAsync(t => t.TenDangNhap == adminUser);
                    if (adminTk != null)
                    {
                        var qtv = await _context.QUANTRIVIEN.FirstOrDefaultAsync(q => q.MaTaiKhoan == adminTk.MaTaiKhoan);
                        if (qtv != null)
                            dky.MaQuanTriVien = qtv.MaQuanTriVien;
                    }
                }

                await _context.SaveChangesAsync();

                // 4️⃣ Gửi email thông báo tài khoản
                try
                {
                    await SendApprovalEmail(dky.Email, username, dky.SoDienThoai);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
                }

                return RedirectToAction(nameof(Pending));
            }
        

        // Hàm gửi email
        private async Task SendApprovalEmail(string toEmail, string username, string password)
        {
            var emailConfig = _configuration.GetSection("Smtp");
            var fromEmail = emailConfig["Email"];
            var fromPass = emailConfig["Password"];
            var subject = "Xét duyệt đăng ký tài khoản trung tâm ngoại ngữ";
            var body = $@"
                <p>Xin chào,</p>
                <p>Đăng ký học của bạn đã được xét duyệt thành công.</p>
                <p><b>Tên đăng nhập:</b> {username}</p>
                <p><b>Mật khẩu:</b> {password}</p>
                <p>Vui lòng đăng nhập vào hệ thống để tiếp tục học.</p>
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

        // GET: Tạo mới đăng ký
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoTen,NgaySinh,SoDienThoai,DiaChi,Email")] DANGKYMOI dky)
        {
            if (!ModelState.IsValid) return View(dky);

            dky.MaQuanTriVien = 1; // Tạm gán admin mặc định
            _context.Add(dky);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Submitted));
        }

        public IActionResult Submitted() => View();

        private bool DANGKYMOIExists(int id) => _context.DANGKYMOI.Any(e => e.MaDangKy == id);
    }
}
