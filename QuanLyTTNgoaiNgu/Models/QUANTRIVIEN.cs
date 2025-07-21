using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTTNgoaiNgu.Models
{
    public class QUANTRIVIEN
    {
        [Key]
        public int MaQuanTriVien { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Tên quản trị viên không được vượt quá 50 ký tự.")]
        public string TenQuanTriVien { get; set; }
        [ForeignKey("TaiKhoan")]
        [Required]

        public int MaTaiKhoan { get; set; }
        public virtual TAIKHOAN? TaiKhoan { get; set; }
        public virtual ICollection<DANGKYMOI>? DangKyMoi { get; set; }


    }
}
