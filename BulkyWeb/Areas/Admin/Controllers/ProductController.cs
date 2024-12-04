using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();

            return View(objProductList);
        }

        // API Create
        public IActionResult Create()
        {
            // Projection in EF Core
            IEnumerable<SelectListItem> objCategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            //ViewBag.CategoryList = objCategoryList;
            ViewData["CategoryList"]= objCategoryList;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj)
        {
            // Xử lý điều kiện bên Product.cs
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                // Sử dụng tempdata để _Notification created thành công
                TempData["success"] = "Product created successfully";
                // Chuyển hướng về trang Product/Index
                return RedirectToAction("Index");
            }
            return View();
        }


        // API Update
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            // Product? productFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            // Xử lí điều kiện bên Product.cs
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                // Sử dụng tempdata để _Notification updated thành công
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }


        // API Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            // Sử dụng tempdata để _Notification deleted thành công
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

