using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Summer.DataContext;

namespace Project_Summer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProviderController : Controller
    {
        private SummerrContext _context;
        public ProviderController(SummerrContext context)
        {
           
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
           
            return View(await _context.NhaCungCaps.ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(NhaCungCap model)
        {
            // nếu thông tin điền đúng thì thực hiện 

            if (ModelState.IsValid)
            {
                
                var TenLoaiAlias = await _context.NhaCungCaps.FirstOrDefaultAsync(p => p.TenCongTy == model.TenCongTy);
                if (TenLoaiAlias != null)
                { // sp đẫ tồn tại
                    ModelState.AddModelError("", "Nhà cung cấp đã có trong database");
                    return View(model);
                }
                _context.NhaCungCaps.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Thông tin chưa đúng";
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ncc = await _context.NhaCungCaps.FindAsync(id);

            return View(ncc);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(NhaCungCap model, int id)
        {
            // nếu thông tin điền đúng thì thực hiện 

            if (ModelState.IsValid)
            {
                var ncc = await _context.NhaCungCaps.FindAsync(id);

            
                var existingNcc = await _context.NhaCungCaps.FirstOrDefaultAsync(p => p.TenCongTy == model.TenCongTy && p.MaNcc != id);
                if (existingNcc != null)
                { // sp đẫ tồn tại
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(model);
                }
                ncc.TenCongTy = model.TenCongTy;
               ncc.NguoiLienLac = model.NguoiLienLac;
                ncc.MoTa = model.MoTa;
                ncc.DiaChi = model.DiaChi;


                try
                {
                    _context.NhaCungCaps.Update(ncc);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ nếu có vấn đề khi cập nhật
                    ModelState.AddModelError("", "Có lỗi khi cập nhật: " + ex.Message);
                }
            }
            else
            {
                TempData["Message"] = "Thông tin chưa đúng";
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(string id)
        {
            NhaCungCap ncc = _context.NhaCungCaps.Find(id);
            // Kiểm tra nếu sản phẩm đã tồn tại trong giỏ hàng hoặc bảng chi tiết hóa đơn
           // bool LoaiInOrder = _context.ChiTietHds.Any(ct => ct.MaCt == id); // Bảng ChiTietHd
            /*      bool productInCart = _context.GioHang.Any(gh => gh.MaHh == id);    // Bảng Giỏ hàng (nếu có)*/

/*            if (LoaiInOrder)
            {
                // Thông báo nếu sản phẩm đã được đặt trong giỏ hàng hoặc hóa đơn
                TempData["Message"] = "Không thể xóa sản phẩm này vì nó đã được đặt trong giỏ hàng hoặc hóa đơn.";
                return RedirectToAction("Index");
            }*/

            _context.NhaCungCaps.Remove(ncc);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
