using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyTTNgoaiNgu.Migrations
{
    /// <inheritdoc />
    public partial class TTNgoaiNgu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DANGKYMOI",
                columns: table => new
                {
                    MaDangKy = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaQuanTriVien = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DANGKYMOI", x => x.MaDangKy);
                });

            migrationBuilder.CreateTable(
                name: "KHOAHOC",
                columns: table => new
                {
                    MaKhoaHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKhoaHoc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MucHocPhi = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHOAHOC", x => x.MaKhoaHoc);
                });

            migrationBuilder.CreateTable(
                name: "TAIKHOAN",
                columns: table => new
                {
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAIKHOAN", x => x.MaTaiKhoan);
                });

            migrationBuilder.CreateTable(
                name: "THONGBAO",
                columns: table => new
                {
                    MaThongBao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NgayThongBao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THONGBAO", x => x.MaThongBao);
                });

            migrationBuilder.CreateTable(
                name: "GIANGVIEN",
                columns: table => new
                {
                    MaGiangVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChuyenMon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GIANGVIEN", x => x.MaGiangVien);
                    table.ForeignKey(
                        name: "FK_GIANGVIEN_TAIKHOAN_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TAIKHOAN",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HOCVIEN",
                columns: table => new
                {
                    MaHocVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDangKy = table.Column<int>(type: "int", nullable: false),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOCVIEN", x => x.MaHocVien);
                    table.ForeignKey(
                        name: "FK_HOCVIEN_DANGKYMOI_MaDangKy",
                        column: x => x.MaDangKy,
                        principalTable: "DANGKYMOI",
                        principalColumn: "MaDangKy",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HOCVIEN_TAIKHOAN_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TAIKHOAN",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QUANTRIVIEN",
                columns: table => new
                {
                    MaQuanTriVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenQuanTriVien = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QUANTRIVIEN", x => x.MaQuanTriVien);
                    table.ForeignKey(
                        name: "FK_QUANTRIVIEN_TAIKHOAN_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TAIKHOAN",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TAIKHOANTHONGBAO",
                columns: table => new
                {
                    TAIKHOANsMaTaiKhoan = table.Column<int>(type: "int", nullable: false),
                    THONGBAOsMaThongBao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAIKHOANTHONGBAO", x => new { x.TAIKHOANsMaTaiKhoan, x.THONGBAOsMaThongBao });
                    table.ForeignKey(
                        name: "FK_TAIKHOANTHONGBAO_TAIKHOAN_TAIKHOANsMaTaiKhoan",
                        column: x => x.TAIKHOANsMaTaiKhoan,
                        principalTable: "TAIKHOAN",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TAIKHOANTHONGBAO_THONGBAO_THONGBAOsMaThongBao",
                        column: x => x.THONGBAOsMaThongBao,
                        principalTable: "THONGBAO",
                        principalColumn: "MaThongBao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOPHOC",
                columns: table => new
                {
                    MaLopHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLopHoc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SLHocVienToiDa = table.Column<int>(type: "int", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaKhoaHoc = table.Column<int>(type: "int", nullable: false),
                    MaGiangVien = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOPHOC", x => x.MaLopHoc);
                    table.ForeignKey(
                        name: "FK_LOPHOC_GIANGVIEN_MaGiangVien",
                        column: x => x.MaGiangVien,
                        principalTable: "GIANGVIEN",
                        principalColumn: "MaGiangVien",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LOPHOC_KHOAHOC_MaKhoaHoc",
                        column: x => x.MaKhoaHoc,
                        principalTable: "KHOAHOC",
                        principalColumn: "MaKhoaHoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DANGKYMOIQUANTRIVIEN",
                columns: table => new
                {
                    DangKyMoiMaDangKy = table.Column<int>(type: "int", nullable: false),
                    QUANTRIVIENsMaQuanTriVien = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DANGKYMOIQUANTRIVIEN", x => new { x.DangKyMoiMaDangKy, x.QUANTRIVIENsMaQuanTriVien });
                    table.ForeignKey(
                        name: "FK_DANGKYMOIQUANTRIVIEN_DANGKYMOI_DangKyMoiMaDangKy",
                        column: x => x.DangKyMoiMaDangKy,
                        principalTable: "DANGKYMOI",
                        principalColumn: "MaDangKy",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DANGKYMOIQUANTRIVIEN_QUANTRIVIEN_QUANTRIVIENsMaQuanTriVien",
                        column: x => x.QUANTRIVIENsMaQuanTriVien,
                        principalTable: "QUANTRIVIEN",
                        principalColumn: "MaQuanTriVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PHIEUDANGKY",
                columns: table => new
                {
                    MaPhieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaHocVien = table.Column<int>(type: "int", nullable: false),
                    MaLopHoc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHIEUDANGKY", x => x.MaPhieu);
                    table.ForeignKey(
                        name: "FK_PHIEUDANGKY_HOCVIEN_MaHocVien",
                        column: x => x.MaHocVien,
                        principalTable: "HOCVIEN",
                        principalColumn: "MaHocVien",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PHIEUDANGKY_LOPHOC_MaLopHoc",
                        column: x => x.MaLopHoc,
                        principalTable: "LOPHOC",
                        principalColumn: "MaLopHoc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "THOIKHOABIEU",
                columns: table => new
                {
                    MaLichHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaHoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayHoc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaLopHoc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THOIKHOABIEU", x => x.MaLichHoc);
                    table.ForeignKey(
                        name: "FK_THOIKHOABIEU_LOPHOC_MaLopHoc",
                        column: x => x.MaLopHoc,
                        principalTable: "LOPHOC",
                        principalColumn: "MaLopHoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HOCPHI",
                columns: table => new
                {
                    MaHocPhi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayNop = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    MaPhieu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOCPHI", x => x.MaHocPhi);
                    table.ForeignKey(
                        name: "FK_HOCPHI_PHIEUDANGKY_MaPhieu",
                        column: x => x.MaPhieu,
                        principalTable: "PHIEUDANGKY",
                        principalColumn: "MaPhieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KETQUAHOCTAP",
                columns: table => new
                {
                    MaKetQua = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diem = table.Column<double>(type: "float", nullable: false),
                    MaPhieu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KETQUAHOCTAP", x => x.MaKetQua);
                    table.ForeignKey(
                        name: "FK_KETQUAHOCTAP_PHIEUDANGKY_MaPhieu",
                        column: x => x.MaPhieu,
                        principalTable: "PHIEUDANGKY",
                        principalColumn: "MaPhieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DANGKYMOI",
                columns: new[] { "MaDangKy", "DiaChi", "Email", "HoTen", "MaQuanTriVien", "NgaySinh", "SoDienThoai" },
                values: new object[,]
                {
                    { 1, "Hanoi", "mai@gmail.com", "Phạm Thị Mai", 1, new DateTime(2003, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456" },
                    { 2, "ThaiNguyen", "nam@gmail.com", "Lê Văn Nam", 1, new DateTime(2004, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "123654" },
                    { 3, "NamDinh", "thu@gmail.com", "Vũ Thị Thu", 2, new DateTime(2005, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "321456" },
                    { 4, "HaiDuong", "long@gmail.com", "Ngô Đức Long", 2, new DateTime(2003, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "456123" },
                    { 5, "PhuTho", "nhung@gmail.com", "Trần Hồng Nhung", 3, new DateTime(2004, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "456321" },
                    { 6, "HaiPhong", "ly@gmail.com", "Nguyễn Thị Lý", 1, new DateTime(2005, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "145236" },
                    { 7, "NinhBinh", "duongductin02@gmail.com", "Dương Đức Tin", 3, new DateTime(2003, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "652341" }
                });

            migrationBuilder.InsertData(
                table: "KHOAHOC",
                columns: new[] { "MaKhoaHoc", "MoTa", "MucHocPhi", "TenKhoaHoc" },
                values: new object[,]
                {
                    { 1, "Khóa học tiếng Anh cơ bản dành cho người mới bắt đầu.", 2000000.0, "Tiếng Anh Cơ Bản" },
                    { 2, "Khóa học tiếng Anh nâng cao cho người đã có kiến thức cơ bản.", 3000000.0, "Tiếng Anh Nâng Cao" },
                    { 3, "Khóa học tiếng Trung cơ bản dành cho người mới bắt đầu.", 2500000.0, "Tiếng Trung Cơ Bản" },
                    { 4, "Khóa học tiếng Hàn cơ bản dành cho người mới bắt đầu.", 2800000.0, "Tiếng Hàn Cơ Bản" },
                    { 5, "Khóa học tiếng Nhật cơ bản dành cho người mới bắt đầu.", 2700000.0, "Tiếng Nhật Cơ Bản" },
                    { 6, "Khóa học tiếng Đức cơ bản dành cho người mới bắt đầu.", 2600000.0, "Tiếng Đức Cơ Bản" },
                    { 7, "Khóa học tiếng Trung nâng cao cho người đã có kiến thức cơ bản.", 3500000.0, "Tiếng Trung Nâng Cao" }
                });

            migrationBuilder.InsertData(
                table: "TAIKHOAN",
                columns: new[] { "MaTaiKhoan", "MatKhau", "TenDangNhap", "VaiTro" },
                values: new object[,]
                {
                    { 1, "admin123", "admin1", "Admin" },
                    { 2, "admin123", "admin2", "Admin" },
                    { 3, "admin123", "admin3", "Admin" },
                    { 4, "admin123", "admin4", "Admin" },
                    { 5, "admin123", "admin5", "Admin" },
                    { 11, "123456", "giaovien1", "GiangVien" },
                    { 12, "123456", "giaovien2", "GiangVien" },
                    { 13, "123456", "giaovien3", "GiangVien" },
                    { 14, "123456", "giaovien4", "GiangVien" },
                    { 15, "123456", "giaovien5", "GiangVien" },
                    { 16, "123456", "giaovien5", "GiangVien" },
                    { 17, "123456", "giaovien6", "GiangVien" },
                    { 21, "123456", "hocvien1", "HocVien" },
                    { 22, "123456", "hocvien2", "HocVien" },
                    { 23, "123456", "hocvien3", "HocVien" },
                    { 24, "123456", "hocvien4", "HocVien" },
                    { 25, "123456", "hocvien5", "HocVien" },
                    { 26, "123456", "hocvien6", "HocVien" },
                    { 27, "123456", "hocvien7", "HocVien" }
                });

            migrationBuilder.InsertData(
                table: "THONGBAO",
                columns: new[] { "MaThongBao", "MaTaiKhoan", "NgayThongBao", "NoiDung", "TieuDe" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lịch học sẽ bắt đầu từ ngày 1 tháng 9 năm 2025.", "Thông báo lịch học" },
                    { 2, 2, new DateTime(2025, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Học phí cho khóa học tiếng Anh cơ bản là 2.000.000 VNĐ.", "Thông báo học phí" },
                    { 3, 3, new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Các bạn học viên vui lòng đăng ký lớp học trước ngày 30 tháng 8 năm 2025.", "Thông báo đăng ký lớp học" }
                });

            migrationBuilder.InsertData(
                table: "GIANGVIEN",
                columns: new[] { "MaGiangVien", "ChuyenMon", "Email", "HoTen", "MaTaiKhoan", "SoDienThoai" },
                values: new object[,]
                {
                    { 1, "Tiếng Anh", "trung@gmail.com", "Cao Đình Trung", 11, "987654" },
                    { 2, "Tiếng Anh", "diep@gmail.com", "Dương Văn Điệp", 12, "987456" },
                    { 3, "Tiếng Trung", "bich@gmail.com", "Trần Thị Bích", 13, "456789" },
                    { 4, "Tiếng Hàn", "van@gmail.com", "Ngô Thanh Vân", 14, "456987" },
                    { 5, "Tiếng Nhật", "hiep@gmail.com", "Vũ Quang Hiệp", 15, "965874" },
                    { 6, "Tiếng Đức", "tam@gmail.com", "Lê Minh Tâm", 16, "478569" },
                    { 7, "Tiếng Trung", "ngoc@gmail.com", "Trần Văn Ngọc", 17, "785496" }
                });

            migrationBuilder.InsertData(
                table: "HOCVIEN",
                columns: new[] { "MaHocVien", "MaDangKy", "MaTaiKhoan" },
                values: new object[,]
                {
                    { 1, 1, 21 },
                    { 2, 2, 22 },
                    { 3, 3, 23 },
                    { 4, 4, 24 },
                    { 5, 5, 25 },
                    { 6, 6, 26 },
                    { 7, 7, 27 }
                });

            migrationBuilder.InsertData(
                table: "QUANTRIVIEN",
                columns: new[] { "MaQuanTriVien", "MaTaiKhoan", "TenQuanTriVien" },
                values: new object[,]
                {
                    { 1, 1, "Đào Văn Quân" },
                    { 2, 2, "Vũ Thanh Ngọc" },
                    { 3, 3, "Dương Đức Tín" },
                    { 4, 4, "Nguyễn Viết Trường" },
                    { 5, 5, "Nguyễn Anh Tuấn" }
                });

            migrationBuilder.InsertData(
                table: "LOPHOC",
                columns: new[] { "MaLopHoc", "MaGiangVien", "MaKhoaHoc", "NgayBatDau", "NgayKetThuc", "SLHocVienToiDa", "TenLopHoc" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Lớp Tiếng Anh Cơ Bản 1" },
                    { 2, 2, 2, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Lớp Tiếng Anh Nâng Cao 1" },
                    { 3, 3, 3, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Lớp Tiếng Trung Cơ Bản 1" },
                    { 4, 4, 4, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Lớp Tiếng Hàn Cơ Bản 1" },
                    { 5, 5, 5, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Lớp Tiếng Nhật Cơ Bản 1" },
                    { 6, 6, 6, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Lớp Tiếng Đức Cơ Bản 1" },
                    { 7, 7, 7, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Lớp Tiếng Trung Nâng Cao 1" }
                });

            migrationBuilder.InsertData(
                table: "PHIEUDANGKY",
                columns: new[] { "MaPhieu", "MaHocVien", "MaLopHoc", "NgayDangKy" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, 2, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, 3, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 4, 4, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 5, 5, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 6, 6, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 7, 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "THOIKHOABIEU",
                columns: new[] { "MaLichHoc", "CaHoc", "MaLopHoc", "NgayHoc" },
                values: new object[,]
                {
                    { 11, "1-3 (7h - 9h)", 1, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, "3-5 (9h - 11h)", 1, new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, "1-3 (7h - 9h)", 1, new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, "1-3 (7h - 9h)", 2, new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, "5-7 (13h - 15h)", 2, new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, "1-3 (7h - 9h)", 2, new DateTime(2025, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 31, "3-5 (9h - 11h)", 3, new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 32, "7-9 (15h - 17h)", 3, new DateTime(2025, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 33, "3-5 (9h - 11h)", 3, new DateTime(2025, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 41, "3-5 (9h - 11h)", 4, new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 42, "5-7 (13h - 15h)", 4, new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 43, "5-7 (13h - 15h)", 4, new DateTime(2025, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 51, "1-3 (7h - 9h)", 5, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 52, "5-7 (13h - 15h)", 5, new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 53, "1-3 (7h - 9h)", 5, new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 61, "1-3 (7h - 9h)", 6, new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 62, "5-7 (13h - 15h)", 6, new DateTime(2025, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 63, "1-3 (7h - 9h)", 6, new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 71, "1-3 (7h - 9h)", 7, new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 72, "7-9 (15h - 17h)", 7, new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 73, "1-3 (7h - 9h)", 7, new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "HOCPHI",
                columns: new[] { "MaHocPhi", "MaPhieu", "NgayNop", "TrangThai" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), true },
                    { 2, 2, new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true },
                    { 3, 3, null, false },
                    { 4, 4, new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true },
                    { 5, 5, null, false },
                    { 6, 6, new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), true },
                    { 7, 7, new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true }
                });

            migrationBuilder.InsertData(
                table: "KETQUAHOCTAP",
                columns: new[] { "MaKetQua", "Diem", "MaPhieu" },
                values: new object[,]
                {
                    { 1, 8.5, 1 },
                    { 2, 4.0, 2 },
                    { 3, 7.5, 3 },
                    { 4, 5.0, 4 },
                    { 5, 6.5, 5 },
                    { 6, 8.0, 6 },
                    { 7, 9.0, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DANGKYMOIQUANTRIVIEN_QUANTRIVIENsMaQuanTriVien",
                table: "DANGKYMOIQUANTRIVIEN",
                column: "QUANTRIVIENsMaQuanTriVien");

            migrationBuilder.CreateIndex(
                name: "IX_GIANGVIEN_MaTaiKhoan",
                table: "GIANGVIEN",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_HOCPHI_MaPhieu",
                table: "HOCPHI",
                column: "MaPhieu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HOCVIEN_MaDangKy",
                table: "HOCVIEN",
                column: "MaDangKy",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HOCVIEN_MaTaiKhoan",
                table: "HOCVIEN",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_KETQUAHOCTAP_MaPhieu",
                table: "KETQUAHOCTAP",
                column: "MaPhieu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LOPHOC_MaGiangVien",
                table: "LOPHOC",
                column: "MaGiangVien");

            migrationBuilder.CreateIndex(
                name: "IX_LOPHOC_MaKhoaHoc",
                table: "LOPHOC",
                column: "MaKhoaHoc");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUDANGKY_MaHocVien",
                table: "PHIEUDANGKY",
                column: "MaHocVien");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUDANGKY_MaLopHoc",
                table: "PHIEUDANGKY",
                column: "MaLopHoc");

            migrationBuilder.CreateIndex(
                name: "IX_QUANTRIVIEN_MaTaiKhoan",
                table: "QUANTRIVIEN",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_TAIKHOANTHONGBAO_THONGBAOsMaThongBao",
                table: "TAIKHOANTHONGBAO",
                column: "THONGBAOsMaThongBao");

            migrationBuilder.CreateIndex(
                name: "IX_THOIKHOABIEU_MaLopHoc",
                table: "THOIKHOABIEU",
                column: "MaLopHoc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DANGKYMOIQUANTRIVIEN");

            migrationBuilder.DropTable(
                name: "HOCPHI");

            migrationBuilder.DropTable(
                name: "KETQUAHOCTAP");

            migrationBuilder.DropTable(
                name: "TAIKHOANTHONGBAO");

            migrationBuilder.DropTable(
                name: "THOIKHOABIEU");

            migrationBuilder.DropTable(
                name: "QUANTRIVIEN");

            migrationBuilder.DropTable(
                name: "PHIEUDANGKY");

            migrationBuilder.DropTable(
                name: "THONGBAO");

            migrationBuilder.DropTable(
                name: "HOCVIEN");

            migrationBuilder.DropTable(
                name: "LOPHOC");

            migrationBuilder.DropTable(
                name: "DANGKYMOI");

            migrationBuilder.DropTable(
                name: "GIANGVIEN");

            migrationBuilder.DropTable(
                name: "KHOAHOC");

            migrationBuilder.DropTable(
                name: "TAIKHOAN");
        }
    }
}
