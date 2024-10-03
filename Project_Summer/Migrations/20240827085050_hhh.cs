using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Summer.Migrations
{
    /// <inheritdoc />
    public partial class hhh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    MaKH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GioiTinh = table.Column<bool>(type: "bit", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    DiaChi = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    DienThoai = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Hinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Photo.gif"),
                    HieuLuc = table.Column<bool>(type: "bit", nullable: false),
                    VaiTro = table.Column<int>(type: "int", nullable: false),
                    RandomKey = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.MaKH);
                });

            migrationBuilder.CreateTable(
                name: "Loai",
                columns: table => new
                {
                    MaLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenLoaiAlias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCap",
                columns: table => new
                {
                    MaNCC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenCongTy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NguoiLienLac = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DienThoai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.MaNCC);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    MaNV = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVien", x => x.MaNV);
                });

            migrationBuilder.CreateTable(
                name: "TrangThai",
                columns: table => new
                {
                    MaTrangThai = table.Column<int>(type: "int", nullable: false),
                    TenTrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrangThai", x => x.MaTrangThai);
                });

            migrationBuilder.CreateTable(
                name: "HangHoa",
                columns: table => new
                {
                    MaHH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenHH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenAlias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaLoai = table.Column<int>(type: "int", nullable: false),
                    MoTaDonVi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DonGia = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    Hinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgaySX = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    GiamGia = table.Column<double>(type: "float", nullable: false),
                    SoLanXem = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaNCC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.MaHH);
                    table.ForeignKey(
                        name: "FK_Products_Categories",
                        column: x => x.MaLoai,
                        principalTable: "Loai",
                        principalColumn: "MaLoai",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers",
                        column: x => x.MaNCC,
                        principalTable: "NhaCungCap",
                        principalColumn: "MaNCC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    MaHD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    NgayCan = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    NgayGiao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(((1)/(1))/(1900))"),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    CachThanhToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Cash"),
                    CachVanChuyen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Airline"),
                    PhiVanChuyen = table.Column<double>(type: "float", nullable: false),
                    MaTrangThai = table.Column<int>(type: "int", nullable: false),
                    MaNV = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.MaHD);
                    table.ForeignKey(
                        name: "FK_HoaDon_NhanVien",
                        column: x => x.MaNV,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDon_TrangThai",
                        column: x => x.MaTrangThai,
                        principalTable: "TrangThai",
                        principalColumn: "MaTrangThai");
                    table.ForeignKey(
                        name: "FK_Orders_Customers",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BanBe",
                columns: table => new
                {
                    MaBB = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaHH = table.Column<int>(type: "int", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgayGui = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.MaBB);
                    table.ForeignKey(
                        name: "FK_BanBe_KhachHang",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH");
                    table.ForeignKey(
                        name: "FK_QuangBa_HangHoa",
                        column: x => x.MaHH,
                        principalTable: "HangHoa",
                        principalColumn: "MaHH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YeuThich",
                columns: table => new
                {
                    MaYT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHH = table.Column<int>(type: "int", nullable: true),
                    MaKH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NgayChon = table.Column<DateTime>(type: "datetime", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.MaYT);
                    table.ForeignKey(
                        name: "FK_Favorites_Customers",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YeuThich_HangHoa",
                        column: x => x.MaHH,
                        principalTable: "HangHoa",
                        principalColumn: "MaHH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHD",
                columns: table => new
                {
                    MaCT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHD = table.Column<int>(type: "int", nullable: false),
                    MaHH = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<double>(type: "float", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    GiamGia = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.MaCT);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders",
                        column: x => x.MaHD,
                        principalTable: "HoaDon",
                        principalColumn: "MaHD",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products",
                        column: x => x.MaHH,
                        principalTable: "HangHoa",
                        principalColumn: "MaHH");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BanBe_MaHH",
                table: "BanBe",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_BanBe_MaKH",
                table: "BanBe",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHD_MaHD",
                table: "ChiTietHD",
                column: "MaHD");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHD_MaHH",
                table: "ChiTietHD",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_HangHoa_MaLoai",
                table: "HangHoa",
                column: "MaLoai");

            migrationBuilder.CreateIndex(
                name: "IX_HangHoa_MaNCC",
                table: "HangHoa",
                column: "MaNCC");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_MaKH",
                table: "HoaDon",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_MaNV",
                table: "HoaDon",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_MaTrangThai",
                table: "HoaDon",
                column: "MaTrangThai");

            migrationBuilder.CreateIndex(
                name: "IX_YeuThich_MaHH",
                table: "YeuThich",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_YeuThich_MaKH",
                table: "YeuThich",
                column: "MaKH");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BanBe");

            migrationBuilder.DropTable(
                name: "ChiTietHD");

            migrationBuilder.DropTable(
                name: "YeuThich");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "HangHoa");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "TrangThai");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "Loai");

            migrationBuilder.DropTable(
                name: "NhaCungCap");
        }
    }
}
