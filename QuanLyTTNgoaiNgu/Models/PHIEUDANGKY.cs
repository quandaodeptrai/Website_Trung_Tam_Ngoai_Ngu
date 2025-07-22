using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTTNgoaiNgu.Models
{
    public class PHIEUDANGKY
    {
        [Key]
        public int MaPhieu { get; set; }
        [Required]
        [Display(Name = "Ngày đăng ký")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public DateTime? NgayDangKy { get; set; }
        [ForeignKey("HOCVIEN")]
        [Required]
        public int MaHocVien { get; set; }
        [ForeignKey("LOPHOC")]
        [Required]
        public int MaLopHoc { get; set; }
        public virtual HOCVIEN? HOCVIEN { get; set; }
        public virtual LOPHOC? LOPHOC { get; set; }
        public virtual KETQUAHOCTAP? KETQUAHOCTAP { get; set; }
        public virtual HOCPHI? HOCPHI { get; set; }
    }
}
