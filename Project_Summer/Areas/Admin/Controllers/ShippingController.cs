using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Summer.DataContext;

namespace Project_Summer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippingController : Controller
    {
        private SummerrContext _context;
        public ShippingController(SummerrContext context)
        {

            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.VanChuyens.ToList();
            ViewBag.data = data;
            return View();
        }
        /* public async Task<IActionResult> StoreShipping(VanChuyen model, string )
         {

         }*/
        [HttpPost]
        public async Task<IActionResult> StoreShipping(VanChuyen vanChuyen, string phuong, string tinh, string quan, double Gia)
        {
            vanChuyen.ThanhPho = tinh;
            vanChuyen.Huyen = quan;
            vanChuyen.Xa = phuong;
            vanChuyen.Gia = Gia;

            try
            {
                var existing = await _context.VanChuyens.AnyAsync(x => x.ThanhPho == tinh && x.Huyen == quan && x.Xa == phuong);
                if (existing)// kiểm
                {
                    return Ok(new { duplicate = true, message = "Dữ liệu trùng lặp" });
                }
                _context.VanChuyens.Add(vanChuyen);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Thêm vận chuyển thành công" });


            }
            catch (Exception)
            {
                return StatusCode(500, " Có lỗi xảy ra");
            }

        }

    }
}
