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
  
    [NoCache]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class TAIKHOANsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public TAIKHOANsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: TAIKHOANs
        public async Task<IActionResult> Index()
        {
            return View(await _context.TAIKHOAN.ToListAsync());
        }

        // GET: TAIKHOANs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tAIKHOAN = await _context.TAIKHOAN
                .FirstOrDefaultAsync(m => m.MaTaiKhoan == id);
            if (tAIKHOAN == null)
            {
                return NotFound();
            }

            return View(tAIKHOAN);
        }

        // GET: TAIKHOANs/Create
        // GET: TAIKHOANs/Create
        public IActionResult Create(int? giangVienId, string suggestedUser, string suggestedDisplayName)
        {
            var model = new TAIKHOAN();

            if (!string.IsNullOrEmpty(suggestedUser))
            {
                model.TenDangNhap = suggestedUser;
            }

            // Đưa các thông tin phụ lên ViewBag để hiển thị / dùng hidden field
            ViewBag.GiangVienId = giangVienId;
            ViewBag.SuggestedDisplayName = suggestedDisplayName;

            return View(model);
        }

        // POST: TAIKHOANs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTaiKhoan,TenDangNhap,MatKhau,VaiTro")] TAIKHOAN tAIKHOAN, int? giangVienId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tAIKHOAN);
                await _context.SaveChangesAsync();

                if (giangVienId.HasValue)
                {
                    var gv = await _context.GIANGVIEN.FindAsync(giangVienId.Value);
                    if (gv != null)
                    {
                        // Chỉ cập nhật nếu hiện đang trỏ tới placeholder = 1 (tránh ghi đè nếu đã có account khác)
                        if (gv.MaTaiKhoan == 1)
                        {
                            gv.MaTaiKhoan = tAIKHOAN.MaTaiKhoan;
                            _context.Update(gv);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            // Tuỳ chọn: thêm log hoặc thông báo admin rằng gv đã có MaTaiKhoan khác
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(tAIKHOAN);
        }

        // GET: TAIKHOANs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tAIKHOAN = await _context.TAIKHOAN.FindAsync(id);
            if (tAIKHOAN == null)
            {
                return NotFound();
            }
            return View(tAIKHOAN);
        }

        // POST: TAIKHOANs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTaiKhoan,TenDangNhap,MatKhau,VaiTro")] TAIKHOAN tAIKHOAN)
        {
            if (id != tAIKHOAN.MaTaiKhoan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tAIKHOAN);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TAIKHOANExists(tAIKHOAN.MaTaiKhoan))
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
            return View(tAIKHOAN);
        }

        // GET: TAIKHOANs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tAIKHOAN = await _context.TAIKHOAN
                .FirstOrDefaultAsync(m => m.MaTaiKhoan == id);
            if (tAIKHOAN == null)
            {
                return NotFound();
            }

            return View(tAIKHOAN);
        }

        // POST: TAIKHOANs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tAIKHOAN = await _context.TAIKHOAN.FindAsync(id);
            if (tAIKHOAN != null)
            {
                _context.TAIKHOAN.Remove(tAIKHOAN);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TAIKHOANExists(int id)
        {
            return _context.TAIKHOAN.Any(e => e.MaTaiKhoan == id);
        }
    }
}
