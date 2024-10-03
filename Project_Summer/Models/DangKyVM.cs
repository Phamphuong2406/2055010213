using System.ComponentModel.DataAnnotations;

namespace Project_Summer.Models
{
    public class DangKyVM
    {
    
        public string MaKh { get; set; } 
        [Display(Name="Mật khẩu")]
        [Required(ErrorMessage="Tên hoặc mật khẩu k đúng  ")]
        public string? MatKhau { get; set; }
        [Display(Name = "Tên đăng nhập ")]
        [Required(ErrorMessage = "Tên hoặc mật khẩu k đúng  ")]
        public string HoTen { get; set; } 
        [Display(Name = "Giới tính ")]
        public bool GioiTinh { get; set; }
        [Display(Name = "Ngày sinh ")]
        public DateTime NgaySinh { get; set; }

        public string? DiaChi { get; set; }

        public string? DienThoai { get; set; }

        public string Email { get; set; } 

        public string? Hinh { get; set; }

        public bool HieuLuc { get; set; }

        public int VaiTro { get; set; }

        public string? RandomKey { get; set; }
    }
}
