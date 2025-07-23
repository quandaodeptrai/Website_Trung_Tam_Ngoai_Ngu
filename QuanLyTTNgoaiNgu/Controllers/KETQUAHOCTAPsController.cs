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
    public class KETQUAHOCTAPsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public KETQUAHOCTAPsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: KETQUAHOCTAPs
        // GET: KETQUAHOCTAPs
        public async Task<IActionResult> Index()
        {
            var ketQuaHocTap = _context.KETQUAHOCTAP
                .Include(k => k.PHIEUDANGKY)
                    .ThenInclude(p => p.HOCVIEN)
                        .ThenInclude(h => h.DANGKYMOI)
                .Include(k => k.PHIEUDANGKY)
                    .ThenInclude(p => p.LOPHOC); // Include lớp học

            return View(await ketQuaHocTap.ToListAsync());
        }



        // GET: KETQUAHOCTAPs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kETQUAHOCTAP = await _context.KETQUAHOCTAP
                .Include(k => k.PHIEUDANGKY)
                .FirstOrDefaultAsync(m => m.MaKetQua == id);
            if (kETQUAHOCTAP == null)
            {
                return NotFound();
            }

            return View(kETQUAHOCTAP);
        }



        // GET: KETQUAHOCTAPs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kETQUAHOCTAP = await _context.KETQUAHOCTAP.FindAsync(id);
            if (kETQUAHOCTAP == null)
            {
                return NotFound();
            }
            ViewData["MaPhieu"] = new SelectList(_context.Set<PHIEUDANGKY>(), "MaPhieu", "MaPhieu", kETQUAHOCTAP.MaPhieu);
            return View(kETQUAHOCTAP);
        }

        // POST: KETQUAHOCTAPs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKetQua,Diem,MaPhieu")] KETQUAHOCTAP kETQUAHOCTAP)
        {
            if (id != kETQUAHOCTAP.MaKetQua)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kETQUAHOCTAP);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KETQUAHOCTAPExists(kETQUAHOCTAP.MaKetQua))
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
            ViewData["MaPhieu"] = new SelectList(_context.Set<PHIEUDANGKY>(), "MaPhieu", "MaPhieu", kETQUAHOCTAP.MaPhieu);
            return View(kETQUAHOCTAP);
        }

        // GET: KETQUAHOCTAPs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kETQUAHOCTAP = await _context.KETQUAHOCTAP
                .Include(k => k.PHIEUDANGKY)
                .FirstOrDefaultAsync(m => m.MaKetQua == id);
            if (kETQUAHOCTAP == null)
            {
                return NotFound();
            }

            return View(kETQUAHOCTAP);
        }

        // POST: KETQUAHOCTAPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kETQUAHOCTAP = await _context.KETQUAHOCTAP.FindAsync(id);
            if (kETQUAHOCTAP != null)
            {
                _context.KETQUAHOCTAP.Remove(kETQUAHOCTAP);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KETQUAHOCTAPExists(int id)
        {
            return _context.KETQUAHOCTAP.Any(e => e.MaKetQua == id);
        }
    }
}
