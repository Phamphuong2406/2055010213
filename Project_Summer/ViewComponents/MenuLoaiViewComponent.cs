using Microsoft.AspNetCore.Mvc;
using Project_Summer.DataContext;
using Project_Summer.Models;

namespace Project_Summer.ViewComponents

{
    public class MenuLoaiViewComponent: ViewComponent 
    {
        private SummerrContext _context;
        public MenuLoaiViewComponent(SummerrContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke() // giống như action trong controller 
        {
            var data = _context.Loais.Select(lo => new LoaiVM
            {
                IdLoai = lo.MaLoai,
                TenLoai = lo.TenLoai,
                SoLuong = lo.HangHoas.Count
            }).OrderBy(p => p.TenLoai);
            return View(data);// trả về view.cshtml
            //return View("Default", data);
            


        }

    }
}
