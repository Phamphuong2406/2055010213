using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Summer.DataContext;
using Project_Summer.Helper;

namespace Project_Summer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private SummerrContext _context;

        public CategoryController(SummerrContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pg=1)
        {
            // var loai = await _context.Loais.OrderByDescending(p => p.MaLoai).ToListAsync();
            List<Loai> loai = await _context.Loais.ToListAsync();// list có số lượng loại
            const int pageSize = 10; //10 items/trang

            if (pg < 1) //page < 1;
            {
                pg = 1; //page ==1
            }
            int recsCount = loai.Count(); //33 items;

            var pager = new Pagination(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

            //category.Skip(20).Take(10).ToList()

            var data = loai.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            return View(data);
          
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Loai model)
        {
            // nếu thông tin điền đúng thì thực hiện 
            
            if (ModelState.IsValid) {
                model.TenLoaiAlias = model.TenLoai.Replace(" ", "-");
                var TenLoaiAlias = await _context.Loais.FirstOrDefaultAsync(p => p.TenLoaiAlias == model.TenLoaiAlias);
                if (TenLoaiAlias != null)
                { // sp đẫ tồn tại
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(model);
                }
                _context.Loais.Add(model);
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
            var loai = await _context.Loais.FindAsync(id);

            return View(loai);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Loai model, int id)
        {
            // nếu thông tin điền đúng thì thực hiện 

            if (ModelState.IsValid)
            {
                var loai = await _context.Loais.FindAsync(id);

                model.TenLoaiAlias = model.TenLoai.Replace(" ", "-");
                var existingLoaiAlias = await _context.Loais.FirstOrDefaultAsync(p => p.TenLoaiAlias == model.TenLoaiAlias && p.MaLoai != id);
                if (existingLoaiAlias != null)
                { // sp đẫ tồn tại
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(model);
                }
                loai.TenLoai = model.TenLoai;
                loai.Hinh = model.Hinh;
                loai.MoTa = model.MoTa;
                loai.TenLoaiAlias = model.TenLoaiAlias;

                try
                {
                    _context.Loais.Update(loai);
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
        public async Task<IActionResult> Delete(int id)
        {
            Loai loai = _context.Loais.Find(id);
            // Kiểm tra nếu sản phẩm đã tồn tại trong giỏ hàng hoặc bảng chi tiết hóa đơn
            bool LoaiInOrder = _context.ChiTietHds.Any(ct => ct.MaCt == id); // Bảng ChiTietHd
            /*      bool productInCart = _context.GioHang.Any(gh => gh.MaHh == id);    // Bảng Giỏ hàng (nếu có)*/

            if (LoaiInOrder)
            {
                // Thông báo nếu sản phẩm đã được đặt trong giỏ hàng hoặc hóa đơn
                TempData["Message"] = "Không thể xóa sản phẩm này vì nó đã được đặt trong giỏ hàng hoặc hóa đơn.";
                return RedirectToAction("Index");
            }
          
            _context.Loais.Remove(loai);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
