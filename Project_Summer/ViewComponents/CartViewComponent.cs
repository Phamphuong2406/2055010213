using Microsoft.AspNetCore.Mvc;
using Project_Summer.DataContext;
using Project_Summer.Helper;
using Project_Summer.Models;

namespace Project_Summer.ViewComponents
{
    public class CartViewComponent: ViewComponent
    {
        private SummerrContext _context;
        public CartViewComponent(SummerrContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke() // giống như action trong controller 
        {
              var Cart = HttpContext.Session.Get<List<CartItem>>(ConstModel.CART_KEY) ?? new List<CartItem>();
            return View("CartPanel", new CartModel
            {
                Quantity = Cart.Sum(p => p.Soluong)
            });

    }
    } }