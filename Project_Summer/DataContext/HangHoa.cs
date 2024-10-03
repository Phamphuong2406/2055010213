using Project_Summer.Helper.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Summer.DataContext;

public partial class HangHoa
{
    public int MaHh { get; set; }

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

    public int MaNcc { get; set; }

    public int? SoLuong { get; set; }

    public int? DaBan { get; set; }
    [NotMapped]
    [FileExtension]
    public IFormFile HinhUpload { get; set; }


public virtual ICollection<BanBe> BanBes { get; set; } = new List<BanBe>();

    public virtual ICollection<ChiTietHd> ChiTietHds { get; set; } = new List<ChiTietHd>();

    public virtual Loai MaLoaiNavigation { get; set; }

    public virtual NhaCungCap MaNccNavigation { get; set; }

    public virtual ICollection<NhanXet> NhanXets { get; set; } = new List<NhanXet>();
}
