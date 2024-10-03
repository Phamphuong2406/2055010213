using System;
using System.Collections.Generic;

namespace Project_Summer.DataContext;

public partial class NhanXet
{
    public int MaNx { get; set; }

    public int? MaHh { get; set; }

    public string HoTen { get; set; }

    public DateTime? NgayDang { get; set; }

    public string Email { get; set; }

    public string NoiDung { get; set; }

    public virtual HangHoa MaHhNavigation { get; set; }
}
