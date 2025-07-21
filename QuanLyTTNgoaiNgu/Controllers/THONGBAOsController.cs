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
    public class THONGBAOsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public THONGBAOsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: THONGBAOs
        public async Task<IActionResult> Index()
        {
            return View(await _context.THONGBAO.ToListAsync());
        }

        // GET: THONGBAOs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tHONGBAO = await _context.THONGBAO
                .FirstOrDefaultAsync(m => m.MaThongBao == id);
            if (tHONGBAO == null)
            {
                return NotFound();
            }

            return View(tHONGBAO);
        }

        // GET: THONGBAOs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: THONGBAOs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaThongBao,TieuDe,NoiDung,NgayThongBao,MaTaiKhoan")] THONGBAO tHONGBAO)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tHONGBAO);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tHONGBAO);
        }

        // GET: THONGBAOs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tHONGBAO = await _context.THONGBAO.FindAsync(id);
            if (tHONGBAO == null)
            {
                return NotFound();
            }
            return View(tHONGBAO);
        }

        // POST: THONGBAOs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaThongBao,TieuDe,NoiDung,NgayThongBao,MaTaiKhoan")] THONGBAO tHONGBAO)
        {
            if (id != tHONGBAO.MaThongBao)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tHONGBAO);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!THONGBAOExists(tHONGBAO.MaThongBao))
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
            return View(tHONGBAO);
        }

        // GET: THONGBAOs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tHONGBAO = await _context.THONGBAO
                .FirstOrDefaultAsync(m => m.MaThongBao == id);
            if (tHONGBAO == null)
            {
                return NotFound();
            }

            return View(tHONGBAO);
        }

        // POST: THONGBAOs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tHONGBAO = await _context.THONGBAO.FindAsync(id);
            if (tHONGBAO != null)
            {
                _context.THONGBAO.Remove(tHONGBAO);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool THONGBAOExists(int id)
        {
            return _context.THONGBAO.Any(e => e.MaThongBao == id);
        }
    }
}
