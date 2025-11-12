using System;
using System.Collections.Generic;

namespace QuanLyTTNgoaiNgu.Models
{
    public class ClassInfoViewModel
    {
        public int MaLopHoc { get; set; }
        public string TenLopHoc { get; set; }
        public int SLHocVienToiDa { get; set; }
        public int SLHocVienHienTai { get; set; }
        public DateTime NgayBatDau { get; set; }
        public bool CanRegister { get; set; }
        public bool HasRegistered { get; set; }
        public List<THOIKHOABIEU> Schedules { get; set; }
    }
}
