using Microsoft.AspNetCore.Mvc;
using Pearl.DataAccess.Data;
using Pearl.DataAccess.Data.Repository.IRepository;
using Pearl.Models;

namespace PearlWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
    {


        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The displayOrder cannot match the Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj); //Vad som ska läggas till
                _unitOfWork.Save();      //Skapar kategorin i databasen
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Edit(int? id) //hämtar in id
        {
            if (id == null || id == 0) //Om id är tomt eller noll.
            {
                return NotFound();
            }
            //Om id inte är tomt eller noll, hämta id för vald category
            Category categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)//Om den hämtade kategorin är tom, visa NotFound.
            {
                return NotFound();
            }
            return View(categoryFromDb); //Skickar kategorin till View
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "The displayOrder cannot match the Name");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj); //Vad som ska läggas till
                _unitOfWork.Save();      //Skapar kategorin i databasen
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Delete(int? id) //hämtar in id
        {
            if (id == null || id == 0) //Om id är tomt eller noll.
            {
                return NotFound();
            }
            //Om id inte är tomt eller noll, hämta id för vald category
            Category categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)//Om den hämtade kategorin är tom, visa NotFound.
            {
                return NotFound();
            }
            return View(categoryFromDb); //Skickar kategorin till View
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {

            Category obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");


        }
    }








}

