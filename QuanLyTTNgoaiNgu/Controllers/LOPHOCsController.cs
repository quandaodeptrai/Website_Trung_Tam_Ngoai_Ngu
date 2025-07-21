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
    public class LOPHOCsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public LOPHOCsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: LOPHOCs
        public async Task<IActionResult> Index()
        {
            var quanLyTTNgoaiNguContext = _context.LOPHOC.Include(l => l.GIANGVIEN).Include(l => l.KHOAHOC);
            return View(await quanLyTTNgoaiNguContext.ToListAsync());
        }

        // GET: LOPHOCs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lOPHOC = await _context.LOPHOC
                .Include(l => l.GIANGVIEN)
                .Include(l => l.KHOAHOC)
                .FirstOrDefaultAsync(m => m.MaLopHoc == id);
            if (lOPHOC == null)
            {
                return NotFound();
            }

            return View(lOPHOC);
        }

        // GET: LOPHOCs/Create
        public IActionResult Create()
        {
            ViewData["MaGiangVien"] = new SelectList(_context.GIANGVIEN, "MaGiangVien", "ChuyenMon");
            ViewData["MaKhoaHoc"] = new SelectList(_context.KHOAHOC, "MaKhoaHoc", "MoTa");
            return View();
        }

        // POST: LOPHOCs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLopHoc,TenLopHoc,SLHocVienToiDa,NgayBatDau,NgayKetThuc,MaKhoaHoc,MaGiangVien")] LOPHOC lOPHOC)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lOPHOC);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaGiangVien"] = new SelectList(_context.GIANGVIEN, "MaGiangVien", "ChuyenMon", lOPHOC.MaGiangVien);
            ViewData["MaKhoaHoc"] = new SelectList(_context.KHOAHOC, "MaKhoaHoc", "MoTa", lOPHOC.MaKhoaHoc);
            return View(lOPHOC);
        }

        // GET: LOPHOCs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lOPHOC = await _context.LOPHOC.FindAsync(id);
            if (lOPHOC == null)
            {
                return NotFound();
            }
            ViewData["MaGiangVien"] = new SelectList(_context.GIANGVIEN, "MaGiangVien", "ChuyenMon", lOPHOC.MaGiangVien);
            ViewData["MaKhoaHoc"] = new SelectList(_context.KHOAHOC, "MaKhoaHoc", "MoTa", lOPHOC.MaKhoaHoc);
            return View(lOPHOC);
        }

        // POST: LOPHOCs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLopHoc,TenLopHoc,SLHocVienToiDa,NgayBatDau,NgayKetThuc,MaKhoaHoc,MaGiangVien")] LOPHOC lOPHOC)
        {
            if (id != lOPHOC.MaLopHoc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lOPHOC);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LOPHOCExists(lOPHOC.MaLopHoc))
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
            ViewData["MaGiangVien"] = new SelectList(_context.GIANGVIEN, "MaGiangVien", "ChuyenMon", lOPHOC.MaGiangVien);
            ViewData["MaKhoaHoc"] = new SelectList(_context.KHOAHOC, "MaKhoaHoc", "MoTa", lOPHOC.MaKhoaHoc);
            return View(lOPHOC);
        }

        // GET: LOPHOCs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lOPHOC = await _context.LOPHOC
                .Include(l => l.GIANGVIEN)
                .Include(l => l.KHOAHOC)
                .FirstOrDefaultAsync(m => m.MaLopHoc == id);
            if (lOPHOC == null)
            {
                return NotFound();
            }

            return View(lOPHOC);
        }

        // POST: LOPHOCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lOPHOC = await _context.LOPHOC.FindAsync(id);
            if (lOPHOC != null)
            {
                _context.LOPHOC.Remove(lOPHOC);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LOPHOCExists(int id)
        {
            return _context.LOPHOC.Any(e => e.MaLopHoc == id);
        }
    }
}
