﻿using System;
using System.Collections.Generic;

namespace Project_Summer.DataContext;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; }

    public string Description { get; set; }

    public virtual ICollection<KhachHang> MaKhs { get; set; } = new List<KhachHang>();
}
