using System.ComponentModel.DataAnnotations;

namespace Project_Summer.Models
{
    public class ProductDetailVM
    {
       
        public HangHoaDetailVM HangHoaDetailVM { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập tên!")]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập email!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập Nội dung!")]
        public string NoiDung { get; set; }


    }
   
}
