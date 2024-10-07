using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Summer.DataContext;
using Project_Summer.Helper;
using Project_Summer.Models;

namespace Project_Summer.ViewComponents
{
    public class SaleSukienViewComponent : ViewComponent
    {
        private SummerrContext _context;
        public SaleSukienViewComponent(SummerrContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke() // giống như action trong controller 
        {
            var data = _context.HangHoas.Where(x => x.MaLoai == 23);
            return View(data);

        }
    }
}

