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
            var courseImages = new Dictionary<int, string>
    {
        { 1, "/images/anh.jpg" },
        { 2, "/images/anh.jpg" },
        { 3, "/images/trungquoc.jpg" },
        { 4, "/images/han.jpg" },
        { 5, "/images/nhat.jpg" },
        { 6, "/images/duc.jpg" },
        { 7, "/images/trungquoc.jpg" },
    };

            // Set FeaturedCourses
            ViewBag.FeaturedCourses = courses.Select(c => new
            {
                c.MaKhoaHoc,
                c.TenKhoaHoc,
                c.MoTa,
                Image = courseImages.ContainsKey(c.MaKhoaHoc) ? courseImages[c.MaKhoaHoc] : "/images/anh_trung_tam.jpg"
            }).ToList();

            // Set CarouselImages riêng
            ViewBag.CarouselImages = new List<string> { "/images/anh_trung_tam.jpg" };


            return View();
        }

        // Home d�nh cho Admin
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminHome()
        {
            // b?n c� th? load th�m data cho Admin dashboard ? ?�y
            return View("AdminHome");
        }

        // Home d�nh cho Gi?ng vi�n
        [Authorize(Policy = "GiangVienOnly")]
        public IActionResult GiangVienHome()
        {
            // load data ri�ng cho Gi?ng vi�n
            return View("GiangVienHome");
        }

        // Home d�nh cho H?c vi�n
        [Authorize(Policy = "HocVienOnly")]
        public IActionResult HocVienHome()
        {
            // load data ri�ng cho H?c vi�n
            return View("HocVienHome");
        }

        public IActionResult Privacy()
            => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
