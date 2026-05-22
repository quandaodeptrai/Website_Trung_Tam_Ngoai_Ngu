using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyTTNgoaiNgu.Data;
using QuanLyTTNgoaiNgu.Models;

namespace QuanLyTTNgoaiNgu.Controllers
{
    
    [NoCache]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class LOPHOCsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public LOPHOCsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: LOPHOCs
        public IActionResult Index(string tenLop, DateTime? ngayBD, DateTime? ngayKT, int? giangVien)
        {
            var query = _context.LOPHOC
                .Include(x => x.KHOAHOC)
                .Include(x => x.GIANGVIEN)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tenLop))
                query = query.Where(x => x.TenLopHoc.Contains(tenLop));

            if (ngayBD.HasValue)
                query = query.Where(x => x.NgayBatDau >= ngayBD.Value);

            if (ngayKT.HasValue)
                query = query.Where(x => x.NgayKetThuc <= ngayKT.Value);

            if (giangVien.HasValue)
                query = query.Where(x => x.MaGiangVien == giangVien.Value);

            ViewBag.GiangVienList = _context.GIANGVIEN.ToList();

            // ✅ GIỮ LẠI GIÁ TRỊ BỘ LỌC
            ViewBag.CurrentFilters = new
            {
                tenLop,
                ngayBD = ngayBD?.ToString("yyyy-MM-dd"),
                ngayKT = ngayKT?.ToString("yyyy-MM-dd"),
                giangVien
            };

            return View(query.ToList());
        }



        // GET: LOPHOCs/Create
        public IActionResult Create()
        {
            if (!_context.KHOAHOC.Any() || !_context.GIANGVIEN.Any())
            {
                ViewBag.Error = "⚠️ Vui lòng thêm dữ liệu cho bảng Khóa học và Giảng viên trước khi tạo lớp học.";
                return View();
            }

            ViewBag.MaKhoaHoc = new SelectList(_context.KHOAHOC, "MaKhoaHoc", "TenKhoaHoc");
            // không cần truyền ViewBag.MaGiangVien nữa nếu không dùng select ở view
            var model = new LOPHOC
            {
                MaGiangVien = 1 // gán mặc định 1
            };
            return View(model);
        }


        //POST: LOPHOCs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("TenLopHoc,SLHocVienToiDa,NgayBatDau,NgayKetThuc,MaKhoaHoc,MaGiangVien")] LOPHOC model)
        {
            if (model.MaGiangVien == 0)
            {
                model.MaGiangVien = 1;
            }

            // --- Kiểm tra binding: nếu client để trống date thì ModelState sẽ có lỗi
            if (ModelState.ContainsKey(nameof(model.NgayBatDau)) && ModelState[nameof(model.NgayBatDau)].Errors.Count > 0)
            {
                // giữ thông điệp thân thiện
                ModelState.AddModelError(nameof(model.NgayBatDau), "❌ Ngày bắt đầu là bắt buộc.");
            }
            else
            {
                // nếu binding ok -> kiểm tra giá trị (so sánh phần Date)
                if (model.NgayBatDau.Date < DateTime.Today)
                {
                    ModelState.AddModelError(nameof(model.NgayBatDau), "❌ Ngày bắt đầu phải từ ngày hiện tại trở đi.");
                }
            }

            if (ModelState.ContainsKey(nameof(model.NgayKetThuc)) && ModelState[nameof(model.NgayKetThuc)].Errors.Count > 0)
            {
                ModelState.AddModelError(nameof(model.NgayKetThuc), "❌ Ngày kết thúc là bắt buộc.");
            }
            else
            {
                // nếu binding ok -> kiểm tra ngày kết thúc > ngày bắt đầu
                if (model.NgayKetThuc.Date <= model.NgayBatDau.Date)
                {
                    ModelState.AddModelError(nameof(model.NgayKetThuc), "❌ Ngày kết thúc phải sau ngày bắt đầu.");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.MaKhoaHoc = new SelectList(_context.KHOAHOC, "MaKhoaHoc", "TenKhoaHoc", model.MaKhoaHoc);
                return View(model);
            }

            try
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "THOIKHOABIEUx", new { maLopHoc = model.MaLopHoc });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "❌ Lỗi lưu dữ liệu: " + ex.Message);
                ViewBag.MaKhoaHoc = new SelectList(_context.KHOAHOC, "MaKhoaHoc", "TenKhoaHoc", model.MaKhoaHoc);
                return View(model);
            }
        }



        // GET: LOPHOCs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var lop = await _context.LOPHOC.FindAsync(id);
            if (lop == null) return NotFound();

            ViewData["MaKhoaHoc"] = new SelectList(_context.KHOAHOC, "MaKhoaHoc", "TenKhoaHoc", lop.MaKhoaHoc);
            ViewData["MaGiangVien"] = new SelectList(_context.GIANGVIEN, "MaGiangVien", "HoTen", lop.MaGiangVien);
            return View(lop);
        }

        // POST: LOPHOCs/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
     [Bind("MaLopHoc,TenLopHoc,SLHocVienToiDa")] LOPHOC model)
        {
            if (id != model.MaLopHoc) return NotFound();

            // Lấy lớp học hiện tại
            var lop = await _context.LOPHOC.FindAsync(id);
            if (lop == null) return NotFound();

            // Lấy số học viên đã đăng ký
            int soHocVienDaDangKy = await _context.PHIEUDANGKY.CountAsync(p => p.MaLopHoc == id);

            if (model.SLHocVienToiDa < soHocVienDaDangKy)
            {
                ModelState.AddModelError(nameof(model.SLHocVienToiDa),
                    $"Số lượng tối đa không được nhỏ hơn số học viên đã đăng ký ({soHocVienDaDangKy}).");
            }

            if (!ModelState.IsValid)
            {
                // Trả lại form với dữ liệu hiện tại
                return View(model);
            }

            // Cập nhật tên và số lượng
            lop.TenLopHoc = model.TenLopHoc;
            lop.SLHocVienToiDa = model.SLHocVienToiDa;

            _context.Update(lop);
            await _context.SaveChangesAsync();

            TempData["Success"] = "✔ Cập nhật thành công!";
            return RedirectToAction(nameof(Index));
        }


        // GET: LOPHOCs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lOPHOC = await _context.LOPHOC
                .Include(l => l.GIANGVIEN)
                .Include(l => l.KHOAHOC)
                .FirstOrDefaultAsync(m => m.MaLopHoc == id);
            if (lOPHOC == null)
            {
                return NotFound();
            }

            return View(lOPHOC);
        }

        // GET: LOPHOCs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lOPHOC = await _context.LOPHOC
                .Include(l => l.GIANGVIEN)
                .Include(l => l.KHOAHOC)
                .FirstOrDefaultAsync(m => m.MaLopHoc == id);
            if (lOPHOC == null)
            {
                return NotFound();
            }

            return View(lOPHOC);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lop = await _context.LOPHOC.FindAsync(id);

            if (lop == null)
            {
                TempData["Error"] = "❌ Lớp học không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra có học viên đăng ký hay không
            bool hasDangKy = await _context.PHIEUDANGKY.AnyAsync(p => p.MaLopHoc == id);

            if (hasDangKy)
            {
                TempData["Error"] = "❌ Không thể xóa lớp vì đã có học viên đăng ký!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.LOPHOC.Remove(lop);
                await _context.SaveChangesAsync();
                TempData["Success"] = "✔ Xóa lớp thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "❌ Lỗi khi xóa lớp: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }


        private bool LOPHOCExists(int id)
        {
            return _context.LOPHOC.Any(e => e.MaLopHoc == id);
        }
    }
}
