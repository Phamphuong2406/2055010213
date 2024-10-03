using System;
using System.Collections.Generic;

namespace Project_Summer.DataContext;

public partial class NhanVien
{
    public string MaNv { get; set; }

    public string HoTen { get; set; }

    public string Email { get; set; }

    public string MatKhau { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
