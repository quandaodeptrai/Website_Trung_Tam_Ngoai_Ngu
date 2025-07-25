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
    public class PHIEUDANGKiesController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public PHIEUDANGKiesController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: PHIEUDANGKies
        public async Task<IActionResult> Index()
        {
            var quanLyTTNgoaiNguContext = _context.PHIEUDANGKY.Include(p => p.HOCVIEN).Include(p => p.LOPHOC);
            return View(await quanLyTTNgoaiNguContext.ToListAsync());
        }

        // GET: PHIEUDANGKies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pHIEUDANGKY = await _context.PHIEUDANGKY
                .Include(p => p.HOCVIEN)
                .Include(p => p.LOPHOC)
                .FirstOrDefaultAsync(m => m.MaPhieu == id);
            if (pHIEUDANGKY == null)
            {
                return NotFound();
            }

            return View(pHIEUDANGKY);
        }

     
    }
}
