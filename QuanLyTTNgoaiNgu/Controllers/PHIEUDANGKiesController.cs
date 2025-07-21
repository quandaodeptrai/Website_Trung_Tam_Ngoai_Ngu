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

        // GET: PHIEUDANGKies/Create
        public IActionResult Create()
        {
            ViewData["MaHocVien"] = new SelectList(_context.HOCVIEN, "MaHocVien", "MaHocVien");
            ViewData["MaLopHoc"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc");
            return View();
        }

        // POST: PHIEUDANGKies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhieu,NgayDangKy,MaHocVien,MaLopHoc")] PHIEUDANGKY pHIEUDANGKY)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pHIEUDANGKY);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHocVien"] = new SelectList(_context.HOCVIEN, "MaHocVien", "MaHocVien", pHIEUDANGKY.MaHocVien);
            ViewData["MaLopHoc"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc", pHIEUDANGKY.MaLopHoc);
            return View(pHIEUDANGKY);
        }

        // GET: PHIEUDANGKies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pHIEUDANGKY = await _context.PHIEUDANGKY.FindAsync(id);
            if (pHIEUDANGKY == null)
            {
                return NotFound();
            }
            ViewData["MaHocVien"] = new SelectList(_context.HOCVIEN, "MaHocVien", "MaHocVien", pHIEUDANGKY.MaHocVien);
            ViewData["MaLopHoc"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc", pHIEUDANGKY.MaLopHoc);
            return View(pHIEUDANGKY);
        }

        // POST: PHIEUDANGKies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPhieu,NgayDangKy,MaHocVien,MaLopHoc")] PHIEUDANGKY pHIEUDANGKY)
        {
            if (id != pHIEUDANGKY.MaPhieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pHIEUDANGKY);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PHIEUDANGKYExists(pHIEUDANGKY.MaPhieu))
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
            ViewData["MaHocVien"] = new SelectList(_context.HOCVIEN, "MaHocVien", "MaHocVien", pHIEUDANGKY.MaHocVien);
            ViewData["MaLopHoc"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc", pHIEUDANGKY.MaLopHoc);
            return View(pHIEUDANGKY);
        }

        // GET: PHIEUDANGKies/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: PHIEUDANGKies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pHIEUDANGKY = await _context.PHIEUDANGKY.FindAsync(id);
            if (pHIEUDANGKY != null)
            {
                _context.PHIEUDANGKY.Remove(pHIEUDANGKY);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PHIEUDANGKYExists(int id)
        {
            return _context.PHIEUDANGKY.Any(e => e.MaPhieu == id);
        }
    }
}
