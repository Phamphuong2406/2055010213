using System.ComponentModel.DataAnnotations;

namespace Project_Summer.Models
{
	public class DangNhapVM
	{
		[Display(Name = "Tên đăng nhập")]
		[Required(ErrorMessage = "Tên hoặc mật khẩu k đúng  ")]
		public string UserName { get; set; }
		[Display(Name = "Mật Khẩu")]
		[Required(ErrorMessage = "Tên hoặc mật khẩu k đúng  ")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
