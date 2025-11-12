using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTTNgoaiNgu.Models
{
    public class DANGKYMOI
    {
        [Key]
        public int MaDangKy { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Họ tên không được vượt quá 50 ký tự.")]
        public string HoTen { get; set; }
        [Required]
        //them phan nay de Ngay 
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NgaySinh { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự.")]

        public string SoDienThoai { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Địa chỉ không được vượt quá 100 ký tự.")]
        public string DiaChi { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; }
        [ForeignKey("QUANTRIVIEN")]
        [Required]
        public int MaQuanTriVien { get; set; }
        public bool DaDuyet { get; set; } = false;

        public virtual ICollection <QUANTRIVIEN>? QUANTRIVIENs { get; set; }
        public virtual HOCVIEN? HOCVIEN { get; set; }
    }
}
