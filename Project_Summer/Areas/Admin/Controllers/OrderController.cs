using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Summer.DataContext;

namespace Project_Summer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private SummerrContext _context;
        public OrderController(SummerrContext context) {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var hoadon = await _context.HoaDons.OrderByDescending(p => p.MaHd).ToListAsync();
            return View(hoadon);
        }
        public async Task<IActionResult> Details(int id)
        {
            var detail = await _context.ChiTietHds.Include(p => p.MaHhNavigation).Where(hd => hd.MaHd == id).ToListAsync();
            return View(detail);
        }
        // xóa đơn hàng
       
        public  IActionResult Delete(int id)
        {
            // xóa chi tiết đơn hàng
            var hd = _context.HoaDons.Include(p => p.ChiTietHds).Where(p => p.MaHd == id).FirstOrDefault();
            if(hd == null)
            {
                ViewBag.message = " không tìm thấy đơn hàng";
            }
            foreach(var ct in hd.ChiTietHds.ToList())
            {
                _context.ChiTietHds.Remove(ct);
            }
            // xóa đơn hàng
            _context.HoaDons.Remove(hd);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
