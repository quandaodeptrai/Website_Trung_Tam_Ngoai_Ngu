using System.ComponentModel.DataAnnotations;

namespace QuanLyTTNgoaiNgu.Models
{
    public class NotificationViewModel
    {
        [Required, StringLength(100)]
        public string TieuDe { get; set; } = "";

        [Required, StringLength(500)]
        public string NoiDung { get; set; } = "";

        [Display(Name = "Chọn vai trò (tùy chọn)")]
        public string SelectedRole { get; set; } = "";

        public List<string> Roles { get; set; } = new() { "Admin", "GiangVien", "HocVien" };

        public List<AccountSelect> Accounts { get; set; } = new();

        [Required(ErrorMessage = "Phải chọn ít nhất một tài khoản.")]
        [Display(Name = "Tài khoản nhận")]
        public List<int> SelectedAccounts { get; set; } = new();
    }

    public class AccountSelect
    {
        public int MaTaiKhoan { get; set; }
        public string TenDangNhap { get; set; } = "";
        public string VaiTro { get; set; } = "";
    }

}
