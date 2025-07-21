using System.ComponentModel.DataAnnotations;

namespace QuanLyTTNgoaiNgu.Models
{
    public class KHOAHOC
    {
        [Key]
        public int MaKhoaHoc { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Tên khóa học không được vượt quá 50 ký tự.")]
        public string TenKhoaHoc { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Mô tả khóa học không được vượt quá 100 ký tự.")]
        public string MoTa { get; set; }
        [Required]
        public double MucHocPhi { get; set; }
        public virtual ICollection<LOPHOC>? LOPHOCs { get; set; }

    }
}
