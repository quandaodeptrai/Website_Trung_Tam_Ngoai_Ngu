using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTTNgoaiNgu.Models
{
    public class THONGBAO
    {
        [Key]
        public int MaThongBao { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề.")]
        [StringLength(100, ErrorMessage = "Tiêu đề không được vượt quá 100 ký tự.")]
        public string TieuDe { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung.")]
        [StringLength(500, ErrorMessage = "Nội dung không được vượt quá 500 ký tự.")]
        public string NoiDung { get; set; }
        [Required]
        public DateTime? NgayThongBao { get; set; }
        [ForeignKey("TAIKHOAN")]
        [Required]
        public int MaTaiKhoan { get; set; }//Ma nhan thong bao
        public virtual TAIKHOAN TaiKhoan { get; set; }
    }
}
