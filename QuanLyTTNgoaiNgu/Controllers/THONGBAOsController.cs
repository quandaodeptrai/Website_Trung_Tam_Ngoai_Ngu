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

        // GET: ThongBao/Create
        public IActionResult Create()
        {
            var vm = new NotificationViewModel
            {
                Accounts = _context.TAIKHOAN
                    .Select(t => new AccountSelect
                    {
                        MaTaiKhoan = t.MaTaiKhoan,
                        TenDangNhap = t.TenDangNhap,
                        VaiTro = t.VaiTro
                    })
                    .ToList()
            };
            return View(vm);
        }

        // POST: ThongBao/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotificationViewModel vm)
        {
            // Luôn nạp lại danh sách tài khoản để hiển thị lại view nếu có lỗi
            vm.Accounts = _context.TAIKHOAN
                .Select(t => new AccountSelect
                {
                    MaTaiKhoan = t.MaTaiKhoan,
                    TenDangNhap = t.TenDangNhap,
                    VaiTro = t.VaiTro
                })
                .ToList();

            // ✅ Kiểm tra model hợp lệ
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var now = DateTime.Now;
            foreach (var accId in vm.SelectedAccounts)
            {
                _context.THONGBAO.Add(new THONGBAO
                {
                    TieuDe = vm.TieuDe,
                    NoiDung = vm.NoiDung,
                    NgayThongBao = now,
                    MaTaiKhoan = accId
                });
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
