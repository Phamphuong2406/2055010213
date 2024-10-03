using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Summer.DataContext;
using Project_Summer.Helper;
using Project_Summer.Models;
using Project_Summer.Repository;
using System.Linq;
using System.Security.Claims;

namespace Project_Summer.Controllers
{
    public class CartController : Controller
    {
        private SummerrContext _context;
        private readonly IEmailSender _emailSender;
        private readonly PaypalClient _paypalClient;

        public CartController(SummerrContext context, IEmailSender emailSender,PaypalClient paypalClient)
        {
            _context = context;
            _emailSender = emailSender;
            _paypalClient = paypalClient;
        }
        public IActionResult Index()
        {
            var GioHang = Cart; // Lấy giỏ hàng từ session
            return View(GioHang);
        }
      
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(ConstModel.CART_KEY) ?? new List<CartItem>();

        public IActionResult AddToCart(int idHH, int  SoLuong = 1)
        {
            var GioHang = Cart;
            var item = Cart.SingleOrDefault(p => p.IDhh == idHH);
            if (item == null)
            {
                var hanghoa = _context.HangHoas.SingleOrDefault(p => p.MaHh == idHH);
                if (hanghoa == null)
                {
                    return NotFound();
                }
                item = new CartItem
                {
                    IDhh = hanghoa.MaHh,
                    Hinh = hanghoa.Hinh ?? string.Empty,
                    DonGia = hanghoa.DonGia ?? 0,
                    Soluong = SoLuong,
                    Tenhh = hanghoa.TenHh
                };
                GioHang.Add(item);

            }
            else
            {
                // tăng số lươngj
                item.Soluong += SoLuong;
            }
            TempData["Message"] = "Thêm giỏ hàng thành công";
            HttpContext.Session.Set(ConstModel.CART_KEY, GioHang);// lưu session
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var GioHang = Cart;
            var item = Cart.SingleOrDefault(p => p.IDhh == id);
            if (item != null)
            {
                GioHang.Remove(item);
                HttpContext.Session.Set(ConstModel.CART_KEY, GioHang);// lưu session
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public IActionResult CheckOut()
        { if(Cart.Count == 0)
            {
                return Redirect("/");
            }
            ViewBag.PaypalClientId = _paypalClient.ClientId;
            
            return View(Cart);
        }
        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckOutVM model, double TongTien)
        {
            if (ModelState.IsValid)
            {
                var idkh = HttpContext.User.Claims.SingleOrDefault(id => id.Type == ConstModel.CLAIM_IDUSER).Value;
                if (idkh == null)
                {
                    // Handle the case where the user is not logged in or the claim is missing
                    return RedirectToAction("Login", "Account");
                }
                var khachhang = new KhachHang();
                if (model.GiongKhachHang)
                {
                    khachhang = _context.KhachHangs.SingleOrDefault(kh => kh.MaKh == idkh);
                }
                var hoadon = new HoaDon()
                {
                    MaKh = idkh,
                    NgayDat = DateTime.Now,
                    HoTen = model.Hoten ?? khachhang.HoTen,
                    DiaChi = model.DiaChi ?? khachhang.DiaChi,
                    DienThoai = model.DienThoai ?? khachhang.DienThoai,
                    TongTien = TongTien,
                    CachThanhToan = "COD",
                    CachVanChuyen = "GRAP",
                    MaTrangThai = 0,

                    GhiChu = model.GhiChu

                };
                _context.Database.BeginTransaction();
                try
                {
                    _context.Database.CommitTransaction();
                    _context.Add(hoadon);
                    _context.SaveChanges();
                    var ChiTietHD = new List<ChiTietHd>();
                    foreach (var item in Cart)
                    {
                        ChiTietHD.Add(new ChiTietHd
                        {
                            MaHd = hoadon.MaHd,
                            SoLuong = item.Soluong,
                            DonGia = item.DonGia,
                            MaHh = item.IDhh,
                            GiamGia = 0
                        });
                        var sp = _context.HangHoas.Where(sp => sp.MaHh == item.IDhh).FirstOrDefault();
                        if (sp != null)
                        {
                            sp.SoLuong -= item.Soluong;
                            sp.DaBan += item.Soluong;
                            _context.Update(sp);
                        }
                        _context.Update(sp);
                    };
                   
                    _context.AddRange(ChiTietHD);
                    _context.SaveChanges();                 // là từ khóa hay gọi là sessionID 
                    HttpContext.Session.Set<List<CartItem>>(ConstModel.CART_KEY, Cart);// Khi bạn thực hiện yêu cầu tiếp theo, Session ID từ cookie được gửi kèm, và server sử dụng nó để tìm và truy xuất dữ liệu session tương ứng.

                    var receiver = "nhi880031@gmail.com";
                    var subject = "Bạn đã đặt hangf thành công";
                    var message = "Mọi Thắc mắc xin liện hệ cho chúng tôi";
                    await _emailSender.SendEmailAsync(receiver, subject, message);
                      
                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    _context.Database.RollbackTransaction();
                }
               
            }
            return View(Cart);
        }
        [Authorize]
        [HttpPost("/Cart/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            // thông tin đơn hàng gửi qua paypal
            var tongtien = Cart.Sum(p => p.ThanhTien).ToString();
            var donvitien = "USD";
            var maDonHnagThamChieu = "DH" + DateTime.Now.Ticks.ToString();
            try
            {
                var response = await _paypalClient.CreateOrder(tongtien, donvitien, maDonHnagThamChieu);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }
        // lưu đơn hàng
        [Authorize]
        [HttpPost("/Cart/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);
                if (response.status.ToLower() != "completed")
                {
                    throw new Exception("Thanh toán PayPal không thành công.");
                }

                // Lấy ID khách hàng từ claims
                var idkh = HttpContext.User.Claims.SingleOrDefault(id => id.Type == ConstModel.CLAIM_IDUSER).Value;
                if (idkh == null)
                {
                    // Handle the case where the user is not logged in or the claim is missing
                    return RedirectToAction("DangNhap", "KhachHang");
                }
               
                // Lấy thông tin khách hàng từ cơ sở dữ liệu
                var khachHang = _context.KhachHangs.SingleOrDefault(kh => kh.MaKh == idkh);
                if (khachHang == null)
                {
                    ModelState.AddModelError("", "Khách hàng không tồn tại.");
                    return BadRequest(new { Message = "Khách hàng không tồn tại." });
                }

                // Tạo hóa đơn mới
                var hoadon = new HoaDon
                {
                    MaKh = idkh,
                    HoTen = khachHang.HoTen,
                    DiaChi = khachHang.DiaChi,
                    DienThoai = khachHang.DienThoai,
                    NgayDat = DateTime.Now,
                    CachThanhToan = "PayPal",
                    CachVanChuyen = "GRAB",
                    MaTrangThai = 0,
                    GhiChu = "Thanh toán qua PayPal"
                };

                _context.Add(hoadon);
                _context.SaveChanges();
                // Tạo chi tiết hóa đơn từ giỏ hàng
                var ChiTietHD = new List<ChiTietHd>();
                foreach (var item in Cart)
                {
                    ChiTietHD.Add(new ChiTietHd
                    {
                        MaHd = hoadon.MaHd,
                        SoLuong = item.Soluong,
                        DonGia = item.DonGia,
                        MaHh = item.IDhh,
                        GiamGia = 0
                    });
                    var sp = _context.HangHoas.Where(sp => sp.MaHh == item.IDhh).FirstOrDefault();
                    if (sp != null)
                    {
                        sp.SoLuong -= item.Soluong;
                        sp.DaBan += item.Soluong;
                        _context.Update(sp);
                    }
                    _context.Update(sp);
                };

                _context.AddRange(ChiTietHD);
                _context.SaveChanges();                 // là từ khóa hay gọi là sessionID 
                HttpContext.Session.Set<List<CartItem>>(ConstModel.CART_KEY, Cart);// Khi bạn thực hiện yêu cầu tiếp theo, Session ID từ cookie được gửi kèm, và server sử dụng nó để tìm và truy xuất dữ liệu session tương ứng.

                var receiver = "nhi880031@gmail.com";
                var subject = "Bạn đã đặt hangf thành công";
                var message = "Mọi Thắc mắc xin liện hệ cho chúng tôi";
                await _emailSender.SendEmailAsync(receiver, subject, message);


                // Xóa giỏ hàng sau khi hoàn thành đơn hàng
                HttpContext.Session.Set<List<CartItem>>(ConstModel.CART_KEY, new List<CartItem>());

                //Lưu database đơn hàng
                // lưu đơn hàng vào database
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }

        }
        public IActionResult Increase(int id)
        {
            List<CartItem> Cart = HttpContext.Session.Get<List<CartItem>>(ConstModel.CART_KEY);// lấy list sản phẩm trong giỏ hàng
            CartItem cartItem = Cart.Where(p => p.IDhh == id).FirstOrDefault();// lấy ra sản phẩm cần giảm số lượng
            if (cartItem.Soluong >= 1)
            {
                ++cartItem.Soluong;
                
            }
            else
            {
                // nếu soluog <1 thì xóa sản phẩm đó trong giỏ hàng theo id
                Cart.RemoveAll(p => p.IDhh == id);

            }
            if (Cart.Count == 0)
            {
                HttpContext.Session.Remove(ConstModel.CART_KEY);
            }
            else
            {
                HttpContext.Session.Set(ConstModel.CART_KEY, Cart);// nếu <= 1 thì xóa giỏ hàng và tạo giỏ hàng mới
            }
            return RedirectToAction("Index");
        }
    
        public async Task<IActionResult> Decrease(int id)
        {
            var data = await _context.HangHoas.Where(p =>p.MaLoai == id).FirstOrDefaultAsync();
            List<CartItem> Cart = HttpContext.Session.Get<List<CartItem>>(ConstModel.CART_KEY) ;// lấy list sản phẩm trong giỏ hàng
            CartItem cartItem = Cart.Where(p => p.IDhh == id).FirstOrDefault();// lấy ra sản phẩm cần giảm số lượng
           
                if (cartItem.Soluong > 1 && cartItem.Soluong <= data.SoLuong)
                {
                    --cartItem.Soluong;

                }
                else
                {
                if (cartItem.Soluong == data.SoLuong)
                {
                    TempData["message"] = "sản phẩm đa đạt số lươngj tối đa";
                }

                }
            if (Cart.Count == 0) {
               HttpContext.Session.Remove(ConstModel.CART_KEY);
            }
            else
            {
                HttpContext.Session.Set(ConstModel.CART_KEY, Cart);// nếu <= 1 thì xóa giỏ hàng và tạo giỏ hàng mới
            }
            return RedirectToAction("Index");
        }
        
        public IActionResult Success()
        {
            // tìm id khách hàng hiện tại trong session
            var claim = HttpContext.User.Claims.SingleOrDefault(id => id.Type == ConstModel.CLAIM_IDUSER);
            // Kiểm tra xem claim có tồn tại hay không
            if (claim == null || string.IsNullOrEmpty(claim.Value))
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }

            var idkh = claim.Value;
            ViewBag.Idkh = idkh;
            return View();
        }
        public IActionResult ViewOder(string idkh) {
           
            var hoadon = _context.HoaDons.Where(hd => hd.MaKh == idkh).ToList();
            // Kiểm tra xem khách hàng có đơn hàng không
            if (hoadon == null || hoadon.Count == 0)
            {
                ViewBag.Message = "Không tìm thấy hóa đơn nào cho khách hàng này.";
                return View("Error"); // Bạn có thể hiển thị view lỗi hoặc thông báo khác
            }
            return View(hoadon);
        }
        public IActionResult ViewDetails (int id)
        {
            var chitiethd = _context.ChiTietHds.Where(x => x.MaHd == id).ToList();
            // Kiểm tra xem hóa đơn có chi tiết nào không
            if (chitiethd == null || chitiethd.Count == 0)
            {
                ViewBag.Message = "Không tìm thấy chi tiết hóa đơn nào.";
                return View("Error"); // Bạn có thể hiển thị view lỗi hoặc thông báo khác
            }
            return View(chitiethd);
        }



    }
}
