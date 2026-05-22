using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyTTNgoaiNgu.Migrations
{
    /// <inheritdoc />
    public partial class UpdateThongBaoNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TAIKHOANTHONGBAO");

            migrationBuilder.AddColumn<int>(
                name: "TAIKHOANMaTaiKhoan",
                table: "THONGBAO",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "THONGBAO",
                keyColumn: "MaThongBao",
                keyValue: 1,
                column: "TAIKHOANMaTaiKhoan",
                value: null);

            migrationBuilder.UpdateData(
                table: "THONGBAO",
                keyColumn: "MaThongBao",
                keyValue: 2,
                column: "TAIKHOANMaTaiKhoan",
                value: null);

            migrationBuilder.UpdateData(
                table: "THONGBAO",
                keyColumn: "MaThongBao",
                keyValue: 3,
                column: "TAIKHOANMaTaiKhoan",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_THONGBAO_MaTaiKhoan",
                table: "THONGBAO",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_THONGBAO_TAIKHOANMaTaiKhoan",
                table: "THONGBAO",
                column: "TAIKHOANMaTaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_THONGBAO_TAIKHOAN_MaTaiKhoan",
                table: "THONGBAO",
                column: "MaTaiKhoan",
                principalTable: "TAIKHOAN",
                principalColumn: "MaTaiKhoan",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_THONGBAO_TAIKHOAN_TAIKHOANMaTaiKhoan",
                table: "THONGBAO",
                column: "TAIKHOANMaTaiKhoan",
                principalTable: "TAIKHOAN",
                principalColumn: "MaTaiKhoan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_THONGBAO_TAIKHOAN_MaTaiKhoan",
                table: "THONGBAO");

            migrationBuilder.DropForeignKey(
                name: "FK_THONGBAO_TAIKHOAN_TAIKHOANMaTaiKhoan",
                table: "THONGBAO");

            migrationBuilder.DropIndex(
                name: "IX_THONGBAO_MaTaiKhoan",
                table: "THONGBAO");

            migrationBuilder.DropIndex(
                name: "IX_THONGBAO_TAIKHOANMaTaiKhoan",
                table: "THONGBAO");

            migrationBuilder.DropColumn(
                name: "TAIKHOANMaTaiKhoan",
                table: "THONGBAO");

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

            migrationBuilder.CreateIndex(
                name: "IX_TAIKHOANTHONGBAO_THONGBAOsMaThongBao",
                table: "TAIKHOANTHONGBAO",
                column: "THONGBAOsMaThongBao");
        }
    }
}
