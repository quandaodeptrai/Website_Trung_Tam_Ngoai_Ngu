using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyTTNgoaiNgu.Data;
using QuanLyTTNgoaiNgu.Models;

namespace QuanLyTTNgoaiNgu.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class ReportsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;
        public ReportsController(QuanLyTTNgoaiNguContext context)
            => _context = context;

        // GET: Reports
        // Tham số startDate/endDate để lọc doanh thu theo kỳ
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var vm = new ReportViewModel();

            // 1. Tổng số lượng
            vm.TotalStudents = await _context.HOCVIEN.CountAsync();
            vm.TotalTeachers = await _context.GIANGVIEN.CountAsync();
            vm.TotalClasses = await _context.LOPHOC.CountAsync();

            // 2. Số học viên từng lớp
            vm.StudentsPerClass = await _context.LOPHOC
                .Select(l => new ClassStudentCount
                {
                    ClassName = l.TenLopHoc,
                    Count = l.PHIEUDANGKies.Count()
                }).ToListAsync();

            // 3. Số lớp từng khóa
            vm.ClassesPerCourse = await _context.KHOAHOC
                .Select(c => new CourseClassCount
                {
                    CourseName = c.TenKhoaHoc,
                    Count = c.LOPHOCs.Count()
                }).ToListAsync();

            // 4. Số học viên từng khóa
            vm.StudentsPerCourse = await _context.KHOAHOC
                .Select(c => new CourseStudentCount
                {
                    CourseName = c.TenKhoaHoc,
                    Count = c.LOPHOCs
                             .SelectMany(l => l.PHIEUDANGKies)
                             .Select(p => p.MaHocVien)
                             .Distinct()
                             .Count()
                }).ToListAsync();

            // 5. Số học viên chưa đăng ký lớp nào
            vm.UnenrolledStudents = await _context.HOCVIEN
                .CountAsync(h => !_context.PHIEUDANGKY
                    .Any(p => p.MaHocVien == h.MaHocVien));

            // 6. Tổng doanh thu
            var payments = _context.HOCPHI
                .Where(hp => hp.TrangThai); // chỉ những học phí đã nộp

            // Theo lớp
            vm.RevenuePerClass = await payments
                .GroupBy(hp => hp.PHIEUDANGKY.LOPHOC.TenLopHoc)
                .Select(g => new RevenuePerGroup
                {
                    Key = g.Key,
                    Amount = g.Sum(hp => hp.PHIEUDANGKY.LOPHOC.KHOAHOC.MucHocPhi)
                })
                .ToListAsync();

            // Theo khóa
            vm.RevenuePerCourse = await payments
                .GroupBy(hp => hp.PHIEUDANGKY.LOPHOC.KHOAHOC.TenKhoaHoc)
                .Select(g => new RevenuePerGroup
                {
                    Key = g.Key,
                    Amount = g.Sum(hp => hp.PHIEUDANGKY.LOPHOC.KHOAHOC.MucHocPhi)
                })
                .ToListAsync();

            return View(vm);
        }
    }
}
