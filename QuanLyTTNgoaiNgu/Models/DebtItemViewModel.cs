namespace QuanLyTTNgoaiNgu.Models
{
    public class DebtItemViewModel
    {
        public int MaHocPhi { get; set; }
        public string TenKhoaHoc { get; set; } = "";
        public string TenLopHoc { get; set; } = "";
        public double MucHocPhi { get; set; }
        public bool TrangThai { get; set; }
        public bool ChoXacNhan { get; set; } = false;
        public DateTime? NgayNop { get; set; }
    }
}
