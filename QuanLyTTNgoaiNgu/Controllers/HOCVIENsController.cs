using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyTTNgoaiNgu.Data;
using QuanLyTTNgoaiNgu.Helpers;
using QuanLyTTNgoaiNgu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuanLyTTNgoaiNgu.Controllers
{

    [NoCache]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class HOCVIENsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public HOCVIENsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: HOCVIENs
        // GET: HOCVIENs
        public async Task<IActionResult> Index(string searchString)
        {
            var query = _context.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .Include(h => h.TAIKHOAN)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(h =>
                    h.DANGKYMOI.HoTen.Contains(searchString) ||
                    h.DANGKYMOI.Email.Contains(searchString) ||
                    h.DANGKYMOI.SoDienThoai.Contains(searchString)
                );
            }

            ViewData["CurrentFilter"] = searchString;

            return View(await query.ToListAsync());
        }


        // GET: HOCVIENs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var hocvien = await _context.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .Include(h => h.TAIKHOAN)
                .FirstOrDefaultAsync(m => m.MaHocVien == id);
            if (hocvien == null) return NotFound();

            return View(hocvien);
        }
        // GET: HOCVIENs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var hv = await _context.HOCVIEN
                .Include(h => h.TAIKHOAN)
                .Include(h => h.DANGKYMOI)
                .FirstOrDefaultAsync(h => h.MaHocVien == id);
            if (hv == null) return NotFound();
            return View(hv);
        }

        // POST: HOCVIENs/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // 1) Load học viên cùng navigation tài khoản
            var hocvien = await _context.HOCVIEN
                .Include(h => h.TAIKHOAN)
                .FirstOrDefaultAsync(h => h.MaHocVien == id);

            if (hocvien != null)
            {
                // 2) Xóa tài khoản trước nếu có
                if (hocvien.TAIKHOAN != null)
                {
                    _context.TAIKHOAN.Remove(hocvien.TAIKHOAN);
                }
                // 3) Xóa học viên
                _context.HOCVIEN.Remove(hocvien);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }



        // GET: HOCVIENs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Load entity kèm DANGKYMOI
            var hocvien = await _context.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .FirstOrDefaultAsync(h => h.MaHocVien == id);
            if (hocvien == null) return NotFound();

            // Dropdown tài khoản
            ViewBag.MaTaiKhoan = new SelectList(
                _context.TAIKHOAN,
                "MaTaiKhoan",
                "MaTaiKhoan",
                hocvien.MaTaiKhoan
            );
            return View(hocvien);
        }

        // POST: HOCVIENs/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HOCVIEN postedModel)
        {
            if (id != postedModel.MaHocVien)
                return NotFound();

            // Load entity gốc từ DB
            var hocvien = await _context.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .FirstOrDefaultAsync(h => h.MaHocVien == id);
            if (hocvien == null) return NotFound();

            // Thêm một bước kiểm tra ModelState
            if (!ModelState.IsValid)
            {
                ViewBag.MaTaiKhoan = new SelectList(
                    _context.TAIKHOAN, "MaTaiKhoan", "MaTaiKhoan", postedModel.MaTaiKhoan);
                // Đảm bảo nested được gán lại để view gọi lại không blank
                hocvien.DANGKYMOI = postedModel.DANGKYMOI;
                hocvien.MaTaiKhoan = postedModel.MaTaiKhoan;
                return View(hocvien);
            }

            // Cập nhật khóa tài khoản
            hocvien.MaTaiKhoan = postedModel.MaTaiKhoan;
            // Cập nhật các trường DANGKYMOI từ postedModel
            hocvien.DANGKYMOI.HoTen = postedModel.DANGKYMOI.HoTen;
            hocvien.DANGKYMOI.NgaySinh = postedModel.DANGKYMOI.NgaySinh;
            hocvien.DANGKYMOI.SoDienThoai = postedModel.DANGKYMOI.SoDienThoai;
            hocvien.DANGKYMOI.DiaChi = postedModel.DANGKYMOI.DiaChi;
            hocvien.DANGKYMOI.Email = postedModel.DANGKYMOI.Email;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.HOCVIEN.Any(e => e.MaHocVien == id))
                    return NotFound();
                throw;
            }
        }





        // GET: HOCVIENs/History/5
        public async Task<IActionResult> History(int? id)
        {
            if (id == null) return NotFound();
            var hv = await _context.HOCVIEN
                .Include(h => h.PHIEUDANGKies)
                    .ThenInclude(p => p.LOPHOC)
                        .ThenInclude(l => l.KHOAHOC)
                .Include(h => h.PHIEUDANGKies)
                    .ThenInclude(p => p.HOCPHI)
                .Include(h => h.PHIEUDANGKies)
                    .ThenInclude(p => p.KETQUAHOCTAP)
                .FirstOrDefaultAsync(h => h.MaHocVien == id);
            if (hv == null) return NotFound();
            return View(hv);
        }



        // GET: ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: ChangePassword
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel m)
        {
            if (!ModelState.IsValid)
                return View(m);

            // Lấy username từ claim
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.TAIKHOAN
                .FirstOrDefaultAsync(u => u.TenDangNhap == username);

            if (user == null)
                return RedirectToAction("Login", "Account");

            // Kiểm tra mật khẩu cũ
            if (user.MatKhau != m.OldPassword)
            {
                ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng.");
                return View(m);
            }

            // Cập nhật mật khẩu mới
            user.MatKhau = m.NewPassword;
            _context.Update(user);
            await _context.SaveChangesAsync();

            // Sau khi đổi, sign-out để bắt login lại
            await HttpContext.SignOutAsync();
            TempData["Message"] = "Đổi mật khẩu thành công. Vui lòng đăng nhập lại.";
            return RedirectToAction("Login", "Account");
        }

        // 1) Danh sách khóa học, có hỗ trợ search
        public async Task<IActionResult> Courses(string searchString)
        {
            // 1a. Lấy danh sách course
            var query = _context.KHOAHOC.AsQueryable();

            // 1b. Nếu có search, filter
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(c => c.TenKhoaHoc.Contains(searchString));
            }

            // 1c. Project về ViewModel (hoặc entity nếu bạn muốn)
            var courses = await query
                .Select(c => new CourseViewModel
                {
                    MaKhoaHoc = c.MaKhoaHoc,
                    TenKhoaHoc = c.TenKhoaHoc,
                    MoTa = c.MoTa,
                    MucHocPhi = c.MucHocPhi
                })
                .ToListAsync();

            // 1d. Đưa lại searchString để view giữ input
            ViewData["CurrentFilter"] = searchString;

            return View(courses);
        }

        public async Task<IActionResult> Classes(int courseId)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var hv = await _context.HOCVIEN
                .FirstOrDefaultAsync(h => h.TAIKHOAN.TenDangNhap == username);
            if (hv == null)
                return RedirectToAction(nameof(Courses));

            var hvId = hv.MaHocVien;

            var list = await _context.LOPHOC
                .Where(l => l.MaKhoaHoc == courseId)
                .Include(l => l.THOIKHOABIEUs)
                .Select(l => new ClassInfoViewModel
                {
                    MaLopHoc = l.MaLopHoc,
                    TenLopHoc = l.TenLopHoc,
                    SLHocVienToiDa = l.SLHocVienToiDa,
                    SLHocVienHienTai = l.PHIEUDANGKies.Count(),
                    NgayBatDau = l.NgayBatDau,
                    Schedules = l.THOIKHOABIEUs.ToList(),
                    HasRegistered = _context.PHIEUDANGKY
                                        .Any(p => p.MaHocVien == hvId && p.MaLopHoc == l.MaLopHoc),
                    CanRegister = DateTime.Now < l.NgayBatDau
                                  && l.PHIEUDANGKies.Count() < l.SLHocVienToiDa
                                  && !_context.PHIEUDANGKY
                                        .Any(p => p.MaHocVien == hvId && p.MaLopHoc == l.MaLopHoc)
                })
                .ToListAsync();

            ViewBag.CourseId = courseId;
            return View(list);
        }


        // 3) Đăng ký lớp
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterClass(int classId)
        {
            // 1. Lấy học viên hiện tại
            var username = User.FindFirstValue(ClaimTypes.Name);
            var hv = await _context.HOCVIEN
                .FirstOrDefaultAsync(h => h.TAIKHOAN.TenDangNhap == username);
            if (hv == null) return RedirectToAction(nameof(Courses));

            // 2. Lấy lớp
            var lop = await _context.LOPHOC
                .Include(l => l.PHIEUDANGKies)
                .FirstOrDefaultAsync(l => l.MaLopHoc == classId);
            if (lop == null) return RedirectToAction(nameof(Courses));

            // 3. Kiểm tra điều kiện
            if (DateTime.Now >= lop.NgayBatDau
             || lop.PHIEUDANGKies.Count() >= lop.SLHocVienToiDa
             || _context.PHIEUDANGKY.Any(p => p.MaHocVien == hv.MaHocVien && p.MaLopHoc == classId))
            {
                TempData["Error"] = "Không thể đăng ký lớp này.";
                return RedirectToAction(nameof(Classes), new { courseId = lop.MaKhoaHoc });
            }

            // 4. Tạo phiếu đăng ký
            var phieu = new PHIEUDANGKY
            {
                MaHocVien = hv.MaHocVien,
                MaLopHoc = classId,
                NgayDangKy = DateTime.Now
            };
            _context.PHIEUDANGKY.Add(phieu);
            await _context.SaveChangesAsync(); // để EF gán phieu.MaPhieu

            // 5. Tạo kết quả học tập mặc định (điểm = 0)
            var ketqua = new KETQUAHOCTAP
            {
                MaPhieu = phieu.MaPhieu,
                Diem = 0.0
            };
            _context.KETQUAHOCTAP.Add(ketqua);

            // 6. Tạo học phí mặc định (chưa nộp)
            var hocphi = new HOCPHI
            {
                MaPhieu = phieu.MaPhieu,
                TrangThai = false,
                NgayNop = null
            };
            _context.HOCPHI.Add(hocphi);

            // 7. Lưu mọi thay đổi
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký lớp thành công! Vui lòng kiểm tra công nợ để thanh toán học phí.";
            return RedirectToAction(nameof(Classes), new { courseId = lop.MaKhoaHoc });
        }

        // GET: /HocVien/Debt
        public async Task<IActionResult> Debt()
        {
            // 1. Tìm Học viên hiện tại
            var username = User.FindFirstValue(ClaimTypes.Name);
            var hv = await _context.HOCVIEN
                .FirstOrDefaultAsync(h => h.TAIKHOAN.TenDangNhap == username);
            if (hv == null) return RedirectToAction("Index", "Home");

            // 2. Lấy danh sách HOCPHI kèm PHIEUDANGKY → LOPHOC → KHOAHOC
            var list = await _context.HOCPHI
                .Include(hp => hp.PHIEUDANGKY)
                    .ThenInclude(p => p.LOPHOC)
                        .ThenInclude(l => l.KHOAHOC)
                .Where(hp => hp.PHIEUDANGKY.MaHocVien == hv.MaHocVien)
                .Select(hp => new DebtItemViewModel
                {
                    MaHocPhi = hp.MaHocPhi,
                    TrangThai = hp.TrangThai,
                    NgayNop = hp.NgayNop,
                    TenLopHoc = hp.PHIEUDANGKY.LOPHOC.TenLopHoc,
                    TenKhoaHoc = hp.PHIEUDANGKY.LOPHOC.KHOAHOC.TenKhoaHoc,
                    MucHocPhi = hp.PHIEUDANGKY.LOPHOC.KHOAHOC.MucHocPhi
                })
                .ToListAsync();

            return View(new DebtViewModel { Items = list });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PayDebt(int[] selectedIds, Dictionary<int, double> soTienNhap)
        {
            if (selectedIds == null || selectedIds.Length == 0)
            {
                TempData["DebtError"] = "Vui lòng chọn ít nhất một khoản cần nộp.";
                return RedirectToAction(nameof(Debt));
            }

            var today = DateTime.Now;
            var items = await _context.HOCPHI
                .Include(hp => hp.PHIEUDANGKY)
                    .ThenInclude(p => p.LOPHOC)
                        .ThenInclude(l => l.KHOAHOC)
                .Where(hp => selectedIds.Contains(hp.MaHocPhi))
                .ToListAsync();

            var errors = new List<string>();
            int successCount = 0;

            foreach (var hp in items)
            {
                var mucHocPhi = hp.PHIEUDANGKY?.LOPHOC?.KHOAHOC?.MucHocPhi ?? 0;
                if (!soTienNhap.TryGetValue(hp.MaHocPhi, out var soTien) || soTien != mucHocPhi)
                {
                    errors.Add($"Khoản học phí {hp.MaHocPhi} không hợp lệ (yêu cầu {mucHocPhi:N0} VND).");
                    continue;
                }

                hp.TrangThai = true;
                hp.NgayNop = today;
                successCount++;
            }

            await _context.SaveChangesAsync();

            if (successCount > 0)
                TempData["DebtSuccess"] = $"Đã thanh toán thành công {successCount} khoản.";
            if (errors.Any())
                TempData["DebtError"] = string.Join("<br>", errors);

            return RedirectToAction(nameof(Debt));
        }


        // GET: HOCVIENs/Results or HOCVIENs/Results/{id}
        public async Task<IActionResult> Results(int? id)
        {
            // Nếu không có id, lấy từ user hiện tại
            if (!id.HasValue)
            {
                var username = User.Identity.Name;
                var hv0 = await _context.HOCVIEN
                    .FirstOrDefaultAsync(h => h.TAIKHOAN.TenDangNhap == username);
                if (hv0 == null) return RedirectToAction("Index", "Home");
                id = hv0.MaHocVien;
            }

            // Lấy Học viên theo id
            var hv = await _context.HOCVIEN
                .Include(h => h.TAIKHOAN)
                .FirstOrDefaultAsync(h => h.MaHocVien == id.Value);
            if (hv == null) return NotFound();

            // Load kết quả kèm PHIEUDANGKY → LOPHOC → KHOAHOC và HOCPHI
            var rawList = await _context.KETQUAHOCTAP
                .Include(kq => kq.PHIEUDANGKY)
                    .ThenInclude(p => p.LOPHOC)
                        .ThenInclude(l => l.KHOAHOC)
                .Include(kq => kq.PHIEUDANGKY)
                    .ThenInclude(p => p.HOCPHI)
                .Where(kq => kq.PHIEUDANGKY.MaHocVien == id.Value)
                .ToListAsync();

            // Tạo ViewModel sau khi load xong (in‑memory)
            var vm = rawList.Select(kq =>
            {
                var paid = kq.PHIEUDANGKY.HOCPHI?.TrangThai == true;
                var grade = kq.Diem >= 8 ? "Giỏi"
                          : kq.Diem >= 5 ? "Khá"
                          : "Không qua";
                return new ResultItemViewModel
                {
                    TenKhoaHoc = kq.PHIEUDANGKY.LOPHOC.KHOAHOC.TenKhoaHoc,
                    TenLopHoc = kq.PHIEUDANGKY.LOPHOC.TenLopHoc,
                    Diem = paid ? kq.Diem : 0,
                    XepLoai = paid ? grade : "",
                    CanView = paid
                };
            }).ToList();

            return View(vm);
        }

        public async Task<IActionResult> Schedule(DateTime? date)
        {
            var username = User.Identity.Name;
            var hv = await _context.HOCVIEN
                .FirstOrDefaultAsync(h => h.TAIKHOAN.TenDangNhap == username);
            if (hv == null)
                return RedirectToAction("Index", "Home");

            var classIds = await _context.PHIEUDANGKY
                .Where(p => p.MaHocVien == hv.MaHocVien)
                .Select(p => p.MaLopHoc)
                .Distinct()
                .ToListAsync();

            if (!classIds.Any())
                return View(new List<ScheduleItemViewModel>());

            var rawSchedules = await _context.THOIKHOABIEU
                .Include(t => t.LOPHOC)
                    .ThenInclude(l => l.KHOAHOC)
                .Include(t => t.LOPHOC)
                    .ThenInclude(l => l.GIANGVIEN) // <-- Include giáo viên
                .Where(t => classIds.Contains(t.MaLopHoc))
                .ToListAsync();

            var list = rawSchedules
                .Select(t => new ScheduleItemViewModel
                {
                    TenKhoaHoc = t.LOPHOC.KHOAHOC.TenKhoaHoc,
                    TenLopHoc = t.LOPHOC.TenLopHoc,
                    CaHoc = t.CaHoc,
                    NgayHoc = t.NgayHoc,
                    GiaoVien = t.LOPHOC.GIANGVIEN?.HoTen 
                })
                .OrderBy(si => si.NgayHoc)
                .ThenBy(si => {
                    // Sắp xếp theo thứ tự ca học: 1-3,4-6,7-9,10-12
                    var order = new List<string> { "1-3", "4-6", "7-9", "10-12" };
                    return order.IndexOf(si.CaHoc);
                })
                .ToList();

            ViewData["SelectedDate"] = date ?? DateTime.Now;
            return View(list);
        }




        public async Task<IActionResult> Notifications()
        {
            // 1) Lấy tài khoản hiện tại
            var username = User.Identity.Name;
            var acc = await _context.TAIKHOAN
                .FirstOrDefaultAsync(t => t.TenDangNhap == username);
            if (acc == null) return RedirectToAction("Index", "Home");

            // 2) Lấy danh sách thông báo
            var list = await _context.THONGBAO
                .Where(tb => tb.MaTaiKhoan == acc.MaTaiKhoan)
                .OrderByDescending(tb => tb.NgayThongBao)
                .ToListAsync();

            return View(list);
        }

        // GET: HOCVIENs/ChangeInfor
        public async Task<IActionResult> ChangeInfor()
        {
            // 1. Lấy username
            var username = User.Identity?.Name;
            if (username == null)
                return Challenge();

            // 2. Tìm HOCVIEN kèm bản ghi DangKyMoi
            var hocVien = await _context.HOCVIEN
                .Include(h => h.DANGKYMOI)
                .FirstOrDefaultAsync(h => h.TAIKHOAN.TenDangNhap == username);

            if (hocVien == null || hocVien.DANGKYMOI == null)
                return NotFound();

            // 3. Trả về model là DANGKYMOI để edit
            return View(hocVien.DANGKYMOI);
        }

        // POST: HOCVIENs/ChangeInfor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeInfor(int MaDangKy, [Bind("MaDangKy,HoTen,NgaySinh,SoDienThoai,DiaChi,Email")] DANGKYMOI model)
        {
            if (MaDangKy != model.MaDangKy)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            // Tìm lại bản ghi
            var dk = await _context.DANGKYMOI.FindAsync(MaDangKy);
            if (dk == null)
                return NotFound();

            // Cập nhật
            dk.HoTen = model.HoTen;
            dk.NgaySinh = model.NgaySinh;
            dk.SoDienThoai = model.SoDienThoai;
            dk.DiaChi = model.DiaChi;
            dk.Email = model.Email;

            try
            {
                _context.Update(dk);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật thông tin thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Lỗi khi lưu dữ liệu. Vui lòng thử lại.");
                return View(model);
            }
            return RedirectToAction(nameof(ChangeInfor));
        }
    }
}
