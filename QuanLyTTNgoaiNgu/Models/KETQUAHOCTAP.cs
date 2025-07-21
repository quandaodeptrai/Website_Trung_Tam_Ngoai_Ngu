using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTTNgoaiNgu.Models
{
    public class KETQUAHOCTAP
    {
        [Key]
        public int MaKetQua { get; set; }
        [Required]
        public double Diem { get; set; }
        [ForeignKey("PHIEUDANGKY")]
        public int MaPhieu { get; set; }
        public virtual PHIEUDANGKY? PHIEUDANGKY { get; set; }

    }
}
