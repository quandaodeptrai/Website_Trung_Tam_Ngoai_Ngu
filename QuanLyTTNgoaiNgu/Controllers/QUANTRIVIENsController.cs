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
    [Authorize(Roles = "Admin")]
    [Authorize]
    [NoCache]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class QUANTRIVIENsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public QUANTRIVIENsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: QUANTRIVIENs
        public async Task<IActionResult> Index()
        {
            var quanLyTTNgoaiNguContext = _context.QUANTRIVIEN.Include(q => q.TaiKhoan);
            return View(await quanLyTTNgoaiNguContext.ToListAsync());
        }

        // GET: QUANTRIVIENs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qUANTRIVIEN = await _context.QUANTRIVIEN
                .Include(q => q.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaQuanTriVien == id);
            if (qUANTRIVIEN == null)
            {
                return NotFound();
            }

            return View(qUANTRIVIEN);
        }

        // GET: QUANTRIVIENs/Create
        public IActionResult Create()
        {
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau");
            return View();
        }

        // POST: QUANTRIVIENs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaQuanTriVien,TenQuanTriVien,MaTaiKhoan")] QUANTRIVIEN qUANTRIVIEN)
        {
            if (ModelState.IsValid)
            {
                _context.Add(qUANTRIVIEN);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau", qUANTRIVIEN.MaTaiKhoan);
            return View(qUANTRIVIEN);
        }

        // GET: QUANTRIVIENs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qUANTRIVIEN = await _context.QUANTRIVIEN.FindAsync(id);
            if (qUANTRIVIEN == null)
            {
                return NotFound();
            }
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau", qUANTRIVIEN.MaTaiKhoan);
            return View(qUANTRIVIEN);
        }

        // POST: QUANTRIVIENs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaQuanTriVien,TenQuanTriVien,MaTaiKhoan")] QUANTRIVIEN qUANTRIVIEN)
        {
            if (id != qUANTRIVIEN.MaQuanTriVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(qUANTRIVIEN);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QUANTRIVIENExists(qUANTRIVIEN.MaQuanTriVien))
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
            ViewData["MaTaiKhoan"] = new SelectList(_context.Set<TAIKHOAN>(), "MaTaiKhoan", "MatKhau", qUANTRIVIEN.MaTaiKhoan);
            return View(qUANTRIVIEN);
        }

        // GET: QUANTRIVIENs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qUANTRIVIEN = await _context.QUANTRIVIEN
                .Include(q => q.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaQuanTriVien == id);
            if (qUANTRIVIEN == null)
            {
                return NotFound();
            }

            return View(qUANTRIVIEN);
        }

        // POST: QUANTRIVIENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qUANTRIVIEN = await _context.QUANTRIVIEN.FindAsync(id);
            if (qUANTRIVIEN != null)
            {
                _context.QUANTRIVIEN.Remove(qUANTRIVIEN);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QUANTRIVIENExists(int id)
        {
            return _context.QUANTRIVIEN.Any(e => e.MaQuanTriVien == id);
        }
    }
}
