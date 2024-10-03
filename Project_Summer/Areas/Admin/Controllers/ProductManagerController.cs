using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_Summer.Areas.Admin.Models;
using Project_Summer.DataContext;
using Project_Summer.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Summer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductManagerController : Controller
    {
        private SummerrContext _context;
        private readonly IWebHostEnvironment _webHostEnvirment;// lấy đừng dẫn lưu ảnh
        public ProductManagerController(SummerrContext context, IWebHostEnvironment webHostEnvirment)
        {
            _webHostEnvirment = webHostEnvirment;
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var list = await _context.HangHoas//// Lấy danh sách sản phẩm và bao gồm cả loại và nhà cung cấp
                .Include(p => p.MaLoaiNavigation)
                .Include(p => p.MaNccNavigation)
                .ToListAsync();
                return View(list);
            }
            else
            {
				TempData["ErrorMessage"] = "Bạn không có quyền";
				return RedirectToAction("AccessDenied", "Home");
            }
          /*  var list = await _context.HangHoas//// Lấy danh sách sản phẩm và bao gồm cả loại và nhà cung cấp
                .Include(p => p.MaLoaiNavigation)
                .Include(p => p.MaNccNavigation)
                .ToListAsync();
            return View(list);*/
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Loai = new SelectList(_context.Loais, "MaLoai", "TenLoai");
            ViewBag.NhaCungCap = new SelectList(_context.NhaCungCaps, "MaNcc", "TenCongTy");
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HangHoa model)
        {
            ViewBag.Loai = new SelectList(_context.Loais, "MaLoai", "TenLoai", model.MaLoai);
            ViewBag.NhaCungCap = new SelectList(_context.NhaCungCaps, "MaNcc", "TenCongTy", model.MaNcc);
            if (ModelState.IsValid)
            {
                model.TenAlias = model.TenHh.Replace(" ", "-");
                model.SoLuong = 0;
                var TenAlias = await _context.HangHoas.FirstOrDefaultAsync(p => p.TenAlias == model.TenAlias);
                if (TenAlias != null)
                { // sp đẫ tồn tại
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(model);
                }
                //nếu chưas có thì tạo mới
                if (model.HinhUpload != null)// nếu người dùng đã tải ảnh lên
                {
                    // thư mục chư ảnh tải lên
                    string uploadsDir = Path.Combine(_webHostEnvirment.WebRootPath, "media/products");
                    // tạo tên ảnh mới đảm bảo không trùng lặp
                    string ImageName = Guid.NewGuid().ToString() + "_" + model.HinhUpload.FileName;// tạo tên ảnh dạng chuỗi
                    string filePath = Path.Combine(uploadsDir, ImageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await model.HinhUpload.CopyToAsync(fs);//// copy hình đã tải lên vào đối tượng fs 
                    fs.Close();// sau khi upload thanhf coong vaof file thif đóng lại
                    model.Hinh = ImageName;
                }

                _context.Add(model);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }
            else
            {

                ViewBag.Message = " Thông tin chưa chính xác";
               /* List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);*/
            }
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            HangHoa product = await _context.HangHoas.FindAsync(id);
            ViewBag.Loai = new SelectList(_context.Loais, "MaLoai", "TenLoai", product.MaLoai);
            ViewBag.NhaCungCap = new SelectList(_context.NhaCungCaps, "MaNcc", "TenCongTy", product.MaNcc);

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HangHoa model, int id)
        {
            ViewBag.Loai = new SelectList(_context.Loais, "MaLoai", "TenLoai", model.MaLoai);
            ViewBag.NhaCungCap = new SelectList(_context.NhaCungCaps, "MaNcc", "TenCongTy", model.MaNcc);
            if (ModelState.IsValid)
            {
                model.TenAlias = model.TenHh.Replace(" ", "-");
                var TenAlias = await _context.HangHoas.FirstOrDefaultAsync(p => p.TenAlias == model.TenAlias);
                if (TenAlias != null)
                { // sp đẫ tồn tại
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(model);
                }
                //nếu chưa có thì tạo mới
                if (model.HinhUpload != null)
                {
                    // thư mục chư ảnh tải lên
                    string uploadsDir = Path.Combine(_webHostEnvirment.WebRootPath, "media/products");
                    // tạo tên ảnh mới đảm bảo không trùng lặp
                    string ImageName = Guid.NewGuid().ToString() + "_" + model.HinhUpload.FileName;// tạo tên ảnh dạng chuỗi
                    string filePath = Path.Combine(uploadsDir, ImageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await model.HinhUpload.CopyToAsync(fs);
                    fs.Close();// sau khi upload thanhf coong vaof file thif đóng lại
                    model.Hinh = ImageName;
                }

                _context.Update(model);
                await _context.SaveChangesAsync();
                ViewBag.Message = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }
            else
            {

                TempData["error"] = " Thông tin chưa chính xác";
                /* List<string> errors = new List<string>();
                 foreach (var value in ModelState.Values)
                 {
                     foreach (var error in value.Errors)
                     {
                         errors.Add(error.ErrorMessage);
                     }
                 }
                 string errorMessage = string.Join("\n", errors);
                 return BadRequest(errorMessage);*/
            }
            return View(model);

        }
        public async Task<IActionResult> Delete(int id)
        {
            var product = _context.HangHoas.Find(id);
            // Kiểm tra nếu sản phẩm đã tồn tại trong giỏ hàng hoặc bảng chi tiết hóa đơn
            bool productInOrder = _context.ChiTietHds.Any(ct => ct.MaHh == id); // Bảng ChiTietHd
      /*      bool productInCart = _context.GioHang.Any(gh => gh.MaHh == id);    // Bảng Giỏ hàng (nếu có)*/

            if (productInOrder)
            {
                // Thông báo nếu sản phẩm đã được đặt trong giỏ hàng hoặc hóa đơn
                ViewBag.Message = "Không thể xóa sản phẩm này vì nó đã được đặt trong giỏ hàng hoặc hóa đơn.";
                return RedirectToAction("Index");
            }
            if (!string.Equals(product.Hinh,"name.jpg"))
            {
                string uploadsDir = Path.Combine(_webHostEnvirment.WebRootPath, "media/products");
                string filePath = Path.Combine(uploadsDir, product.Hinh);
                if (System.IO.File.Exists(filePath)) { 
                    System.IO.File.Delete(filePath);
                }
            }
            ViewBag.Message = "Xóa thành công";
            _context.HangHoas.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AddQuantity(int id) // view thêm số lượng
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        [Route("UpdateProductQuantity")]
        public IActionResult UpdateProductQuantity(SoLuongSp model)// cộng số lượng vào soluong trong product
        {
            var product = _context.HangHoas.Find(model.MaHh);
            if (product == null)
            {
                return NotFound();
            }
            product.SoLuong += model.SoLuong;// số lượng cũ cộng với số lượng mới => gán giá trị cho product.soluong
            model.SoLuong = model.SoLuong;// không thay đổi về giá trị chỉ gán lại chính nó

            model.MaHh = model.MaHh;
            model.NgayCapNhat = DateTime.Now;

            _context.Add(model);
            _context.SaveChanges();
            TempData["Messege"] = "Thêm số lượng thành công";
            return RedirectToAction("AddQuantity", new { id = model.MaHh });
        }
    }
}
/*[NotMapped]
[FileExtension]
public IFormFile HinhUpload { get; set; }*/
