using System.ComponentModel.DataAnnotations;

namespace QuanLyTTNgoaiNgu.Models
{
    public class PhanCongGiangVienViewModel
    {
        [Required]
        public DateTime NgayHoc { get; set; }

        [Required]
        public string CaHoc { get; set; }

        [Required]
        public int MaLopHoc { get; set; }

        public List<GIANGVIEN>? DanhSachGiangVienPhuHop { get; set; }

        public int? MaGiangVienDuocChon { get; set; }
    }
}
