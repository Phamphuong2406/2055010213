
using Project_Summer.Helper.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Summer.Areas.Admin.Models
{
    public class ProductVM
    {
        [Key]
        public string TenHh { get; set; }

        public string TenAlias { get; set; }

        public int MaLoai { get; set; }

        public string MoTaDonVi { get; set; }

        public double? DonGia { get; set; }

        public string Hinh { get; set; }

        public DateTime NgaySx { get; set; }

        public double GiamGia { get; set; }

        public int SoLanXem { get; set; }

        public string MoTa { get; set; }

        public string MaNcc { get; set; } 
        [NotMapped]
        [FileExtension]
        public IFormFile HinhUpload { get; set; }
    }
}
