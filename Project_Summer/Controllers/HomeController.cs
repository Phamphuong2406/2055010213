using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Summer.DataContext;
using Project_Summer.Models;
using System.Diagnostics;

namespace Project_Summer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private SummerrContext _context;

        public HomeController(ILogger<HomeController> logger, SummerrContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var sp = _context.HangHoas.Include(p =>p.MaLoaiNavigation).ToList();
            return View(sp); 
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
        public IActionResult Loi()
        {
            return View();
        }
        
        public IActionResult GioiThieu()
        {
            return View();
        }
    }
}
