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
    [Authorize]
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
        public async Task<IActionResult> ThanhToan(ThanhToanViewModel model)
        {
            var hocPhi = await _context.HOCPHI
                .Include(h => h.PHIEUDANGKY)
                    .ThenInclude(p => p.LOPHOC)
                        .ThenInclude(l => l.KHOAHOC)
                .FirstOrDefaultAsync(h => h.MaHocPhi == model.MaHocPhi);

            if (hocPhi == null)
                return NotFound();

            var mucHocPhi = hocPhi.PHIEUDANGKY?.LOPHOC?.KHOAHOC?.MucHocPhi ?? 0;

            if (hocPhi.TrangThai)
            {
                TempData["Error"] = "Học viên này đã thanh toán!";
            }
            else if (model.SoTienNhap != mucHocPhi)
            {
                TempData["Error"] = $"Số tiền không khớp với học phí ({mucHocPhi:N0} VND)";
            }
            else
            {
                // ✅ Cập nhật vào DB
                hocPhi.TrangThai = true;
                hocPhi.NgayNop = DateTime.Now;

                await _context.SaveChangesAsync();

                TempData["Success"] = "Thanh toán thành công!";
            }

            return RedirectToAction("Index");
        }


    }
}
