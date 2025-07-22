using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuanLyTTNgoaiNgu.Data;
using QuanLyTTNgoaiNgu.Models;
using System.Linq;

namespace QuanLyTTNgoaiNgu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QuanLyTTNgoaiNguContext _ctx;

        public HomeController(ILogger<HomeController> logger,
                              QuanLyTTNgoaiNguContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        // Home chung (ch?a login)
        public IActionResult Index()
        {
            var courses = _ctx.KHOAHOC.ToList();
            return View("Index", courses);
        }

        // Home dành cho Admin
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminHome()
        {
            // b?n có th? load thêm data cho Admin dashboard ? ?ây
            return View("AdminHome");
        }

        // Home dành cho Gi?ng viên
        [Authorize(Policy = "GiangVienOnly")]
        public IActionResult GiangVienHome()
        {
            // load data riêng cho Gi?ng viên
            return View("GiangVienHome");
        }

        // Home dành cho H?c viên
        [Authorize(Policy = "HocVienOnly")]
        public IActionResult HocVienHome()
        {
            // load data riêng cho H?c viên
            return View("HocVienHome");
        }

        public IActionResult Privacy()
            => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
