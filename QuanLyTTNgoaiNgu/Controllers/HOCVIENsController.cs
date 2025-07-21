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
    public class HOCVIENsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public HOCVIENsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: HOCVIENs
        public async Task<IActionResult> Index()
        {
            var quanLyTTNgoaiNguContext = _context.HOCVIEN.Include(h => h.DANGKYMOI).Include(h => h.TAIKHOAN);
            return View(await quanLyTTNgoaiNguContext.ToListAsync());
        }

        // GET: HOCVIENs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hOCVIEN = await _context.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .Include(h => h.TAIKHOAN)
                .FirstOrDefaultAsync(m => m.MaHocVien == id);
            if (hOCVIEN == null)
            {
                return NotFound();
            }

            return View(hOCVIEN);
        }

        // GET: HOCVIENs/Create
        public IActionResult Create()
        {
            ViewData["MaDangKy"] = new SelectList(_context.DANGKYMOI, "MaDangKy", "DiaChi");
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau");
            return View();
        }

        // POST: HOCVIENs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHocVien,MaDangKy,MaTaiKhoan")] HOCVIEN hOCVIEN)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hOCVIEN);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDangKy"] = new SelectList(_context.DANGKYMOI, "MaDangKy", "DiaChi", hOCVIEN.MaDangKy);
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau", hOCVIEN.MaTaiKhoan);
            return View(hOCVIEN);
        }

        // GET: HOCVIENs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hOCVIEN = await _context.HOCVIEN.FindAsync(id);
            if (hOCVIEN == null)
            {
                return NotFound();
            }
            ViewData["MaDangKy"] = new SelectList(_context.DANGKYMOI, "MaDangKy", "DiaChi", hOCVIEN.MaDangKy);
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau", hOCVIEN.MaTaiKhoan);
            return View(hOCVIEN);
        }

        // POST: HOCVIENs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHocVien,MaDangKy,MaTaiKhoan")] HOCVIEN hOCVIEN)
        {
            if (id != hOCVIEN.MaHocVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hOCVIEN);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HOCVIENExists(hOCVIEN.MaHocVien))
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
            ViewData["MaDangKy"] = new SelectList(_context.DANGKYMOI, "MaDangKy", "DiaChi", hOCVIEN.MaDangKy);
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau", hOCVIEN.MaTaiKhoan);
            return View(hOCVIEN);
        }

        // GET: HOCVIENs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hOCVIEN = await _context.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .Include(h => h.TAIKHOAN)
                .FirstOrDefaultAsync(m => m.MaHocVien == id);
            if (hOCVIEN == null)
            {
                return NotFound();
            }

            return View(hOCVIEN);
        }

        // POST: HOCVIENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hOCVIEN = await _context.HOCVIEN.FindAsync(id);
            if (hOCVIEN != null)
            {
                _context.HOCVIEN.Remove(hOCVIEN);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HOCVIENExists(int id)
        {
            return _context.HOCVIEN.Any(e => e.MaHocVien == id);
        }
    }
}
