namespace QuanLyTTNgoaiNgu.Models
{
    public class ThanhToanNhieuViewModel
    {
        public List<int> SelectedIds { get; set; } = new();
        public Dictionary<int, double> SoTienNhap { get; set; } = new();
    }
}
