using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTTNgoaiNgu.Models
{
    public class HOCVIEN
    {
        [Key]
        public int MaHocVien { get; set; }
        [ForeignKey("DANGKYMOI")]
        [Required]
        public int MaDangKy { get; set; }
        [ForeignKey("TAIKHOAN")]
        [Required]
        public int MaTaiKhoan { get; set; }
        public virtual ICollection<PHIEUDANGKY>? PHIEUDANGKies { get; set; }
        public virtual DANGKYMOI? DANGKYMOI { get; set; }
        public virtual TAIKHOAN? TAIKHOAN { get; set; }
    }
}
