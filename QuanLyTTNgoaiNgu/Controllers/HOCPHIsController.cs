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


     
    }
}
