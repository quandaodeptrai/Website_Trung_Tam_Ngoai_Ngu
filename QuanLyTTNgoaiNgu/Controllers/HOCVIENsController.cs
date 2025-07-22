using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyTTNgoaiNgu.Data;
using QuanLyTTNgoaiNgu.Models;

namespace QuanLyTTNgoaiNgu.Controllers
{
    public class HOCVIENsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public HOCVIENsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: HOCVIENs
        public async Task<IActionResult> Index()
        {
            var list = await _context.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .Include(h => h.TAIKHOAN)
                .ToListAsync();
            return View(list);
        }

        // GET: HOCVIENs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var hocvien = await _context.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .Include(h => h.TAIKHOAN)
                .FirstOrDefaultAsync(m => m.MaHocVien == id);
            if (hocvien == null) return NotFound();

            return View(hocvien);
        }

        // GET: HOCVIENs/Create
        public IActionResult Create()
        {
            ViewData["MaDangKy"] = new SelectList(_context.DANGKYMOI, "MaDangKy", "DiaChi");
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau");
            return View();
        }

        // POST: HOCVIENs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHocVien,MaDangKy,MaTaiKhoan")] HOCVIEN hOCVIEN)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hOCVIEN);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDangKy"] = new SelectList(_context.DANGKYMOI, "MaDangKy", "DiaChi", hOCVIEN.MaDangKy);
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau", hOCVIEN.MaTaiKhoan);
            return View(hOCVIEN);
        }

        // GET: HOCVIENs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Load entity kèm DANGKYMOI
            var hocvien = await _context.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .FirstOrDefaultAsync(h => h.MaHocVien == id);
            if (hocvien == null) return NotFound();

            // Dropdown tài khoản
            ViewBag.MaTaiKhoan = new SelectList(
                _context.TAIKHOAN,
                "MaTaiKhoan",
                "MaTaiKhoan",
                hocvien.MaTaiKhoan
            );
            return View(hocvien);
        }

        // POST: HOCVIENs/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HOCVIEN postedModel)
        {
            if (id != postedModel.MaHocVien)
                return NotFound();

            // Load entity gốc từ DB
            var hocvien = await _context.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .FirstOrDefaultAsync(h => h.MaHocVien == id);
            if (hocvien == null) return NotFound();

            // Thêm một bước kiểm tra ModelState
            if (!ModelState.IsValid)
            {
                ViewBag.MaTaiKhoan = new SelectList(
                    _context.TAIKHOAN, "MaTaiKhoan", "MaTaiKhoan", postedModel.MaTaiKhoan);
                // Đảm bảo nested được gán lại để view gọi lại không blank
                hocvien.DANGKYMOI = postedModel.DANGKYMOI;
                hocvien.MaTaiKhoan = postedModel.MaTaiKhoan;
                return View(hocvien);
            }

            // Cập nhật khóa tài khoản
            hocvien.MaTaiKhoan = postedModel.MaTaiKhoan;
            // Cập nhật các trường DANGKYMOI từ postedModel
            hocvien.DANGKYMOI.HoTen = postedModel.DANGKYMOI.HoTen;
            hocvien.DANGKYMOI.NgaySinh = postedModel.DANGKYMOI.NgaySinh;
            hocvien.DANGKYMOI.SoDienThoai = postedModel.DANGKYMOI.SoDienThoai;
            hocvien.DANGKYMOI.DiaChi = postedModel.DANGKYMOI.DiaChi;
            hocvien.DANGKYMOI.Email = postedModel.DANGKYMOI.Email;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.HOCVIEN.Any(e => e.MaHocVien == id))
                    return NotFound();
                throw;
            }
        }


        // GET: HOCVIENs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var hocvien = await _context.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .Include(h => h.TAIKHOAN)
                .FirstOrDefaultAsync(m => m.MaHocVien == id);
            if (hocvien == null) return NotFound();

            return View(hocvien);
        }

        // POST: HOCVIENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hOCVIEN = await _context.HOCVIEN.FindAsync(id);
            if (hOCVIEN != null)
            {
                _context.HOCVIEN.Remove(hOCVIEN);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool HOCVIENExists(int id)
        {
            return _context.HOCVIEN.Any(e => e.MaHocVien == id);
        }

        

        // GET: HOCVIENs/History/5
        public async Task<IActionResult> History(int? id)
        {
            if (id == null) return NotFound();
            var hv = await _context.HOCVIEN
                .Include(h => h.PHIEUDANGKies)
                    .ThenInclude(p => p.LOPHOC)
                .Include(h => h.PHIEUDANGKies)
                    .ThenInclude(p => p.KETQUAHOCTAP)
                .FirstOrDefaultAsync(h => h.MaHocVien == id);

            if (hv == null) return NotFound();
            return View(hv);
        }

        public async Task<IActionResult> AssignClass(int? id)
        {
            if (id == null) return NotFound();

            // Include DANGKYMOI để Model.DANGKYMOI không null
            var hocvien = await _context.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .FirstOrDefaultAsync(h => h.MaHocVien == id);

            if (hocvien == null) return NotFound();

            // Lấy danh sách lớp hiện có để chọn
            ViewBag.AvailableClasses = new SelectList(
                _context.LOPHOC.ToList(),
                "MaLopHoc",
                "TenLopHoc"
            );

            return View(hocvien);
        }

        // POST: HOCVIENs/AssignClass/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignClass(int id, int selectedLopHoc)
        {
            // Tạo phieu dang ky mới cho học viên id vào lớp selectedLopHoc
            var phieu = new PHIEUDANGKY
            {
                MaHocVien = id,
                MaLopHoc = selectedLopHoc,
                NgayDangKy = DateTime.Now
            };
            _context.PHIEUDANGKY.Add(phieu);
            await _context.SaveChangesAsync();

            // Chuyển về History để xem ngay kết quả
            return RedirectToAction(nameof(History), new { id });
        }

    }
}
