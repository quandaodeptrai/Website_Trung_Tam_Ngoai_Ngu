using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTTNgoaiNgu.Models
{
    public class HOCPHI
    {
        [Key]
        public int MaHocPhi { get; set; }
        
        public DateTime? NgayNop { get; set; }
        [Required]
        public bool TrangThai { get; set; }
        [ForeignKey("PHIEUDANGKY")]
        [Required]

        public int MaPhieu { get; set; }
        public virtual PHIEUDANGKY? PHIEUDANGKY { get; set; }

    }
}
