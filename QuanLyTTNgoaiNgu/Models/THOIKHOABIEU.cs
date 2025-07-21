using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTTNgoaiNgu.Models
{
    public class THOIKHOABIEU
    {
        [Key]
        public int MaLichHoc { get; set; }
        [Required]

        public string CaHoc { get; set; }
        [Required]

        public DateTime? NgayHoc { get; set; }
        [ForeignKey("LOPHOC")]
        [Required]
        public int MaLopHoc { get; set; }

        public virtual LOPHOC? LOPHOC { get; set; }


    }
}
