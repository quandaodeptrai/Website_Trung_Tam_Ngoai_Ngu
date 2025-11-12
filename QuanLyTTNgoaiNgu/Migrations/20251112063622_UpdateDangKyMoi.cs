using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyTTNgoaiNgu.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDangKyMoi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DaDuyet",
                table: "DANGKYMOI",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DANGKYMOI",
                keyColumn: "MaDangKy",
                keyValue: 1,
                column: "DaDuyet",
                value: false);

            migrationBuilder.UpdateData(
                table: "DANGKYMOI",
                keyColumn: "MaDangKy",
                keyValue: 2,
                column: "DaDuyet",
                value: false);

            migrationBuilder.UpdateData(
                table: "DANGKYMOI",
                keyColumn: "MaDangKy",
                keyValue: 3,
                column: "DaDuyet",
                value: false);

            migrationBuilder.UpdateData(
                table: "DANGKYMOI",
                keyColumn: "MaDangKy",
                keyValue: 4,
                column: "DaDuyet",
                value: false);

            migrationBuilder.UpdateData(
                table: "DANGKYMOI",
                keyColumn: "MaDangKy",
                keyValue: 5,
                column: "DaDuyet",
                value: false);

            migrationBuilder.UpdateData(
                table: "DANGKYMOI",
                keyColumn: "MaDangKy",
                keyValue: 6,
                column: "DaDuyet",
                value: false);

            migrationBuilder.UpdateData(
                table: "DANGKYMOI",
                keyColumn: "MaDangKy",
                keyValue: 7,
                column: "DaDuyet",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaDuyet",
                table: "DANGKYMOI");
        }
    }
}
