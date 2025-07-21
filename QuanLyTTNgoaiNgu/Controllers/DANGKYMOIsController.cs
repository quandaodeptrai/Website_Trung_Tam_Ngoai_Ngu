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
    public class DANGKYMOIsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public DANGKYMOIsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: DANGKYMOIs
        public async Task<IActionResult> Index()
        {
            return View(await _context.DANGKYMOI.ToListAsync());
        }

        // GET: DANGKYMOIs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dANGKYMOI = await _context.DANGKYMOI
                .FirstOrDefaultAsync(m => m.MaDangKy == id);
            if (dANGKYMOI == null)
            {
                return NotFound();
            }

            return View(dANGKYMOI);
        }

        // GET: DANGKYMOIs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DANGKYMOIs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDangKy,HoTen,NgaySinh,SoDienThoai,DiaChi,Email,MaQuanTriVien")] DANGKYMOI dANGKYMOI)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dANGKYMOI);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dANGKYMOI);
        }

        // GET: DANGKYMOIs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dANGKYMOI = await _context.DANGKYMOI.FindAsync(id);
            if (dANGKYMOI == null)
            {
                return NotFound();
            }
            return View(dANGKYMOI);
        }

        // POST: DANGKYMOIs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDangKy,HoTen,NgaySinh,SoDienThoai,DiaChi,Email,MaQuanTriVien")] DANGKYMOI dANGKYMOI)
        {
            if (id != dANGKYMOI.MaDangKy)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dANGKYMOI);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DANGKYMOIExists(dANGKYMOI.MaDangKy))
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
            return View(dANGKYMOI);
        }

        // GET: DANGKYMOIs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dANGKYMOI = await _context.DANGKYMOI
                .FirstOrDefaultAsync(m => m.MaDangKy == id);
            if (dANGKYMOI == null)
            {
                return NotFound();
            }

            return View(dANGKYMOI);
        }

        // POST: DANGKYMOIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dANGKYMOI = await _context.DANGKYMOI.FindAsync(id);
            if (dANGKYMOI != null)
            {
                _context.DANGKYMOI.Remove(dANGKYMOI);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DANGKYMOIExists(int id)
        {
            return _context.DANGKYMOI.Any(e => e.MaDangKy == id);
        }
    }
}
