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
    public class GIANGVIENsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public GIANGVIENsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: GIANGVIENs
        public async Task<IActionResult> Index()
        {
            var quanLyTTNgoaiNguContext = _context.GIANGVIEN.Include(g => g.TAIKHOAN);
            return View(await quanLyTTNgoaiNguContext.ToListAsync());
        }

        // GET: GIANGVIENs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gIANGVIEN = await _context.GIANGVIEN
                .Include(g => g.TAIKHOAN)
                .FirstOrDefaultAsync(m => m.MaGiangVien == id);
            if (gIANGVIEN == null)
            {
                return NotFound();
            }

            return View(gIANGVIEN);
        }

        // GET: GIANGVIENs/Create
        public IActionResult Create()
        {
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau");
            return View();
        }

        // POST: GIANGVIENs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaGiangVien,HoTen,ChuyenMon,Email,SoDienThoai,MaTaiKhoan")] GIANGVIEN gIANGVIEN)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gIANGVIEN);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau", gIANGVIEN.MaTaiKhoan);
            return View(gIANGVIEN);
        }

        // GET: GIANGVIENs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gIANGVIEN = await _context.GIANGVIEN.FindAsync(id);
            if (gIANGVIEN == null)
            {
                return NotFound();
            }
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau", gIANGVIEN.MaTaiKhoan);
            return View(gIANGVIEN);
        }

        // POST: GIANGVIENs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaGiangVien,HoTen,ChuyenMon,Email,SoDienThoai,MaTaiKhoan")] GIANGVIEN gIANGVIEN)
        {
            if (id != gIANGVIEN.MaGiangVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gIANGVIEN);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GIANGVIENExists(gIANGVIEN.MaGiangVien))
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
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau", gIANGVIEN.MaTaiKhoan);
            return View(gIANGVIEN);
        }

        // GET: GIANGVIENs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gIANGVIEN = await _context.GIANGVIEN
                .Include(g => g.TAIKHOAN)
                .FirstOrDefaultAsync(m => m.MaGiangVien == id);
            if (gIANGVIEN == null)
            {
                return NotFound();
            }

            return View(gIANGVIEN);
        }

        // POST: GIANGVIENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gIANGVIEN = await _context.GIANGVIEN.FindAsync(id);
            if (gIANGVIEN != null)
            {
                _context.GIANGVIEN.Remove(gIANGVIEN);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GIANGVIENExists(int id)
        {
            return _context.GIANGVIEN.Any(e => e.MaGiangVien == id);
        }
    }
}
