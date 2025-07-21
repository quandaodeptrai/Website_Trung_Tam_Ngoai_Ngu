using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLyTTNgoaiNgu.Models;

namespace QuanLyTTNgoaiNgu.Data
{
    public class QuanLyTTNgoaiNguContext : DbContext
    {
        public QuanLyTTNgoaiNguContext (DbContextOptions<QuanLyTTNgoaiNguContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Dang ky moi
            modelBuilder.Entity<DANGKYMOI>().HasData(
                new { MaDangKy = 1, HoTen = "Phạm Thị Mai", NgaySinh = new DateTime(2003, 4, 5), SoDienThoai = "123456", DiaChi = "Hanoi", Email = "mai@gmail.com", MaQuanTriVien = 1 },
                new { MaDangKy = 2, HoTen = "Lê Văn Nam", NgaySinh = new DateTime(2004, 3, 5), SoDienThoai = "123654", DiaChi = "ThaiNguyen", Email = "nam@gmail.com", MaQuanTriVien = 1 },
                new { MaDangKy = 3, HoTen = "Vũ Thị Thu", NgaySinh = new DateTime(2005, 4, 3), SoDienThoai = "321456", DiaChi = "NamDinh", Email = "thu@gmail.com", MaQuanTriVien = 2 },
                new { MaDangKy = 4, HoTen = "Ngô Đức Long", NgaySinh = new DateTime(2003, 5, 4), SoDienThoai = "456123", DiaChi = "HaiDuong", Email = "long@gmail.com",  MaQuanTriVien = 2 },
                new { MaDangKy = 5, HoTen = "Trần Hồng Nhung", NgaySinh = new DateTime(2004, 6, 5), SoDienThoai = "456321", DiaChi = "PhuTho", Email = "nhung@gmail.com",  MaQuanTriVien = 3 },
                new { MaDangKy = 6, HoTen = "Nguyễn Thị Lý", NgaySinh = new DateTime(2005, 7, 6), SoDienThoai = "145236", DiaChi = "HaiPhong", Email = "ly@gmail.com", MaQuanTriVien = 1 },
                new { MaDangKy = 7, HoTen = "Dương Đức Tin", NgaySinh = new DateTime(2003, 8, 7), SoDienThoai = "652341", DiaChi = "NinhBinh", Email = "duongductin02@gmail.com", MaQuanTriVien = 3 }
                );
            //Giang vien
            modelBuilder.Entity<GIANGVIEN>().HasData(
                new { MaGiangVien = 1, HoTen = "Cao Đình Trung", ChuyenMon = "Tiếng Anh", Email = "trung@gmail.com", SoDienThoai = "987654", MaTaiKhoan = 11 },
                new { MaGiangVien = 2, HoTen = "Dương Văn Điệp", ChuyenMon = "Tiếng Anh", Email = "diep@gmail.com", SoDienThoai = "987456", MaTaiKhoan = 12 },
                new { MaGiangVien = 3, HoTen = "Trần Thị Bích", ChuyenMon = "Tiếng Trung", Email = "bich@gmail.com", SoDienThoai = "456789", MaTaiKhoan = 13 },
                new { MaGiangVien = 4, HoTen = "Ngô Thanh Vân", ChuyenMon = "Tiếng Hàn", Email = "van@gmail.com", SoDienThoai = "456987", MaTaiKhoan = 14 },
                new { MaGiangVien = 5, HoTen = "Vũ Quang Hiệp", ChuyenMon = "Tiếng Nhật", Email = "hiep@gmail.com", SoDienThoai = "965874", MaTaiKhoan = 15 },
                new { MaGiangVien = 6, HoTen = "Lê Minh Tâm", ChuyenMon = "Tiếng Đức", Email = "tam@gmail.com", SoDienThoai = "478569", MaTaiKhoan = 16 },
                new { MaGiangVien = 7, HoTen = "Trần Văn Ngọc", ChuyenMon = "Tiếng Trung", Email = "ngoc@gmail.com", SoDienThoai = "785496", MaTaiKhoan = 17 }
                );
            //Hoc Phi
            modelBuilder.Entity<HOCPHI>().HasData(
                new { MaHocPhi = 1, NgayNop = new DateTime(2025, 9, 20), TrangThai = true, MaPhieu = 1 },
                new { MaHocPhi = 2, NgayNop = new DateTime(2025, 9, 15), TrangThai = true, MaPhieu = 2 },
                new { MaHocPhi = 3, NgayNop = (DateTime?)null, TrangThai = false, MaPhieu = 3 },
                new { MaHocPhi = 4, NgayNop = new DateTime(2025, 9, 5), TrangThai = true, MaPhieu = 4 },
                new { MaHocPhi = 5, NgayNop = (DateTime?)null, TrangThai = false, MaPhieu = 5 },
                new { MaHocPhi = 6, NgayNop = new DateTime(2025, 9, 20), TrangThai = true, MaPhieu = 6 },
                new { MaHocPhi = 7, NgayNop = new DateTime(2025, 9, 15), TrangThai = true, MaPhieu = 7 }
                );
            //Hoc Vien
            modelBuilder.Entity<HOCVIEN>().HasData(
                 new { MaHocVien = 1, MaDangKy = 1, MaTaiKhoan = 21 },
                new { MaHocVien = 2, MaDangKy = 2, MaTaiKhoan = 22 },
                new { MaHocVien = 3, MaDangKy = 3, MaTaiKhoan = 23 },
                new { MaHocVien = 4, MaDangKy = 4, MaTaiKhoan = 24 },
                new { MaHocVien = 5, MaDangKy = 5, MaTaiKhoan = 25 },
                new { MaHocVien = 6, MaDangKy = 6, MaTaiKhoan = 26 },
                new { MaHocVien = 7, MaDangKy = 7, MaTaiKhoan = 27 }
                );
            //Ket Qua Hoc Tap
            modelBuilder.Entity<KETQUAHOCTAP>().HasData(
                new { MaKetQua = 1, Diem = 8.5, MaPhieu = 1 },
                new { MaKetQua = 2, Diem = 4.0, MaPhieu = 2 },
                new { MaKetQua = 3, Diem = 7.5, MaPhieu = 3 },
                new { MaKetQua = 4, Diem = 5.0, MaPhieu = 4 },
                new { MaKetQua = 5, Diem = 6.5, MaPhieu = 5 },
                new { MaKetQua = 6, Diem = 8.0, MaPhieu = 6 },
                new { MaKetQua = 7, Diem = 9.0, MaPhieu = 7 }
                );
            //Khoa Hoc
            modelBuilder.Entity<KHOAHOC>().HasData(
                new { MaKhoaHoc = 1, TenKhoaHoc = "Tiếng Anh Cơ Bản", MoTa = "Khóa học tiếng Anh cơ bản dành cho người mới bắt đầu.", MucHocPhi = (double)2000000 },
                new { MaKhoaHoc = 2, TenKhoaHoc = "Tiếng Anh Nâng Cao", MoTa = "Khóa học tiếng Anh nâng cao cho người đã có kiến thức cơ bản.", MucHocPhi = (double)3000000 },
                new { MaKhoaHoc = 3, TenKhoaHoc = "Tiếng Trung Cơ Bản", MoTa = "Khóa học tiếng Trung cơ bản dành cho người mới bắt đầu.", MucHocPhi = (double)2500000 },
                new { MaKhoaHoc = 4, TenKhoaHoc = "Tiếng Hàn Cơ Bản", MoTa = "Khóa học tiếng Hàn cơ bản dành cho người mới bắt đầu.", MucHocPhi = (double)2800000 },
                new { MaKhoaHoc = 5, TenKhoaHoc = "Tiếng Nhật Cơ Bản", MoTa = "Khóa học tiếng Nhật cơ bản dành cho người mới bắt đầu.", MucHocPhi = (double)2700000 },
                new { MaKhoaHoc = 6, TenKhoaHoc = "Tiếng Đức Cơ Bản", MoTa = "Khóa học tiếng Đức cơ bản dành cho người mới bắt đầu.", MucHocPhi = (double)2600000 },
                new { MaKhoaHoc = 7, TenKhoaHoc = "Tiếng Trung Nâng Cao", MoTa = "Khóa học tiếng Trung nâng cao cho người đã có kiến thức cơ bản.", MucHocPhi = (double)3500000 }
                );
            //Lop Hoc
            modelBuilder.Entity<LOPHOC>().HasData(
                new { MaLopHoc = 1, TenLopHoc = "Lớp Tiếng Anh Cơ Bản 1", SLHocVienToiDa = 50, NgayBatDau = new DateTime(2025, 9, 1), NgayKetThuc = new DateTime(2025, 12, 31), MaKhoaHoc = 1, MaGiangVien = 1 },
                new { MaLopHoc = 2, TenLopHoc = "Lớp Tiếng Anh Nâng Cao 1", SLHocVienToiDa = 50, NgayBatDau = new DateTime(2025, 9, 1), NgayKetThuc = new DateTime(2026, 1, 31), MaKhoaHoc = 2, MaGiangVien = 2 },
                new { MaLopHoc = 3, TenLopHoc = "Lớp Tiếng Trung Cơ Bản 1", SLHocVienToiDa = 50, NgayBatDau = new DateTime(2025, 9, 1), NgayKetThuc = new DateTime(2026, 1, 28), MaKhoaHoc = 3, MaGiangVien = 3 },
                new { MaLopHoc = 4, TenLopHoc = "Lớp Tiếng Hàn Cơ Bản 1", SLHocVienToiDa = 50, NgayBatDau = new DateTime(2025, 9, 1), NgayKetThuc = new DateTime(2026, 1, 31), MaKhoaHoc = 4, MaGiangVien = 4 },
                new { MaLopHoc = 5, TenLopHoc = "Lớp Tiếng Nhật Cơ Bản 1", SLHocVienToiDa = 50, NgayBatDau = new DateTime(2025, 9, 1), NgayKetThuc = new DateTime(2026, 1, 30), MaKhoaHoc = 5, MaGiangVien = 5 },
                new { MaLopHoc = 6, TenLopHoc = "Lớp Tiếng Đức Cơ Bản 1", SLHocVienToiDa = 50, NgayBatDau = new DateTime(2025, 9, 1), NgayKetThuc = new DateTime(2026, 1, 31), MaKhoaHoc = 6, MaGiangVien = 6 },
                new { MaLopHoc = 7, TenLopHoc = "Lớp Tiếng Trung Nâng Cao 1", SLHocVienToiDa = 50, NgayBatDau = new DateTime(2025, 9, 1), NgayKetThuc = new DateTime(2026, 1, 30), MaKhoaHoc = 7, MaGiangVien = 7 }
                );
            //Phieu Dang Ky
            modelBuilder.Entity<PHIEUDANGKY>().HasData(
                new { MaPhieu = 1, NgayDangKy = new DateTime(2025, 7, 1), MaHocVien = 1, MaLopHoc = 1 },
                new { MaPhieu = 2, NgayDangKy = new DateTime(2025, 8, 1), MaHocVien = 2, MaLopHoc = 2 },
                new { MaPhieu = 3, NgayDangKy = new DateTime(2025, 9, 1), MaHocVien = 3, MaLopHoc = 3 },
                new { MaPhieu = 4, NgayDangKy = new DateTime(2025, 10, 1), MaHocVien = 4, MaLopHoc = 4 },
                new { MaPhieu = 5, NgayDangKy = new DateTime(2025, 11, 1), MaHocVien = 5, MaLopHoc = 5 },
                new { MaPhieu = 6, NgayDangKy = new DateTime(2025, 12, 1), MaHocVien = 6, MaLopHoc = 6 },
                new { MaPhieu = 7, NgayDangKy = new DateTime(2026, 1, 1), MaHocVien = 7, MaLopHoc = 7 }
                );
            // Cấu hình Disable Cascade Delete cho PHIEUDANGKY → HOCVIEN
            modelBuilder.Entity<PHIEUDANGKY>()
                .HasOne(p => p.HOCVIEN)
                .WithMany(h => h.PHIEUDANGKies)
                .HasForeignKey(p => p.MaHocVien)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình Disable Cascade Delete cho PHIEUDANGKY → LOPHOC
            modelBuilder.Entity<PHIEUDANGKY>()
                .HasOne(p => p.LOPHOC)
                .WithMany(l => l.PHIEUDANGKies)
                .HasForeignKey(p => p.MaLopHoc)
                .OnDelete(DeleteBehavior.Restrict);
            //Quan Tri Vien
            modelBuilder.Entity<QUANTRIVIEN>().HasData(
                new {MaQuanTriVien=1, TenQuanTriVien="Đào Văn Quân", MaTaiKhoan = 1 },
                new { MaQuanTriVien = 2, TenQuanTriVien = "Vũ Thanh Ngọc", MaTaiKhoan = 2 },
                new { MaQuanTriVien = 3, TenQuanTriVien = "Dương Đức Tín", MaTaiKhoan = 3 },
                new { MaQuanTriVien = 4, TenQuanTriVien = "Nguyễn Viết Trường", MaTaiKhoan = 4 },
                new { MaQuanTriVien = 5, TenQuanTriVien = "Nguyễn Anh Tuấn", MaTaiKhoan = 5 }
                );
            //Tai Khoan
            modelBuilder.Entity<TAIKHOAN>().HasData(
              // Quan Tri Vien (1-10)
              new { MaTaiKhoan = 1, TenDangNhap = "admin1", MatKhau = "admin123", VaiTro = "Admin" },
              new { MaTaiKhoan = 2, TenDangNhap = "admin2", MatKhau = "admin123", VaiTro = "Admin" },
              new { MaTaiKhoan = 3, TenDangNhap = "admin3", MatKhau = "admin123", VaiTro = "Admin" },
              new { MaTaiKhoan = 4, TenDangNhap = "admin4", MatKhau = "admin123", VaiTro = "Admin" },
              new { MaTaiKhoan = 5, TenDangNhap = "admin5", MatKhau = "admin123", VaiTro = "Admin" },
              // Giang Vien (11-20)
              new { MaTaiKhoan = 11, TenDangNhap = "giaovien1", MatKhau = "123456", VaiTro = "GiangVien" },
              new { MaTaiKhoan = 12, TenDangNhap = "giaovien2", MatKhau = "123456", VaiTro = "GiangVien" },
              new { MaTaiKhoan = 13, TenDangNhap = "giaovien3", MatKhau = "123456", VaiTro = "GiangVien" },
              new { MaTaiKhoan = 14, TenDangNhap = "giaovien4", MatKhau = "123456", VaiTro = "GiangVien" },
              new { MaTaiKhoan = 15, TenDangNhap = "giaovien5", MatKhau = "123456", VaiTro = "GiangVien" },
              new { MaTaiKhoan = 16, TenDangNhap = "giaovien5", MatKhau = "123456", VaiTro = "GiangVien" },
              new { MaTaiKhoan = 17, TenDangNhap = "giaovien6", MatKhau = "123456", VaiTro = "GiangVien" },
              // Hoc Vien (>20)
              new { MaTaiKhoan = 21, TenDangNhap = "hocvien1", MatKhau = "123456", VaiTro = "HocVien" },
              new { MaTaiKhoan = 22, TenDangNhap = "hocvien2", MatKhau = "123456", VaiTro = "HocVien" },
              new { MaTaiKhoan = 23, TenDangNhap = "hocvien3", MatKhau = "123456", VaiTro = "HocVien" },
              new { MaTaiKhoan = 24, TenDangNhap = "hocvien4", MatKhau = "123456", VaiTro = "HocVien" },
              new { MaTaiKhoan = 25, TenDangNhap = "hocvien5", MatKhau = "123456", VaiTro = "HocVien" },
                new { MaTaiKhoan = 26, TenDangNhap = "hocvien6", MatKhau = "123456", VaiTro = "HocVien" },
                new { MaTaiKhoan = 27, TenDangNhap = "hocvien7", MatKhau = "123456", VaiTro = "HocVien" }
              );
            // Thoi Khoa Bieu
            modelBuilder.Entity<THOIKHOABIEU>().HasData(
                new { MaLichHoc = 11, CaHoc = "1-3 (7h - 9h)", NgayHoc = new DateTime(2025, 9, 1), MaLopHoc = 1 },
                new { MaLichHoc = 12, CaHoc = "3-5 (9h - 11h)", NgayHoc = new DateTime(2025, 9, 3), MaLopHoc = 1 },
                new { MaLichHoc = 13, CaHoc = "1-3 (7h - 9h)", NgayHoc = new DateTime(2025, 9, 5), MaLopHoc = 1 },
                new { MaLichHoc = 21, CaHoc = "1-3 (7h - 9h)", NgayHoc = new DateTime(2025, 9, 2), MaLopHoc = 2 },
                new { MaLichHoc = 22, CaHoc = "5-7 (13h - 15h)", NgayHoc = new DateTime(2025, 9, 4), MaLopHoc = 2 },
                new { MaLichHoc = 23, CaHoc = "1-3 (7h - 9h)", NgayHoc = new DateTime(2025, 9, 6), MaLopHoc = 2 },
                new { MaLichHoc = 31, CaHoc = "3-5 (9h - 11h)", NgayHoc = new DateTime(2025, 9, 3), MaLopHoc = 3 },
                new { MaLichHoc = 32, CaHoc = "7-9 (15h - 17h)", NgayHoc = new DateTime(2025, 9, 7), MaLopHoc = 3 },
                new { MaLichHoc = 33, CaHoc = "3-5 (9h - 11h)", NgayHoc = new DateTime(2025, 9, 11), MaLopHoc = 3 },
                new { MaLichHoc = 41, CaHoc = "3-5 (9h - 11h)", NgayHoc = new DateTime(2025, 9, 4), MaLopHoc = 4 },
                new { MaLichHoc = 42, CaHoc = "5-7 (13h - 15h)", NgayHoc = new DateTime(2025, 9, 5), MaLopHoc = 4 },
                new { MaLichHoc = 43, CaHoc = "5-7 (13h - 15h)", NgayHoc = new DateTime(2025, 9, 6), MaLopHoc = 4 },
                new { MaLichHoc = 51, CaHoc = "1-3 (7h - 9h)", NgayHoc = new DateTime(2025, 9, 1), MaLopHoc = 5 },
                new { MaLichHoc = 52, CaHoc = "5-7 (13h - 15h)", NgayHoc = new DateTime(2025, 9, 5), MaLopHoc = 5 },
                new { MaLichHoc = 53, CaHoc = "1-3 (7h - 9h)", NgayHoc = new DateTime(2025, 9, 9), MaLopHoc = 5 },
                new { MaLichHoc = 61, CaHoc = "1-3 (7h - 9h)", NgayHoc = new DateTime(2025, 9, 2), MaLopHoc = 6 },
                new { MaLichHoc = 62, CaHoc = "5-7 (13h - 15h)", NgayHoc = new DateTime(2025, 9, 6), MaLopHoc = 6 },
                new { MaLichHoc = 63, CaHoc = "1-3 (7h - 9h)", NgayHoc = new DateTime(2025, 9, 10), MaLopHoc = 6 },
                new { MaLichHoc = 71, CaHoc = "1-3 (7h - 9h)", NgayHoc = new DateTime(2025, 9, 2), MaLopHoc = 7 },
                new { MaLichHoc = 72, CaHoc = "7-9 (15h - 17h)", NgayHoc = new DateTime(2025, 9, 3), MaLopHoc = 7 },
                new { MaLichHoc = 73, CaHoc = "1-3 (7h - 9h)", NgayHoc = new DateTime(2025, 9, 4), MaLopHoc = 7 }
                );
            // Thong Bao
            modelBuilder.Entity<THONGBAO>().HasData(
                new { MaThongBao = 1, TieuDe = "Thông báo lịch học", NoiDung = "Lịch học sẽ bắt đầu từ ngày 1 tháng 9 năm 2025.", NgayThongBao = new DateTime(2025, 8, 20), MaTaiKhoan = 1 },
                new { MaThongBao = 2, TieuDe = "Thông báo học phí", NoiDung = "Học phí cho khóa học tiếng Anh cơ bản là 2.000.000 VNĐ.", NgayThongBao = new DateTime(2025, 8, 25), MaTaiKhoan = 2 },
                new { MaThongBao = 3, TieuDe = "Thông báo đăng ký lớp học", NoiDung = "Các bạn học viên vui lòng đăng ký lớp học trước ngày 30 tháng 8 năm 2025.", NgayThongBao = new DateTime(2025, 8, 28), MaTaiKhoan = 3 }
                );

        }


