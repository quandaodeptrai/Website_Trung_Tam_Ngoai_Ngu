using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyTTNgoaiNgu.Data;
using QuanLyTTNgoaiNgu.Models;

namespace QuanLyTTNgoaiNgu.Controllers
{
    [NoCache]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class THOIKHOABIEUxController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public THOIKHOABIEUxController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: THOIKHOABIEUx
        public async Task<IActionResult> Index()
        {
            var quanLyTTNgoaiNguContext = _context.THOIKHOABIEU.Include(t => t.LOPHOC);
            return View(await quanLyTTNgoaiNguContext.ToListAsync());
        }

        // GET: THOIKHOABIEUx/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var tHOIKHOABIEU = await _context.THOIKHOABIEU
                .Include(t => t.LOPHOC)
                .FirstOrDefaultAsync(m => m.MaLichHoc == id);
            if (tHOIKHOABIEU == null) return NotFound();

            return View(tHOIKHOABIEU);
        }

        // GET: THOIKHOABIEUx/Create
        public IActionResult Create(int? maLopHoc)
        {
            // tải danh sách lớp để show trong select và kèm ngày bắt đầu/kết thúc
            var lopList = _context.LOPHOC
                .Select(l => new {
                    l.MaLopHoc,
                    l.TenLopHoc,
                    NgayBatDau = l.NgayBatDau.ToString("yyyy-MM-dd"),
                    NgayKetThuc = l.NgayKetThuc.ToString("yyyy-MM-dd")
                })
                .ToList();

            // Tạo dictionary mapping MaLopHoc -> list ngày đã có lịch (yyyy-MM-dd)
            var takenDict = new Dictionary<int, List<string>>();
            var allLopIds = lopList.Select(x => (int)x.MaLopHoc).ToList();
            foreach (var id in allLopIds)
            {
                var dates = _context.THOIKHOABIEU
                    .Where(t => t.MaLopHoc == id && t.NgayHoc.HasValue)
                    .Select(t => t.NgayHoc.Value.Date)
                    .ToList()
                    .Select(d => d.ToString("yyyy-MM-dd"))
                    .ToList();
                takenDict[id] = dates;
            }

            ViewBag.LOPHOCList = lopList;
            ViewBag.TakenDatesJson = JsonSerializer.Serialize(takenDict);

            var model = new THOIKHOABIEU();
            if (maLopHoc.HasValue) model.MaLopHoc = maLopHoc.Value;
            if (TempData["SuccessMessage"] != null) ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();

            return View(model);
        }

        // POST: THOIKHOABIEUx/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int? MaLopHoc,
            DateTime? NgayHoc,
            string[] SelectedCaHoc,
            string action,
            bool RepeatWeekly = false)
        {
            // reload lop list & taken dates in case of return view on error
            var lopList = _context.LOPHOC
                .Select(l => new {
                    l.MaLopHoc,
                    l.TenLopHoc,
                    NgayBatDau = l.NgayBatDau.ToString("yyyy-MM-dd"),
                    NgayKetThuc = l.NgayKetThuc.ToString("yyyy-MM-dd")
                })
                .ToList();
            ViewBag.LOPHOCList = lopList;

            // rebuild taken dict for client (if needed)
            var takenDict = new Dictionary<int, List<string>>();
            var allLopIds = lopList.Select(x => (int)x.MaLopHoc).ToList();
            foreach (var id in allLopIds)
            {
                var dates = _context.THOIKHOABIEU
                    .Where(t => t.MaLopHoc == id && t.NgayHoc.HasValue)
                    .Select(t => t.NgayHoc.Value.Date)
                    .ToList()
                    .Select(d => d.ToString("yyyy-MM-dd"))
                    .ToList();
                takenDict[id] = dates;
            }
            ViewBag.TakenDatesJson = JsonSerializer.Serialize(takenDict);

            // Server-side validation
            if (!MaLopHoc.HasValue || MaLopHoc.Value == 0)
            {
                ModelState.AddModelError(nameof(MaLopHoc), "Vui lòng chọn lớp.");
            }
            if (!NgayHoc.HasValue)
            {
                ModelState.AddModelError(nameof(NgayHoc), "Vui lòng chọn ngày học.");
            }
            if (SelectedCaHoc == null || SelectedCaHoc.Length == 0)
            {
                ModelState.AddModelError("SelectedCaHoc", "Vui lòng chọn ít nhất một ca học.");
            }

            if (!ModelState.IsValid)
            {
                var vm = new THOIKHOABIEU
                {
                    MaLopHoc = MaLopHoc ?? 0,
                    NgayHoc = NgayHoc
                };
                return View(vm);
            }

            // Lấy thông tin lớp
            var lop = await _context.LOPHOC.FindAsync(MaLopHoc.Value);
            if (lop == null)
            {
                ModelState.AddModelError(nameof(MaLopHoc), "Lớp không tồn tại.");
                var vm = new THOIKHOABIEU { MaLopHoc = MaLopHoc ?? 0, NgayHoc = NgayHoc };
                return View(vm);
            }

            DateTime startDate = NgayHoc.Value.Date;
            DateTime endDate = lop.NgayKetThuc.Date;

            // Lấy danh sách ngày đã có lịch cho lớp này để tránh chọn
            var takenForThisClass = await _context.THOIKHOABIEU
                .Where(t => t.MaLopHoc == MaLopHoc.Value && t.NgayHoc.HasValue)
                .Select(t => t.NgayHoc.Value.Date)
                .ToListAsync();

            // nếu user cố tình chọn ngày đã bị block -> throw model error
            if (takenForThisClass.Contains(startDate) && !RepeatWeekly)
            {
                ModelState.AddModelError(nameof(NgayHoc), "Ngày bạn chọn đã có lịch. Vui lòng chọn ngày khác.");
                var vm = new THOIKHOABIEU { MaLopHoc = MaLopHoc ?? 0 };
                return View(vm);
            }

            var toAdd = new List<THOIKHOABIEU>();

            foreach (var ca in SelectedCaHoc)
            {
                DateTime current = startDate;
                if (!RepeatWeekly)
                {
                    bool exists = await _context.THOIKHOABIEU
                        .AnyAsync(t => t.MaLopHoc == MaLopHoc.Value
                                       && t.NgayHoc.HasValue && t.NgayHoc.Value.Date == current
                                       && t.CaHoc == ca);
                    if (!exists)
                    {
                        toAdd.Add(new THOIKHOABIEU
                        {
                            MaLopHoc = MaLopHoc.Value,
                            NgayHoc = current,
                            CaHoc = ca
                        });
                    }
                }
                else
                {
                    while (current <= endDate)
                    {
                        bool exists = await _context.THOIKHOABIEU
                            .AnyAsync(t => t.MaLopHoc == MaLopHoc.Value
                                           && t.NgayHoc.HasValue && t.NgayHoc.Value.Date == current
                                           && t.CaHoc == ca);
                        if (!exists)
                        {
                            toAdd.Add(new THOIKHOABIEU
                            {
                                MaLopHoc = MaLopHoc.Value,
                                NgayHoc = current,
                                CaHoc = ca
                            });
                        }
                        current = current.AddDays(7);
                    }
                }
            }

            if (toAdd.Count > 0)
            {
                _context.THOIKHOABIEU.AddRange(toAdd);
                await _context.SaveChangesAsync();
            }

            if (action == "Lưu và thêm tiếp")
            {
                TempData["SuccessMessage"] = $"Đã lưu {toAdd.Count} lịch học. Bạn có thể thêm tiếp.";
                return RedirectToAction(nameof(Create), new { maLopHoc = MaLopHoc.Value });
            }
            else // "Xác nhận"
            {
                return RedirectToAction("Details", "LOPHOCs", new { id = MaLopHoc.Value });
            }
        }

        // GET: THOIKHOABIEUx/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tHOIKHOABIEU = await _context.THOIKHOABIEU.FindAsync(id);
            if (tHOIKHOABIEU == null) return NotFound();

            ViewData["MaLopHoc"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc", tHOIKHOABIEU.MaLopHoc);
            return View(tHOIKHOABIEU);
        }

        // POST: THOIKHOABIEUx/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            // Lấy đối tượng hiện tại từ DB
            var existing = await _context.THOIKHOABIEU.FindAsync(id);
            if (existing == null) return NotFound();

            // Cập nhật *chỉ* CaHoc từ form POST vào existing
            // "" là prefix (không dùng)
            var updated = await TryUpdateModelAsync<THOIKHOABIEU>(
                existing,
                prefix: "",
                t => t.CaHoc
            );

            // Nếu model binder đã gán giá trị thành công (true) thì kiểm tra thêm
            if (!updated)
            {
                // không bind được (hiếm khi xảy ra), trả về view cùng existing
                ViewData["MaLopHoc"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc", existing.MaLopHoc);
                return View(existing);
            }

            // Server-side validate CaHoc
            if (string.IsNullOrWhiteSpace(existing.CaHoc))
            {
                ModelState.AddModelError("CaHoc", "Vui lòng chọn ca học.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["MaLopHoc"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc", existing.MaLopHoc);
                return View(existing);
            }

            try
            {
                // Ghi thay đổi (chỉ CaHoc đã bị thay đổi trên existing)
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!THOIKHOABIEUExists(existing.MaLichHoc)) return NotFound();
                else throw;
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: THOIKHOABIEUx/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tHOIKHOABIEU = await _context.THOIKHOABIEU
                .Include(t => t.LOPHOC)
                .FirstOrDefaultAsync(m => m.MaLichHoc == id);
            if (tHOIKHOABIEU == null) return NotFound();

            return View(tHOIKHOABIEU);
        }

        // POST: THOIKHOABIEUx/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tkb = await _context.THOIKHOABIEU.FindAsync(id);
            if (tkb == null) return NotFound();

            try
            {
                _context.THOIKHOABIEU.Remove(tkb);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa thời khóa biểu.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Không thể xóa thời khóa biểu do có dữ liệu liên quan hoặc lỗi cơ sở dữ liệu.");
                var item = await _context.THOIKHOABIEU
                    .Include(t => t.LOPHOC)
                    .FirstOrDefaultAsync(t => t.MaLichHoc == id);
                return View("Delete", item);
            }
        }

        private bool THOIKHOABIEUExists(int id)
        {
            return _context.THOIKHOABIEU.Any(e => e.MaLichHoc == id);
        }
    }
}
