using System.ComponentModel.DataAnnotations;

namespace QuanLyTTNgoaiNgu.Models
{
    public class ScoreEntryViewModel
    {
        public int MaPhieu { get; set; }               // Khóa PHIEUDANGKY

        public string TenHocVien { get; set; } = "";   // Hiển thị tên

        [Required]
        [Range(0, 10, ErrorMessage = "Điểm phải từ 0 đến 10.")]
        [Display(Name = "Điểm")]
        public double Diem { get; set; }               // Nhập điểm

    }
}
