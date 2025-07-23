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
            var list = await _context.LOPHOC
                .Include(l => l.KHOAHOC)
                .Include(l => l.GIANGVIEN)
                .ToListAsync();
            return View(list);
        }

        // GET: LOPHOCs/Create
        public IActionResult Create()
        {
            ViewData["MaKhoaHoc"] = new SelectList(_context.KHOAHOC, "MaKhoaHoc", "TenKhoaHoc");
            ViewData["MaGiangVien"] = new SelectList(_context.GIANGVIEN, "MaGiangVien", "HoTen");
            return View();
        }

        // POST: LOPHOCs/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("TenLopHoc,SLHocVienToiDa,NgayBatDau,NgayKetThuc,MaKhoaHoc,MaGiangVien")]
            LOPHOC model)
        {
            // Xem log ModelState
            if (!ModelState.IsValid)
            {
                // Build lại dropdown để view có thể render
                ViewData["MaKhoaHoc"] = new SelectList(_context.KHOAHOC, "MaKhoaHoc", "TenKhoaHoc", model.MaKhoaHoc);
                ViewData["MaGiangVien"] = new SelectList(_context.GIANGVIEN, "MaGiangVien", "HoTen", model.MaGiangVien);
                return View(model);
            }

            _context.Add(model);
            await _context.SaveChangesAsync();
            // Đảm bảo redirect đúng controller/action
            return RedirectToAction(nameof(Index), "LOPHOCs");
        }


        // GET: LOPHOCs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var lop = await _context.LOPHOC.FindAsync(id);
            if (lop == null) return NotFound();

            ViewData["MaKhoaHoc"] = new SelectList(_context.KHOAHOC, "MaKhoaHoc", "TenKhoaHoc", lop.MaKhoaHoc);
            ViewData["MaGiangVien"] = new SelectList(_context.GIANGVIEN, "MaGiangVien", "HoTen", lop.MaGiangVien);
            return View(lop);
        }

        // POST: LOPHOCs/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("MaLopHoc,TenLopHoc,SLHocVienToiDa,NgayBatDau,NgayKetThuc,MaKhoaHoc,MaGiangVien")] LOPHOC model)
        {
            if (id != model.MaLopHoc) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaKhoaHoc"] = new SelectList(_context.KHOAHOC, "MaKhoaHoc", "TenKhoaHoc", model.MaKhoaHoc);
            ViewData["MaGiangVien"] = new SelectList(_context.GIANGVIEN, "MaGiangVien", "HoTen", model.MaGiangVien);
            return View(model);
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
