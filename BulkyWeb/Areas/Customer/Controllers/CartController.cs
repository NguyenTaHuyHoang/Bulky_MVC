using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var clamesIdentity = (ClaimsIdentity)User.Identity; //  lấy thông tin về người dùng hiện tại từ thuộc tính User, vốn chứa chi tiết về người dùng đã đăng nhập. Đối tượng Identity được chuyển sang kiểu ClaimsIdentity để truy cập các claim liên quan đến người dùng.
            var userId = clamesIdentity.FindFirst(ClaimTypes.NameIdentifier).Value; // lấy ID duy nhất của người dùng (thường là ID người dùng trong hệ thống) từ các claim của người dùng. 

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product")
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }
        
        public IActionResult Summary()
        {
            return View();
        }

        public IActionResult Plus(int cardId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cardId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cardId)
           {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cardId);
            if (cartFromDb.Count <= 1)
               {
                   // remove 
                   _unitOfWork.ShoppingCart.Remove(cartFromDb);
               }
               else
               {
                   cartFromDb.Count -= 1;
                   _unitOfWork.ShoppingCart.Update(cartFromDb);
               }
               _unitOfWork.Save();
               return RedirectToAction(nameof(Index));
           }

        public IActionResult Remove(int cardId)
           {
               var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cardId);
               // remove 
               _unitOfWork.ShoppingCart.Remove(cartFromDb);
               _unitOfWork.Save();
               return RedirectToAction(nameof(Index));
           }


        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }
    }
}
