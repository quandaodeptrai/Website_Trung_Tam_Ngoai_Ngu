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
    public class HOCPHIsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public HOCPHIsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: HOCPHIs
public async Task<IActionResult> Index()
{
    var hocPhis = await _context.HOCPHI
        .Include(h => h.PHIEUDANGKY)
            .ThenInclude(p => p.HOCVIEN)
                .ThenInclude(hv => hv.DANGKYMOI) // Lấy Họ tên
        .Include(h => h.PHIEUDANGKY)
            .ThenInclude(p => p.LOPHOC)
                .ThenInclude(lh => lh.KHOAHOC) // Lấy học phí
        .ToListAsync();

    return View(hocPhis);
}



        // GET: HOCPHIs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hOCPHI = await _context.HOCPHI
                .Include(h => h.PHIEUDANGKY)
                    .ThenInclude(p => p.HOCVIEN)
                        .ThenInclude(hv => hv.DANGKYMOI)
                .Include(h => h.PHIEUDANGKY)
                    .ThenInclude(p => p.LOPHOC)
                        .ThenInclude(l => l.KHOAHOC) //   để lấy MucHocPhi
                .FirstOrDefaultAsync(m => m.MaHocPhi == id);

            if (hOCPHI == null)
            {
                return NotFound();
            }

            return View(hOCPHI);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(int[] selectedIds)
        {
            if (selectedIds == null || selectedIds.Length == 0)
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một khoản cần xác nhận.";
                return RedirectToAction(nameof(Index));
            }

            var items = await _context.HOCPHI
                .Include(hp => hp.PHIEUDANGKY)
                .Where(hp => selectedIds.Contains(hp.MaHocPhi)
                          && hp.TrangThai == false
                          && hp.NgayNop != null) // chỉ khoản đã nộp mới được xác nhận
                .ToListAsync();

            if (!items.Any())
            {
                TempData["Error"] = "Không có khoản hợp lệ để xác nhận.";
                return RedirectToAction(nameof(Index));
            }

            int count = 0;
            var today = DateTime.Now;

            foreach (var hp in items)
            {
                hp.TrangThai = true;   // ✅ chuyển sang ĐÃ THANH TOÁN
                hp.NgayNop = today;    // cập nhật lại ngày xác nhận
                count++;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã xác nhận thành công {count} khoản học phí.";

            return RedirectToAction(nameof(Index));
        }



    }
}
