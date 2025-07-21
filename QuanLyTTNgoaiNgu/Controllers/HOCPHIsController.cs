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
            var quanLyTTNgoaiNguContext = _context.HOCPHI.Include(h => h.PHIEUDANGKY);
            return View(await quanLyTTNgoaiNguContext.ToListAsync());
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
                .FirstOrDefaultAsync(m => m.MaHocPhi == id);
            if (hOCPHI == null)
            {
                return NotFound();
            }

            return View(hOCPHI);
        }

        // GET: HOCPHIs/Create
        public IActionResult Create()
        {
            ViewData["MaPhieu"] = new SelectList(_context.Set<PHIEUDANGKY>(), "MaPhieu", "MaPhieu");
            return View();
        }

        // POST: HOCPHIs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHocPhi,NgayNop,TrangThai,MaPhieu")] HOCPHI hOCPHI)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hOCPHI);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaPhieu"] = new SelectList(_context.Set<PHIEUDANGKY>(), "MaPhieu", "MaPhieu", hOCPHI.MaPhieu);
            return View(hOCPHI);
        }

        // GET: HOCPHIs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hOCPHI = await _context.HOCPHI.FindAsync(id);
            if (hOCPHI == null)
            {
                return NotFound();
            }
            ViewData["MaPhieu"] = new SelectList(_context.Set<PHIEUDANGKY>(), "MaPhieu", "MaPhieu", hOCPHI.MaPhieu);
            return View(hOCPHI);
        }

        // POST: HOCPHIs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHocPhi,NgayNop,TrangThai,MaPhieu")] HOCPHI hOCPHI)
        {
            if (id != hOCPHI.MaHocPhi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hOCPHI);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HOCPHIExists(hOCPHI.MaHocPhi))
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
            ViewData["MaPhieu"] = new SelectList(_context.Set<PHIEUDANGKY>(), "MaPhieu", "MaPhieu", hOCPHI.MaPhieu);
            return View(hOCPHI);
        }

        // GET: HOCPHIs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hOCPHI = await _context.HOCPHI
                .Include(h => h.PHIEUDANGKY)
                .FirstOrDefaultAsync(m => m.MaHocPhi == id);
            if (hOCPHI == null)
            {
                return NotFound();
            }

            return View(hOCPHI);
        }

        // POST: HOCPHIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hOCPHI = await _context.HOCPHI.FindAsync(id);
            if (hOCPHI != null)
            {
                _context.HOCPHI.Remove(hOCPHI);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HOCPHIExists(int id)
        {
            return _context.HOCPHI.Any(e => e.MaHocPhi == id);
        }
    }
}
