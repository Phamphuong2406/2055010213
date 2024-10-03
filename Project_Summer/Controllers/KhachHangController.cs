using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_Summer.DataContext;
using Project_Summer.Helper;
using Project_Summer.Models;
using System.Security.Claims;

namespace Project_Summer.Controllers
{
    public class KhachHangController : Controller
    {
        private SummerrContext _context;
        private IMapper _mapper;

        public KhachHangController(SummerrContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }


        [HttpPost]

        public IActionResult DangKy(DangKyVM model, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                var khachhang = _mapper.Map<KhachHang>(model);// map dữ liệu giống nhau từ KhachHang đến model
                khachhang.RandomKey = MyUtil.GenerateRandomKey();//ramdom ra key bằng thuật toám viết trước đó
                khachhang.MatKhau = model.MatKhau.ToMd5Hash(khachhang.RandomKey);
                khachhang.HieuLuc = true;
                khachhang.VaiTro = 0;
                if (Hinh != null)
                {
                    khachhang.Hinh = MyUtil.UploadHinh(Hinh, "khachhang");
                }
                _context.Add(khachhang);
                _context.SaveChanges();
                return RedirectToAction("Index", "Product");

            }

            return View();
        }

        [HttpGet]
        public IActionResult DangNhap(string? returnURL)
        {
            ViewBag.ReturnURL = returnURL;

            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> DangNhap(DangNhapVM model, string? returnURL)
        {
            ViewBag.ReturnURL = returnURL;
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			if (ModelState.IsValid)
            {
                var khachhang = _context.KhachHangs.SingleOrDefault(k => k.MaKh == model.UserName);//makh này là tên
                if (khachhang == null)
                {
                    ModelState.AddModelError("loi", "sai thông tin đăng nhâpj");
                }
                else
                {
                    if (khachhang.MatKhau != model.Password.ToMd5Hash(khachhang.RandomKey))
                    {

                        //báo lỗi
                        ModelState.AddModelError("loi", "sai thông tin đăng nhâpj");

                    }
                    else
                    {
                        // Phân quyền theo vai trò từ cột VaiTro
                        var role = khachhang.VaiTro == 1 ? "Admin" : "Customer"; // 1 là Admin, 0 là Customer

                        var claims = new List<Claim>()// khơỉ tạo các claim thông tin để xác thực danh tính của người dùng khi phân quyền
                        { //claim:yêu cầu
                         new Claim(ClaimTypes.Name, khachhang.HoTen),
                        new Claim(ClaimTypes.Email, khachhang.Email),
                        new Claim(ConstModel.CLAIM_IDUSER, khachhang.MaKh),
                        //claim role động
                        new Claim(ClaimTypes.Role, role),
                    };
                        var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme); // claims này được lưu trữ thông qua cơ chế Cookies (với Cookie Authentication) hoặc Token (JWT).
                        var ClaimsPrincipal = new ClaimsPrincipal(claimsIdentity);//ClaimsPrincipal, chứa các thông tin liên quan đến quyền hạn và thông tin định danh của người dùng trong phiên làm việc hiện tại( được gọi tới thôn qua HttpContext.User)
                        await HttpContext.SignInAsync(ClaimsPrincipal);
                        // sau khi xác thực thành công thì trả về url mong muố
                        if (Url.IsLocalUrl(returnURL))
                        {
                            return Redirect(returnURL);
                        }
                        else
                        {
                            RedirectToAction("Index", "Product");
                        }
                    }
                }
            }
            return View();
        }
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
    }
}