        public DbSet<QuanLyTTNgoaiNgu.Models.DANGKYMOI> DANGKYMOI { get; set; } = default!;
        public DbSet<QuanLyTTNgoaiNgu.Models.GIANGVIEN> GIANGVIEN { get; set; } = default!;
        public DbSet<QuanLyTTNgoaiNgu.Models.HOCPHI> HOCPHI { get; set; } = default!;
        public DbSet<QuanLyTTNgoaiNgu.Models.HOCVIEN> HOCVIEN { get; set; } = default!;
        public DbSet<QuanLyTTNgoaiNgu.Models.KETQUAHOCTAP> KETQUAHOCTAP { get; set; } = default!;
        public DbSet<QuanLyTTNgoaiNgu.Models.KHOAHOC> KHOAHOC { get; set; } = default!;
        public DbSet<QuanLyTTNgoaiNgu.Models.LOPHOC> LOPHOC { get; set; } = default!;
        public DbSet<QuanLyTTNgoaiNgu.Models.PHIEUDANGKY> PHIEUDANGKY { get; set; } = default!;
        public DbSet<QuanLyTTNgoaiNgu.Models.QUANTRIVIEN> QUANTRIVIEN { get; set; } = default!;
        public DbSet<QuanLyTTNgoaiNgu.Models.TAIKHOAN> TAIKHOAN { get; set; } = default!;
        public DbSet<QuanLyTTNgoaiNgu.Models.THOIKHOABIEU> THOIKHOABIEU { get; set; } = default!;
        public DbSet<QuanLyTTNgoaiNgu.Models.THONGBAO> THONGBAO { get; set; } = default!;
    }
}
