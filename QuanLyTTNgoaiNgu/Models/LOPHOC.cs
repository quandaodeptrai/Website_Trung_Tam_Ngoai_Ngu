using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTTNgoaiNgu.Models
{
    public class LOPHOC
    {
        [Key]
        public int MaLopHoc { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Tên lớp học không được vượt quá 100 ký tự.")]
        public string TenLopHoc { get; set; }
        [Required]
        [Range(0, 50, ErrorMessage = "Số lượng học viên tối đa phải từ 0 đến 50.")]
        public int SLHocVienToiDa { get; set; }


        [Required]
        public DateTime NgayBatDau { get; set; }
        [Required]
        public DateTime NgayKetThuc { get; set; }
        [ForeignKey("KHOAHOC")]
        [Required]
        public int MaKhoaHoc { get; set; }
        [ForeignKey("GIANGVIEN")]
        [Required]
        public int MaGiangVien { get; set; }
        public virtual KHOAHOC? KHOAHOC { get; set; }
        public virtual GIANGVIEN? GIANGVIEN { get; set; }
        public virtual ICollection<THOIKHOABIEU>? THOIKHOABIEUs { get; set; }
        public virtual ICollection<PHIEUDANGKY>? PHIEUDANGKies { get; set; }



    }
}