using System.ComponentModel.DataAnnotations;

namespace QuanLyTTNgoaiNgu.Models
{
    public class ChangePasswordViewModel
    {
        [Required, DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required, DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu mới ít nhất 6 ký tự.")]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        [Display(Name = "Xác nhận mật khẩu mới")]
        public string ConfirmPassword { get; set; }
    }
}

