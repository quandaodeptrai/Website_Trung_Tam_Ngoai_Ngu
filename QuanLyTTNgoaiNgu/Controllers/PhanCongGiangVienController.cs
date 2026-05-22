using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyTTNgoaiNgu.Data;
using QuanLyTTNgoaiNgu.Models;
using QuanLyTTNgoaiNgu.Models;

namespace QuanLyTTNgoaiNgu.Controllers
{
    [NoCache]
    public class PhanCongGiangVienController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public PhanCongGiangVienController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        public IActionResult PhanCong()
        {
            ViewData["LopHocList"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc");

            ViewBag.LichDaPhanCong = _context.THOIKHOABIEU
                .Include(t => t.LOPHOC)
                .ThenInclude(l => l.GIANGVIEN)
                .Select(t => new LichPhanCongItem
                {
                    MaLichHoc = t.MaLichHoc,
                    TenGiangVien = t.LOPHOC.GIANGVIEN != null ? t.LOPHOC.GIANGVIEN.HoTen : "(Chưa có)",
                    TenLopHoc = t.LOPHOC.TenLopHoc,
                    NgayHoc = t.NgayHoc.Value,
                    CaHoc = t.CaHoc
                })
                .OrderBy(x => x.NgayHoc)
                .ThenBy(x => x.CaHoc)
                .ToList();

            return View(new PhanCongGiangVienViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ThucHienPhanCong(PhanCongGiangVienViewModel model)
        {
            // repopulate Lop dropdown
            ViewData["LopHocList"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc", model.MaLopHoc);

            // Lấy tất cả schedules của lớp (in-memory list)
            var schedules = await _context.THOIKHOABIEU
                .Where(t => t.MaLopHoc == model.MaLopHoc && t.NgayHoc.HasValue)
                .OrderBy(t => t.NgayHoc)
                .ThenBy(t => t.CaHoc)
                .ToListAsync();

            ViewBag.Schedules = schedules;

            // Cập nhật Lịch đã phân công để hiển thị
            ViewBag.LichDaPhanCong = await _context.THOIKHOABIEU
                .Include(t => t.LOPHOC)
                    .ThenInclude(l => l.GIANGVIEN)
                .Select(t => new LichPhanCongItem
                {
                    MaLichHoc = t.MaLichHoc,
                    TenGiangVien = t.LOPHOC.GIANGVIEN != null ? t.LOPHOC.GIANGVIEN.HoTen : "(Chưa có)",
                    TenLopHoc = t.LOPHOC.TenLopHoc,
                    NgayHoc = t.NgayHoc.Value,
                    CaHoc = t.CaHoc
                })
                .OrderBy(x => x.NgayHoc).ThenBy(x => x.CaHoc)
                .ToListAsync();

            // CASE 1: user chỉ xem danh sách giảng viên (chưa chọn gv)
            if (model.MaGiangVienDuocChon == null)
            {
                if (model.MaLopHoc == 0)
                {
                    ModelState.AddModelError("", "Vui lòng chọn một lớp trước khi xem danh sách giảng viên.");
                    return View("PhanCong", model);
                }

                if (!schedules.Any())
                {
                    ModelState.AddModelError("", "Lớp này chưa có lịch học. Vui lòng tạo lịch trước.");
                    return View("PhanCong", model);
                }

                // --- CHIẾN LƯỢC:
                // 1) Lấy danh sách các ngày (distinct) của schedules (in-memory)
                var dates = schedules.Select(s => s.NgayHoc.Value).Distinct().ToList();

                // 2) Lấy candidate THOIKHOABIEU từ DB chỉ những bản ghi có NgayHoc trong 'dates'
                //    (giảm lượng bản ghi đưa về client, tránh AsEnumerable toàn bộ bảng)
                var tkbCandidates = await _context.THOIKHOABIEU
                    .Include(t => t.LOPHOC)
                    .Where(t => t.NgayHoc.HasValue && dates.Contains(t.NgayHoc.Value))
                    .ToListAsync();

                // 3) So khớp cặp (NgayHoc, CaHoc) ở phía client giữa tkbCandidates và schedules
                var busyTeacherIds = tkbCandidates
                    .Where(tkb => schedules.Any(s => s.NgayHoc.Value == tkb.NgayHoc && s.CaHoc == tkb.CaHoc))
                    .Select(tkb => tkb.LOPHOC.MaGiangVien)
                    .Distinct()
                    .ToList();

                // 4) Danh sách giảng viên rảnh là những GIANGVIEN không có trong busyTeacherIds
                model.DanhSachGiangVienPhuHop = await _context.GIANGVIEN
                    .Where(g => !busyTeacherIds.Contains(g.MaGiangVien))
                    .ToListAsync();

                // Mặc định chọn lịch đầu tiên để điền hidden inputs
                var first = schedules.First();
                model.NgayHoc = first.NgayHoc.Value;
                model.CaHoc = first.CaHoc;

                return View("PhanCong", model);
            }

            // CASE 2: user đã chọn giảng viên -> thực hiện phân công
            if (model.MaLopHoc == 0 || model.NgayHoc == default || string.IsNullOrWhiteSpace(model.CaHoc) || model.MaGiangVienDuocChon == null)
            {
                ModelState.AddModelError("", "Dữ liệu không đầy đủ để phân công. Vui lòng thử lại.");
                model.DanhSachGiangVienPhuHop = await _context.GIANGVIEN.ToListAsync();
                return View("PhanCong", model);
            }

            // Kiểm tra giảng viên đã bận trên ngày/ca đó chưa (query dịch được)
            var isBusy = await _context.THOIKHOABIEU
                .Include(t => t.LOPHOC)
                .Where(t => t.NgayHoc.HasValue
                            && t.NgayHoc.Value == model.NgayHoc
                            && t.CaHoc == model.CaHoc
                            && t.LOPHOC.MaGiangVien == model.MaGiangVienDuocChon)
                .AnyAsync();

            if (isBusy)
            {
                ModelState.AddModelError("", "Giảng viên này đã có lịch dạy vào ngày/ca đó. Vui lòng chọn giảng viên khác.");

                // recompute danh sách giảng viên rảnh cho hiển thị
                var dates = schedules.Select(s => s.NgayHoc.Value).Distinct().ToList();
                var tkbCandidates = await _context.THOIKHOABIEU
                    .Include(t => t.LOPHOC)
                    .Where(t => t.NgayHoc.HasValue && dates.Contains(t.NgayHoc.Value))
                    .ToListAsync();

                var busyTeacherIds2 = tkbCandidates
                    .Where(tkb => schedules.Any(s => s.NgayHoc.Value == tkb.NgayHoc && s.CaHoc == tkb.CaHoc))
                    .Select(tkb => tkb.LOPHOC.MaGiangVien)
                    .Distinct()
                    .ToList();

                model.DanhSachGiangVienPhuHop = await _context.GIANGVIEN
                    .Where(g => !busyTeacherIds2.Contains(g.MaGiangVien))
                    .ToListAsync();

                ViewBag.Schedules = schedules;
                return View("PhanCong", model);
            }

            // Cập nhật LOPHOC.MaGiangVien
            var lopToUpdate = await _context.LOPHOC.FindAsync(model.MaLopHoc);
            if (lopToUpdate == null)
            {
                ModelState.AddModelError("", "Không tìm thấy lớp để phân công.");
                return View("PhanCong", model);
            }

            lopToUpdate.MaGiangVien = model.MaGiangVienDuocChon.Value;
            _context.Update(lopToUpdate);

            // Chỉ tạo THOIKHOABIEU mới nếu chưa tồn tại (same MaLopHoc + NgayHoc + CaHoc)
            bool existsTkb = await _context.THOIKHOABIEU.AnyAsync(t =>
                t.MaLopHoc == model.MaLopHoc
                && t.NgayHoc.HasValue
                && t.NgayHoc.Value == model.NgayHoc
                && t.CaHoc == model.CaHoc
            );

            if (!existsTkb)
            {
                var newTkb = new THOIKHOABIEU
                {
                    MaLopHoc = model.MaLopHoc,
                    NgayHoc = model.NgayHoc,
                    CaHoc = model.CaHoc
                };
                _context.THOIKHOABIEU.Add(newTkb);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Phân công giảng viên thành công!";

            return RedirectToAction(nameof(PhanCong));
        }

    }
}
