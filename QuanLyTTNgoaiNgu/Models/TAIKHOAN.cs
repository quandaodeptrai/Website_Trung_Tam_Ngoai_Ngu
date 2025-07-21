using System.ComponentModel.DataAnnotations;

namespace QuanLyTTNgoaiNgu.Models
{
    public class TAIKHOAN
    {
        [Key]
        public int MaTaiKhoan { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự.")]
        public string TenDangNhap { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Mật khẩu không được vượt quá 50 ký tự.")]
        public string MatKhau { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Email không được vượt quá 50 ký tự.")]
        public string VaiTro { get; set; } // "GiangVien", "QuanTriVien", "HocVien"
        public virtual ICollection<QUANTRIVIEN>? QUANTRIVIENs { get; set; }
        public virtual ICollection<GIANGVIEN>? GIANGVIENs { get; set; }
        public virtual ICollection<HOCVIEN>? HOCVIENs { get; set; } 
        public virtual ICollection<THONGBAO>? THONGBAOs { get; set; }
    }
}
