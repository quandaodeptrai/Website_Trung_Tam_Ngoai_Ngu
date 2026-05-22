using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyTTNgoaiNgu.Data;
using QuanLyTTNgoaiNgu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult Index(string searchTitle, string searchContent, string roleFilter)
        {
            var query = _context.THONGBAO.Include(t => t.TaiKhoan).AsQueryable();

            if (!string.IsNullOrEmpty(searchTitle))
                query = query.Where(t => t.TieuDe.Contains(searchTitle));

            if (!string.IsNullOrEmpty(searchContent))
                query = query.Where(t => t.NoiDung.Contains(searchContent));

            if (!string.IsNullOrEmpty(roleFilter))
                query = query.Where(t => t.TaiKhoan.VaiTro == roleFilter);

            // Lấy duy nhất theo TieuDe + NoiDung + VaiTro
            var model = query
                .AsEnumerable() // đưa về memory trước khi group
                .GroupBy(t => new { t.TieuDe, t.NoiDung, VaiTro = t.TaiKhoan?.VaiTro })
                .Select(g => g.First())
                .OrderByDescending(t => t.NgayThongBao)
                .ToList();

            ViewBag.SearchTitle = searchTitle;
            ViewBag.SearchContent = searchContent;
            ViewBag.RoleFilter = roleFilter;
            ViewBag.RoleList = new SelectList(_context.TAIKHOAN.Select(t => t.VaiTro).Distinct());

            return View(model);
        }

        // GET: THONGBAOs/Create
        public IActionResult Create()
        {
            var model = new NotificationViewModel
            {
                Roles = _context.TAIKHOAN.Select(t => t.VaiTro).Distinct().ToList(),
                Accounts = _context.TAIKHOAN
                            .Select(a => new AccountSelect
                            {
                                MaTaiKhoan = a.MaTaiKhoan,
                                TenDangNhap = a.TenDangNhap,
                                VaiTro = a.VaiTro
                            })
                            .ToList()
            };
            return View(model);
        }

        // POST: THONGBAOs/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotificationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Load lại danh sách role & accounts nếu form có lỗi
                model.Roles = _context.TAIKHOAN.Select(t => t.VaiTro).Distinct().ToList();
                model.Accounts = _context.TAIKHOAN
                                .Select(a => new AccountSelect
                                {
                                    MaTaiKhoan = a.MaTaiKhoan,
                                    TenDangNhap = a.TenDangNhap,
                                    VaiTro = a.VaiTro
                                })
                                .ToList();
                return View(model);
            }

            // Thêm thông báo cho từng tài khoản được chọn
            if (model.SelectedAccounts != null && model.SelectedAccounts.Any())
            {
                foreach (var accountId in model.SelectedAccounts)
                {
                    var thongBao = new THONGBAO
                    {
                        TieuDe = model.TieuDe,
                        NoiDung = model.NoiDung,
                        MaTaiKhoan = accountId,
                        NgayThongBao = DateTime.Now
                    };
                    _context.THONGBAO.Add(thongBao);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool THONGBAOExists(int id) => _context.THONGBAO.Any(e => e.MaThongBao == id);
    }
}
