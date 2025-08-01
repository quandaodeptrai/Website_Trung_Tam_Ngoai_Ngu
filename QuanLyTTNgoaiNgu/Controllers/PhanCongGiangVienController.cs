using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLyTTNgoaiNgu.Data;
using QuanLyTTNgoaiNgu.Models;

namespace QuanLyTTNgoaiNgu.Controllers
{
 
    [NoCache]
    public class PhanCongGiangVienController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public PhanCongGiangVienController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PhanCong()
        {
            ViewData["LopHocList"] = new SelectList(_context.LOPHOC, "MaLopHoc", "TenLopHoc");
            var lichDaPhanCong = (from tkb in _context.THOIKHOABIEU
                                  join lop in _context.LOPHOC on tkb.MaLopHoc equals lop.MaLopHoc
                                  join gv in _context.GIANGVIEN on lop.MaGiangVien equals gv.MaGiangVien
                                  select new LichPhanCongItem
                                  {
                                      TenGiangVien = gv.HoTen,
                                      TenLopHoc = lop.TenLopHoc,
                                      NgayHoc = tkb.NgayHoc.Value,
                                      CaHoc = tkb.CaHoc
                                  }).OrderBy(x => x.NgayHoc).ThenBy(x => x.CaHoc).ToList();

            ViewBag.LichDaPhanCong = lichDaPhanCong;

            return View(new PhanCongGiangVienViewModel());
        }
        [HttpPost]
public async Task<IActionResult> ThucHienPhanCong(PhanCongGiangVienViewModel model)
{
    // 1. Khai báo biến để reuse sau này
    bool trung = false;

    // 2. Validation cơ bản
    if (model.MaGiangVienDuocChon == null
        || model.MaLopHoc == 0
        || model.NgayHoc == default)
    {
        ModelState.AddModelError("", "Vui lòng chọn đầy đủ Ngày, Ca học, Lớp và Giảng viên.");
    }
    else
    {
        // 3. Kiểm tra trùng lịch: có lịch nào trong THOIKHOABIEU trùng ngày/ca với giảng viên này?
        trung = (from tkb in _context.THOIKHOABIEU
                 join lopHoc in _context.LOPHOC on tkb.MaLopHoc equals lopHoc.MaLopHoc
                 where tkb.NgayHoc == model.NgayHoc
                    && tkb.CaHoc   == model.CaHoc
                    && lopHoc.MaGiangVien == model.MaGiangVienDuocChon
                 select tkb).Any();

        if (trung)
        {
            ModelState.AddModelError("", "Giảng viên này đã có lịch dạy, vui lòng chọn giảng viên khác!");
        }
        else
        {
            // 4. Cập nhật giảng viên cho lớp
            var lop = await _context.LOPHOC.FindAsync(model.MaLopHoc);
            lop.MaGiangVien = model.MaGiangVienDuocChon.Value;
            _context.Update(lop);

            // 5. Tạo bản ghi lịch mới
            var lichMoi = new THOIKHOABIEU
            {
                MaLopHoc = model.MaLopHoc,
                NgayHoc  = model.NgayHoc,
                CaHoc    = model.CaHoc
            };
            _context.THOIKHOABIEU.Add(lichMoi);

            // 6. Lưu xuống database
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Phân công giảng viên thành công!";
        }
    }

    // 7. Repopulate dropdown và danh sách giảng viên
    ViewData["LopHocList"] = new SelectList(
        _context.LOPHOC, "MaLopHoc", "TenLopHoc", model.MaLopHoc);
    model.DanhSachGiangVienPhuHop = _context.GIANGVIEN.ToList();

    // 8. Repopulate bảng lịch đã phân công
    ViewBag.LichDaPhanCong = (from tkb in _context.THOIKHOABIEU
                              join lopHoc in _context.LOPHOC on tkb.MaLopHoc equals lopHoc.MaLopHoc
                              join gv in _context.GIANGVIEN on lopHoc.MaGiangVien equals gv.MaGiangVien
                              select new LichPhanCongItem
                              {
                                  TenGiangVien = gv.HoTen,
                                  TenLopHoc    = lopHoc.TenLopHoc,
                                  NgayHoc      = tkb.NgayHoc.Value,
                                  CaHoc        = tkb.CaHoc
                              })
                             .OrderBy(x => x.NgayHoc)
                             .ThenBy(x => x.CaHoc)
                             .ToList();

    // 9. Trả về View với đầy đủ model và ViewBag
    return View("PhanCong", model);
}




    }
}
