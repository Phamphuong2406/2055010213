﻿using System;
using System.Collections.Generic;

namespace Project_Summer.DataContext;

public partial class Loai
{
    public int MaLoai { get; set; }

    public string TenLoai { get; set; }

    public string TenLoaiAlias { get; set; }

    public string MoTa { get; set; }

    public string Hinh { get; set; }

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
