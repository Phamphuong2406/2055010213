﻿using System;
using System.Collections.Generic;

namespace Project_Summer.DataContext;

public partial class ChiTietHd
{
    public int MaCt { get; set; }

    public int MaHd { get; set; }

    public int MaHh { get; set; }

    public double DonGia { get; set; }

    public int SoLuong { get; set; }

    public double GiamGia { get; set; }

    public string HinhHh { get; set; }

    public virtual HoaDon MaHdNavigation { get; set; }

    public virtual HangHoa MaHhNavigation { get; set; }
}
