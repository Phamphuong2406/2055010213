using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Summer.DataContext;
using Project_Summer.Models;
using System;

namespace Project_Summer.Controllers
{
    public class ProductController : Controller
    {
        private SummerrContext _context;
        public ProductController(SummerrContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? loai)
        {
            var hanghoas = _context.HangHoas.AsQueryable();
            if(loai.HasValue)
            {
                hanghoas = hanghoas.Where(p => p.MaLoai == loai.Value);
            }

            var data = hanghoas.Select(p => new HangHoaVM
            {
                IdHH = p.MaHh,
                TenHH = p.TenHh,
                TenLoai = p.MaLoaiNavigation.TenLoai,
                Hinh = p.Hinh ?? " ",
                GiaHH = p.DonGia ?? 0,
                MoTa = p.MoTa ?? "",
                SoLuong = p.SoLuong


            });
            return View(data);
        }
        public IActionResult Detail(int? id)
        {
            var product = _context.HangHoas
               .Include(p =>p.MaLoaiNavigation)
               .Include(p =>p.NhanXets)
                .SingleOrDefault( p => p.MaHh == id);// tìm sp trong db
           /* if(product == null)
            {
                return RedirectToAction("Loi","Home");
            }*/
            var datas = new HangHoaDetailVM 
            {
                IdHH = product.MaHh,
                TenHH = product.TenHh,
                GiaHH = product.DonGia ?? 0,
                Hinh = product.Hinh ?? " ",
                Mota = product.MoTa ?? " ",
                TenLoai = product.MaLoaiNavigation.TenLoai
            };
            // Gói HangHoaDetailVM vào ProductDetailVM
            var productDetailVM = new ProductDetailVM
            {
                HangHoaDetailVM = datas,
                HoTen = "", // Chưa có thông tin, để trống
                Email = "", // Chưa có thông tin, để trống
                NoiDung = "" // Chưa có nội dung đánh giá, để trống
            };
            return View(productDetailVM);

        }
        [HttpPost]
        public IActionResult Reviews(ProductDetailVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.HangHoaDetailVM == null)
                {
                    // Xử lý khi HangHoaDetailVM là null
                    ViewBag.Message = "Có lỗi xảy ra, vui lòng thử lại.";
                    return RedirectToAction("Detail", new { id = model.HangHoaDetailVM?.IdHH });
                }
                var data = new NhanXet
                {
                    MaHh = model.HangHoaDetailVM.IdHH,
                    HoTen = model.HoTen,
                    Email = model.Email,
                    NoiDung = model.NoiDung,
                    NgayDang = DateTime.Now,
                };
                _context.NhanXets.Add(data);
                _context.SaveChanges();
            }
            ViewBag.Message = "Thêm đánh giá thành công";
            return RedirectToAction("Detail",  new { id = model.HangHoaDetailVM.IdHH });
        }
        public async Task<IActionResult> Search(string searchform)
        {
            // Kiểm tra xem từ khóa tìm kiếm có rỗng không
            if (string.IsNullOrEmpty(searchform))
            {
                return RedirectToAction("Index"); // Nếu không có từ khóa, trả về trang chính
            }
            var products = await _context.HangHoas
                .Where(p => p.TenHh.Contains(searchform) || p.MoTa.Contains(searchform))
                .Select(p => new HangHoaVM
                {
                    IdHH = p.MaHh,
                    TenHH = p.TenHh,
                    TenLoai = p.MaLoaiNavigation.TenLoai,
                    Hinh = p.Hinh ?? " ",
                    GiaHH = p.DonGia ?? 0,
                    MoTa = p.MoTa ?? ""
                })
                .ToListAsync();
            ViewBag.Key = searchform; 
            return View(products);
        }

    }
}
