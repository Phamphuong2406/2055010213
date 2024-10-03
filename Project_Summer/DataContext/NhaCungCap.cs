using System;
using System.Collections.Generic;

namespace Project_Summer.DataContext;

public partial class NhaCungCap
{
    public int MaNcc { get; set; }

    public string TenCongTy { get; set; }

    public string Logo { get; set; }

    public string NguoiLienLac { get; set; }

    public string Email { get; set; }

    public string DienThoai { get; set; }

    public string DiaChi { get; set; }

    public string MoTa { get; set; }

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
