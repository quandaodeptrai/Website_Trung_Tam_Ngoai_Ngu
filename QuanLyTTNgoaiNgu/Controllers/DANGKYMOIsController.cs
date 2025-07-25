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


namespace QuanLyTTNgoaiNgu.Controllers
{

    [NoCache]

    public class DANGKYMOIsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public DANGKYMOIsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: DANGKYMOIs
        public async Task<IActionResult> Index()
        {
            var list = await _context.DANGKYMOI.ToListAsync();
            return View(list);
        }
        // GET: Pending: Ds đăng ký chưa xét duyệt
        public async Task<IActionResult> Pending()
        {
            var list = await _context.DANGKYMOI
                                     .Include(d => d.HOCVIEN)
                                     .Where(d => d.HOCVIEN == null)
                                     .ToListAsync();
            return View(list);
        }

        // GET: Approve : xét duyệt 1 đk
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
                GeneratedUsername = GenerateUsername(dky.HoTen)
            };
            return View(vm);
        }

        // POST: Approve
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int MaDangKy)
        {
            var dky = await _context.DANGKYMOI.FindAsync(MaDangKy);
            if (dky == null) return NotFound();

            // Sinh username mới
            var username = GenerateUsername(dky.HoTen);

            // 1) Tạo tài khoản mới
            var newTk = new TAIKHOAN
            {
                TenDangNhap = username,
                MatKhau = dky.SoDienThoai,
                VaiTro = "HocVien"
            };
            _context.TAIKHOAN.Add(newTk);
            await _context.SaveChangesAsync();

            // 2) Tạo HOCVIEN liên kết
            _context.HOCVIEN.Add(new HOCVIEN
            {
                MaDangKy = dky.MaDangKy,
                MaTaiKhoan = newTk.MaTaiKhoan
            });

            // 3) Cập nhật MaQuanTriVien theo admin hiện tại
            var adminUser = User.Identity.Name;
            var adminTk = await _context.TAIKHOAN
                                 .FirstOrDefaultAsync(t => t.TenDangNhap == adminUser);
            if (adminTk != null)
            {
                var qtv = await _context.QUANTRIVIEN
                              .FirstOrDefaultAsync(q => q.MaTaiKhoan == adminTk.MaTaiKhoan);
                if (qtv != null)
                    dky.MaQuanTriVien = qtv.MaQuanTriVien;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Pending));
        }

        // Loại bỏ dấu tiếng việt để tạo tên đăng nhập ko dấu
        private string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
        //Sinh tên đăg nhập từ họ tên
        private string GenerateUsername(string hoTen)
        {
            var baseName = RemoveDiacritics(hoTen).ToLowerInvariant();
            baseName = Regex.Replace(baseName, @"\s+", ".");
            var username = baseName;
            int suffix = 1;
            while (_context.TAIKHOAN.Any(u => u.TenDangNhap == username))
            {
                username = $"{baseName}{suffix}";
                suffix++;
            }
            return username;
        }
        


        // GET: DANGKYMOIs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DANGKYMOIs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoTen,NgaySinh,SoDienThoai,DiaChi,Email")] DANGKYMOI dky)
        {
            if (!ModelState.IsValid)
                return View(dky);

            // Gán tạm MaQuanTriVien = 1 (Admin đầu tiên) để chờ xét duyệt
            dky.MaQuanTriVien = 1;

            _context.Add(dky);
            await _context.SaveChangesAsync();

            // Redirect sang trang thông báo
            return RedirectToAction(nameof(Submitted));
        }

        // GET: DANGKYMOIs/Submitted
        public IActionResult Submitted()
        {
            return View();
        }



        // GET: DANGKYMOIs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.DANGKYMOI
                .FirstOrDefaultAsync(m => m.MaDangKy == id);
            if (item == null) return NotFound();

            return View(item);
        }

        // POST: DANGKYMOIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.DANGKYMOI.FindAsync(id);
            if (item != null)
            {
                _context.DANGKYMOI.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DANGKYMOIExists(int id)
            => _context.DANGKYMOI.Any(e => e.MaDangKy == id);


    }
}
