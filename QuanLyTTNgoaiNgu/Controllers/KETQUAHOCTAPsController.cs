using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyTTNgoaiNgu.Data;
using QuanLyTTNgoaiNgu.Models;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace QuanLyTTNgoaiNgu.Controllers
{

    [NoCache]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class KETQUAHOCTAPsController : Controller
    {
        private readonly QuanLyTTNgoaiNguContext _context;

        public KETQUAHOCTAPsController(QuanLyTTNgoaiNguContext context)
        {
            _context = context;
        }

        // GET: KETQUAHOCTAPs
        // GET: KETQUAHOCTAPs
        public async Task<IActionResult> Index()
        {
            var ketQuaHocTap = _context.KETQUAHOCTAP
                .Include(k => k.PHIEUDANGKY)
                    .ThenInclude(p => p.HOCVIEN)
                        .ThenInclude(h => h.DANGKYMOI)
                .Include(k => k.PHIEUDANGKY)
                    .ThenInclude(p => p.LOPHOC); // Include lớp học

            return View(await ketQuaHocTap.ToListAsync());
        }



        // GET: KETQUAHOCTAPs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kETQUAHOCTAP = await _context.KETQUAHOCTAP
                .Include(k => k.PHIEUDANGKY)
                .FirstOrDefaultAsync(m => m.MaKetQua == id);
            if (kETQUAHOCTAP == null)
            {
                return NotFound();
            }

            return View(kETQUAHOCTAP);
        }



        // GET: KETQUAHOCTAPs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kETQUAHOCTAP = await _context.KETQUAHOCTAP.FindAsync(id);
            if (kETQUAHOCTAP == null)
            {
                return NotFound();
            }
            ViewData["MaPhieu"] = new SelectList(_context.Set<PHIEUDANGKY>(), "MaPhieu", "MaPhieu", kETQUAHOCTAP.MaPhieu);
            return View(kETQUAHOCTAP);
        }

        // POST: KETQUAHOCTAPs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKetQua,Diem,MaPhieu")] KETQUAHOCTAP kETQUAHOCTAP)
        {
            if (id != kETQUAHOCTAP.MaKetQua)
            {
                return NotFound();
            }

            // Kiểm tra nếu có lỗi về định dạng float (ModelState không hợp lệ)
            if (!ModelState.IsValid)
            {
                // Nếu lỗi binding do nhập sai định dạng cho Diem
                if (ModelState.ContainsKey("Diem") &&
                    ModelState["Diem"].Errors.Count > 0 &&
                    ModelState["Diem"].Errors[0].ErrorMessage.Contains("not valid"))
                {
                    // Gỡ lỗi cũ và thêm lỗi thân thiện
                    ModelState["Diem"].Errors.Clear();
                    ModelState.AddModelError("Diem", "Vui lòng nhập điểm là số từ 0 đến 10.");
                }

                ViewData["MaPhieu"] = new SelectList(_context.PHIEUDANGKY, "MaPhieu", "MaPhieu", kETQUAHOCTAP.MaPhieu);
                return View(kETQUAHOCTAP);
            }


            try
            {
                _context.Update(kETQUAHOCTAP);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KETQUAHOCTAPExists(kETQUAHOCTAP.MaKetQua))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        public async Task<IActionResult> Export()
        {
            // 1. Lấy dữ liệu cùng các quan hệ
            var data = await _context.KETQUAHOCTAP
                .Include(k => k.PHIEUDANGKY)
                    .ThenInclude(p => p.HOCVIEN)
                        .ThenInclude(h => h.DANGKYMOI)
                .Include(k => k.PHIEUDANGKY)
                    .ThenInclude(p => p.LOPHOC)
                .ToListAsync();

            // 2. Tạo workbook và worksheet
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Kết quả học tập");

            // 3. Ghi header
            ws.Cell(1, 1).Value = "STT";
            ws.Cell(1, 2).Value = "Mã Phiếu";
            ws.Cell(1, 3).Value = "Mã Học viên";
            ws.Cell(1, 4).Value = "Họ tên HV";
            ws.Cell(1, 5).Value = "Lớp học";
            ws.Cell(1, 6).Value = "Điểm";

            // 4. Ghi dữ liệu
            for (int i = 0; i < data.Count; i++)
            {
                var row = i + 2;
                var item = data[i];
                ws.Cell(row, 1).Value = i + 1;
                ws.Cell(row, 2).Value = item.MaKetQua;
                ws.Cell(row, 3).Value = item.PHIEUDANGKY?.MaHocVien;
                ws.Cell(row, 4).Value = item.PHIEUDANGKY?.HOCVIEN?.DANGKYMOI?.HoTen;
                ws.Cell(row, 5).Value = item.PHIEUDANGKY?.LOPHOC?.TenLopHoc;
                ws.Cell(row, 6).Value = item.Diem;
            }

            // 5. Tự động điều chỉnh cột
            ws.Columns().AdjustToContents();

            // 6. Xuất file
            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Seek(0, SeekOrigin.Begin);

            var fileName = $"KetQuaHocTap_{DateTime.Now:yyyyMMdd}.xlsx";
            return File(
                ms.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }

        private bool KETQUAHOCTAPExists(int id)
        {
            return _context.KETQUAHOCTAP.Any(e => e.MaKetQua == id);
        }

    }
}
