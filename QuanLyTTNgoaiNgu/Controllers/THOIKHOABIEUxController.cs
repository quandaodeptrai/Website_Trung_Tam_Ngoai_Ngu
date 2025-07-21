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
            if (id == null)
            {
                return NotFound();
            }

            var tHOIKHOABIEU = await _context.THOIKHOABIEU
                .Include(t => t.LOPHOC)
                .FirstOrDefaultAsync(m => m.MaLichHoc == id);
            if (tHOIKHOABIEU == null)
            {
                return NotFound();
            }

            return View(tHOIKHOABIEU);
        }

        // GET: THOIKHOABIEUx/Create
        public IActionResult Create()
        {
            ViewData["MaLopHoc"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc");
            return View();
        }

        // POST: THOIKHOABIEUx/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLichHoc,CaHoc,NgayHoc,MaLopHoc")] THOIKHOABIEU tHOIKHOABIEU)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tHOIKHOABIEU);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLopHoc"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc", tHOIKHOABIEU.MaLopHoc);
            return View(tHOIKHOABIEU);
        }

        // GET: THOIKHOABIEUx/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tHOIKHOABIEU = await _context.THOIKHOABIEU.FindAsync(id);
            if (tHOIKHOABIEU == null)
            {
                return NotFound();
            }
            ViewData["MaLopHoc"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc", tHOIKHOABIEU.MaLopHoc);
            return View(tHOIKHOABIEU);
        }

        // POST: THOIKHOABIEUx/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLichHoc,CaHoc,NgayHoc,MaLopHoc")] THOIKHOABIEU tHOIKHOABIEU)
        {
            if (id != tHOIKHOABIEU.MaLichHoc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tHOIKHOABIEU);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!THOIKHOABIEUExists(tHOIKHOABIEU.MaLichHoc))
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
            ViewData["MaLopHoc"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc", tHOIKHOABIEU.MaLopHoc);
            return View(tHOIKHOABIEU);
        }

        // GET: THOIKHOABIEUx/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tHOIKHOABIEU = await _context.THOIKHOABIEU
                .Include(t => t.LOPHOC)
                .FirstOrDefaultAsync(m => m.MaLichHoc == id);
            if (tHOIKHOABIEU == null)
            {
                return NotFound();
            }

            return View(tHOIKHOABIEU);
        }

        // POST: THOIKHOABIEUx/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tHOIKHOABIEU = await _context.THOIKHOABIEU.FindAsync(id);
            if (tHOIKHOABIEU != null)
            {
                _context.THOIKHOABIEU.Remove(tHOIKHOABIEU);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool THOIKHOABIEUExists(int id)
        {
            return _context.THOIKHOABIEU.Any(e => e.MaLichHoc == id);
        }
    }
}
