using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTTNgoaiNgu.Models
{
    public class GIANGVIEN
    {
        [Key]
        public int MaGiangVien { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Tên giảng viên không được vượt quá 50 ký tự.")]
        public string HoTen { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Chuyên môn không được vượt quá 50 ký tự.")]
        public string ChuyenMon { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        public string Email { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự.")]
        public string SoDienThoai { get; set; }
        [ForeignKey("TAIKHOAN")]
        [Required]
        public int MaTaiKhoan { get; set; }

        public virtual TAIKHOAN? TAIKHOAN { get; set; }
        public virtual ICollection<LOPHOC>? LOPHOCs { get; set; } 

    }
}
