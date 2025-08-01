using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class GIANGVIENsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public GIANGVIENsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: GIANGVIENs
        public async Task<IActionResult> Index()
        {
            var quanLyTTNgoaiNguContext = _context.GIANGVIEN.Include(g => g.TAIKHOAN);
            return View(await quanLyTTNgoaiNguContext.ToListAsync());
        }

        // GET: GIANGVIENs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gIANGVIEN = await _context.GIANGVIEN
                .Include(g => g.TAIKHOAN)
                .FirstOrDefaultAsync(m => m.MaGiangVien == id);
            if (gIANGVIEN == null)
            {
                return NotFound();
            }

            return View(gIANGVIEN);
        }

        // GET: GIANGVIENs/Create
        public IActionResult Create()
        {
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau");
            return View();
        }

        // POST: GIANGVIENs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaGiangVien,HoTen,ChuyenMon,Email,SoDienThoai,MaTaiKhoan")] GIANGVIEN gIANGVIEN)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gIANGVIEN);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau", gIANGVIEN.MaTaiKhoan);
            return View(gIANGVIEN);
        }

        // GET: GIANGVIENs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gIANGVIEN = await _context.GIANGVIEN.FindAsync(id);
            if (gIANGVIEN == null)
            {
                return NotFound();
            }
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau", gIANGVIEN.MaTaiKhoan);
            return View(gIANGVIEN);
        }

        // POST: GIANGVIENs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaGiangVien,HoTen,ChuyenMon,Email,SoDienThoai,MaTaiKhoan")] GIANGVIEN gIANGVIEN)
        {
            if (id != gIANGVIEN.MaGiangVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gIANGVIEN);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GIANGVIENExists(gIANGVIEN.MaGiangVien))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau", gIANGVIEN.MaTaiKhoan);
            return View(gIANGVIEN);
        }

        // GET: GIANGVIENs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gIANGVIEN = await _context.GIANGVIEN
                .Include(g => g.TAIKHOAN)
                .FirstOrDefaultAsync(m => m.MaGiangVien == id);
            if (gIANGVIEN == null)
            {
                return NotFound();
            }

            return View(gIANGVIEN);
        }

        // POST: GIANGVIENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gIANGVIEN = await _context.GIANGVIEN.FindAsync(id);
            if (gIANGVIEN != null)
            {
                _context.GIANGVIEN.Remove(gIANGVIEN);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GIANGVIENExists(int id)
        {
            return _context.GIANGVIEN.Any(e => e.MaGiangVien == id);
        }
        // GET: Xem lop duoc phan cong
        public async Task<IActionResult> MyClasses()
        {
            // 1) Lấy giảng viên hiện tại
            var username = User.Identity.Name;
            var gv = await _context.GIANGVIEN
                .Include(g => g.TAIKHOAN)
                .FirstOrDefaultAsync(g => g.TAIKHOAN.TenDangNhap == username);
            if (gv == null) return RedirectToAction("Index", "Home");

            // 2) Lấy tất cả lớp được phân công
            var all = await _context.LOPHOC
                .Where(l => l.MaGiangVien == gv.MaGiangVien)
                .Include(l => l.KHOAHOC)
                .Include(l => l.PHIEUDANGKies)
                .ToListAsync();

            var today = DateTime.Today;
            var upcoming = all
                .Where(l => l.NgayKetThuc >= today)
                .OrderBy(l => l.NgayKetThuc)
                .ToList();

            var past = all
                .Where(l => l.NgayKetThuc < today)
                .OrderByDescending(l => l.NgayKetThuc)
                .ToList();

            var vm = new MyClassesViewModel
            {
                UpcomingClasses = upcoming,
                PastClasses = past
            };
            return View(vm);
        }

        // GET: Xem thoi khoa bieu cua rieng tung lop
        public async Task<IActionResult> Schedule(int classId)
        {
            // Xác thực: giảng viên chỉ xem lớp của mình
            var username = User.Identity.Name;
            var gv = await _context.GIANGVIEN
                .Include(g => g.TAIKHOAN)
                .FirstOrDefaultAsync(g => g.TAIKHOAN.TenDangNhap == username);
            if (gv == null) return RedirectToAction("Index", "Home");

            // Kiểm tra lớp có thuộc giảng viên hay không
            var lop = await _context.LOPHOC
                .FirstOrDefaultAsync(l => l.MaLopHoc == classId && l.MaGiangVien == gv.MaGiangVien);
            if (lop == null) return Forbid();

            // Lấy thời khóa biểu
            var schedules = await _context.THOIKHOABIEU
                .Where(t => t.MaLopHoc == classId)
                .OrderBy(t => t.NgayHoc)
                .ToListAsync();

            ViewBag.ClassName = lop.TenLopHoc;
            return View(schedules);
        }

        // GET: xem thoi khoa bieu cua ngay hom nay
        public async Task<IActionResult> ScheduleAll()
        {
            // 1) Xác định giảng viên hiện tại
            var username = User.Identity.Name;
            var gv = await _context.GIANGVIEN
                .Include(g => g.TAIKHOAN)
                .FirstOrDefaultAsync(g => g.TAIKHOAN.TenDangNhap == username);
            if (gv == null) return RedirectToAction("Index", "Home");

            // 2) Lấy danh sách MaLopHoc của giảng viên
            var classIds = await _context.LOPHOC
                .Where(l => l.MaGiangVien == gv.MaGiangVien)
                .Select(l => l.MaLopHoc)
                .ToListAsync();

            // 3) Lấy toàn bộ thời khóa biểu cho các lớp đó
            var schedules = await _context.THOIKHOABIEU
                .Where(t => classIds.Contains(t.MaLopHoc))
                .Include(t => t.LOPHOC)
                    .ThenInclude(l => l.KHOAHOC)
                .OrderBy(t => t.NgayHoc)
                .ThenBy(t => t.CaHoc)
                .ToListAsync();

            return View(schedules);
        }
        // GET: xem thông báo
        public async Task<IActionResult> Notifications()
        {
            var username = User.Identity.Name;
            var acc = await _context.TAIKHOAN
                .FirstOrDefaultAsync(t => t.TenDangNhap == username);
            if (acc == null) return RedirectToAction("Index", "Home");

            var list = await _context.THONGBAO
                .Where(tb => tb.MaTaiKhoan == acc.MaTaiKhoan)
                .OrderByDescending(tb => tb.NgayThongBao)
                .ToListAsync();

            return View(list);
        }

        // GET: GIANGVIENs/EnterScores/5
        public async Task<IActionResult> EnterScores(int classId)
        {
            // Xác định giảng viên hiện tại
            var username = User.FindFirstValue(ClaimTypes.Name);
            var gv = await _context.GIANGVIEN
                         .Include(g => g.TAIKHOAN)
                         .FirstOrDefaultAsync(g => g.TAIKHOAN.TenDangNhap == username);
            if (gv == null) return RedirectToAction("Index", "Home");

            // Kiểm tra lớp có của mình hay không
            var lop = await _context.LOPHOC
                         .FirstOrDefaultAsync(l => l.MaLopHoc == classId && l.MaGiangVien == gv.MaGiangVien);
            if (lop == null) return Forbid();

            // Lấy danh sách PHIEUDANGKY của lớp kèm KETQUAHOCTAP và DANGKYMOI.HoTen
            var list = await _context.PHIEUDANGKY
                .Where(p => p.MaLopHoc == classId)
                .Include(p => p.HOCVIEN).ThenInclude(h => h.DANGKYMOI)
                .Include(p => p.KETQUAHOCTAP)
                .ToListAsync();

            var vm = list.Select(p => new ScoreEntryViewModel
            {
                MaPhieu = p.MaPhieu,
                TenHocVien = p.HOCVIEN?.DANGKYMOI?.HoTen ?? "(Không tên)",
                Diem = p.KETQUAHOCTAP?.Diem ?? 0
            }).ToList();

            ViewBag.ClassId = classId;
            return View(vm);
        }

        // POST: GIANGVIENs/EnterScores/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EnterScores(int classId, List<ScoreEntryViewModel> model)
        {
            // Xác thực tương tự GET
            var username = User.FindFirstValue(ClaimTypes.Name);
            var gv = await _context.GIANGVIEN
                         .Include(g => g.TAIKHOAN)
                         .FirstOrDefaultAsync(g => g.TAIKHOAN.TenDangNhap == username);
            if (gv == null) return RedirectToAction("Index", "Home");

            var lop = await _context.LOPHOC
                         .FirstOrDefaultAsync(l => l.MaLopHoc == classId && l.MaGiangVien == gv.MaGiangVien);
            if (lop == null) return Forbid();

            if (!ModelState.IsValid)
            {
                ViewBag.TenLop = lop.TenLopHoc;
                return View(model);
            }

            // Với mỗi entry, cập nhật hoặc tạo mới KETQUAHOCTAP
            foreach (var entry in model)
            {
                var existing = await _context.KETQUAHOCTAP
                    .FirstOrDefaultAsync(k => k.MaPhieu == entry.MaPhieu);
                if (existing != null)
                {
                    existing.Diem = entry.Diem;
                    _context.KETQUAHOCTAP.Update(existing);
                }
                else
                {
                    _context.KETQUAHOCTAP.Add(new KETQUAHOCTAP
                    {
                        MaPhieu = entry.MaPhieu,
                        Diem = entry.Diem
                    });
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Lưu điểm thành công!";
            return RedirectToAction(nameof(EnterScores), new { classId });
        }

    }


}
