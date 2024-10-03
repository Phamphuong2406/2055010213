using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project_Summer.DataContext;

public partial class SummerrContext : DbContext
{
    public SummerrContext()
    {
    }

    public SummerrContext(DbContextOptions<SummerrContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BanBe> BanBes { get; set; }

    public virtual DbSet<ChiTietHd> ChiTietHds { get; set; }

    public virtual DbSet<HangHoa> HangHoas { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<Loai> Loais { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<NhanXet> NhanXets { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SoLuongSp> SoLuongSps { get; set; }

    public virtual DbSet<TrangThai> TrangThais { get; set; }

    public virtual DbSet<VChiTietHoaDon> VChiTietHoaDons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DbSummerr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BanBe>(entity =>
        {
            entity.HasKey(e => e.MaBb).HasName("PK_Promotions");

            entity.ToTable("BanBe");

            entity.Property(e => e.MaBb).HasColumnName("MaBB");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.MaKh)
                .HasMaxLength(100)
                .HasColumnName("MaKH");
            entity.Property(e => e.NgayGui)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.BanBes)
                .HasForeignKey(d => d.MaHh)
                .HasConstraintName("FK_QuangBa_HangHoa");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.BanBes)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK_BanBe_KhachHang");
        });

        modelBuilder.Entity<ChiTietHd>(entity =>
        {
            entity.HasKey(e => e.MaCt).HasName("PK_OrderDetails");

            entity.ToTable("ChiTietHD");

            entity.Property(e => e.MaCt).HasColumnName("MaCT");
            entity.Property(e => e.HinhHh)
                .HasMaxLength(100)
                .HasColumnName("HinhHH");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.SoLuong).HasDefaultValue(1);

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.ChiTietHds)
                .HasForeignKey(d => d.MaHd)
                .HasConstraintName("FK_OrderDetails_Orders");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.ChiTietHds)
                .HasForeignKey(d => d.MaHh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietHD_HangHoa");
        });

        modelBuilder.Entity<HangHoa>(entity =>
        {
            entity.HasKey(e => e.MaHh).HasName("PK_Products");

            entity.ToTable("HangHoa");

            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.DonGia).HasDefaultValue(0.0);
            entity.Property(e => e.Hinh).HasMaxLength(50);
            entity.Property(e => e.MaNcc).HasColumnName("MaNCC");
            entity.Property(e => e.MoTaDonVi).HasMaxLength(50);
            entity.Property(e => e.NgaySx)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NgaySX");
            entity.Property(e => e.TenAlias).HasMaxLength(50);
            entity.Property(e => e.TenHh)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("TenHH");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.MaLoai)
                .HasConstraintName("FK_Products_Categories");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.MaNcc)
                .HasConstraintName("FK_Products_Suppliers");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK_Orders");

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.CachThanhToan)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Cash");
            entity.Property(e => e.CachVanChuyen)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Airline");
            entity.Property(e => e.DiaChi)
                .IsRequired()
                .HasMaxLength(60);
            entity.Property(e => e.DienThoai).HasMaxLength(24);
            entity.Property(e => e.GhiChu).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaKh)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.NgayCan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayGiao)
                .HasDefaultValueSql("(((1)/(1))/(1900))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_HoaDon_NhanVien");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaTrangThai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoaDon_TrangThai");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK_Customers");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKh)
                .HasMaxLength(100)
                .HasColumnName("MaKH");
            entity.Property(e => e.DiaChi).HasMaxLength(60);
            entity.Property(e => e.DienThoai).HasMaxLength(24);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Hinh)
                .HasMaxLength(50)
                .HasDefaultValue("Photo.gif");
            entity.Property(e => e.HoTen)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.MatKhau).HasMaxLength(50);
            entity.Property(e => e.NgaySinh)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RandomKey)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(d => d.Roles).WithMany(p => p.MaKhs)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK__UserRole__RoleId__65370702"),
                    l => l.HasOne<KhachHang>().WithMany()
                        .HasForeignKey("MaKh")
                        .HasConstraintName("FK__UserRole__MaKH__6442E2C9"),
                    j =>
                    {
                        j.HasKey("MaKh", "RoleId").HasName("PK__UserRole__9F8A63FFC63E4089");
                        j.ToTable("UserRole");
                        j.IndexerProperty<string>("MaKh")
                            .HasMaxLength(100)
                            .HasColumnName("MaKH");
                    });
        });

        modelBuilder.Entity<Loai>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK_Categories");

            entity.ToTable("Loai");

            entity.Property(e => e.Hinh).HasMaxLength(50);
            entity.Property(e => e.TenLoai)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TenLoaiAlias).HasMaxLength(50);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNcc).HasName("PK_Suppliers");

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.MaNcc).HasColumnName("MaNCC");
            entity.Property(e => e.DiaChi).HasMaxLength(50);
            entity.Property(e => e.DienThoai).HasMaxLength(50);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Logo)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.NguoiLienLac).HasMaxLength(50);
            entity.Property(e => e.TenCongTy).HasMaxLength(50);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv);

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.HoTen)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.MatKhau).HasMaxLength(50);
        });

        modelBuilder.Entity<NhanXet>(entity =>
        {
            entity.HasKey(e => e.MaNx).HasName("PK_Favorites");

            entity.ToTable("NhanXet");

            entity.Property(e => e.MaNx).HasColumnName("MaNX");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.NgayDang).HasColumnType("datetime");
            entity.Property(e => e.NoiDung).HasMaxLength(255);

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.NhanXets)
                .HasForeignKey(d => d.MaHh)
                .HasConstraintName("FK_NhanXet_HangHoa");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1AC3D2B8DF");

            entity.ToTable("Role");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<SoLuongSp>(entity =>
        {
            entity.HasKey(e => e.MaSl);

            entity.ToTable("SoLuongSp");

            entity.Property(e => e.MaSl).HasColumnName("MaSL");
            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.NgayCapNhat).HasColumnType("datetime");
        });

        modelBuilder.Entity<TrangThai>(entity =>
        {
            entity.HasKey(e => e.MaTrangThai);

            entity.ToTable("TrangThai");

            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.TenTrangThai)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<VChiTietHoaDon>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vChiTietHoaDon");

            entity.Property(e => e.MaCt).HasColumnName("MaCT");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.TenHh)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("TenHH");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
