using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTTNgoaiNgu.Models
{
    public class KETQUAHOCTAP
    {
        [Key]
        public int MaKetQua { get; set; }
        [Required]
        [Range(0, 10, ErrorMessage = "Điểm phải nằm trong khoảng từ 0 đến 10.")]
        public double Diem { get; set; }
        [ForeignKey("PHIEUDANGKY")]
        public int MaPhieu { get; set; }
        public virtual PHIEUDANGKY? PHIEUDANGKY { get; set; }

    }
}
