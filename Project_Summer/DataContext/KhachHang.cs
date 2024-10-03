using System;
using System.Collections.Generic;

namespace Project_Summer.DataContext;

public partial class KhachHang
{
    public string MaKh { get; set; }

    public string MatKhau { get; set; }

    public string HoTen { get; set; }

    public bool GioiTinh { get; set; }

    public DateTime NgaySinh { get; set; }

    public string DiaChi { get; set; }

    public string DienThoai { get; set; }

    public string Email { get; set; }

    public string Hinh { get; set; }

    public bool HieuLuc { get; set; }

    public int VaiTro { get; set; }

    public string RandomKey { get; set; }

    public virtual ICollection<BanBe> BanBes { get; set; } = new List<BanBe>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
