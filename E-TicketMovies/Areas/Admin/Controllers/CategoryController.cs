using E_TicketMovies.Models;
using E_TicketMovies.Repositories;
using E_TicketMovies.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace E_TicketMovies.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository) {
            this.categoryRepository = categoryRepository;

        }
        public IActionResult Index()
        {
            var categories = categoryRepository.Get().ToList(); 
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Create(category);
                categoryRepository.Commit();
                return RedirectToAction("Index", "Category");
            }
            else {

                ModelState.AddModelError("Name","The Name Must be at Least 3 letters");
            }
            return View(category);




        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var oldCat = categoryRepository.GetOne(e => e.Id == id);
            if (oldCat != null) {
                return View(oldCat);    
            }

            return RedirectToAction("NotFoundPage", "Home");

        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid) { 
                categoryRepository.Update(category);
                categoryRepository.Commit();
                return RedirectToAction("Index");
            }

            return View(category);

        }
        public IActionResult Delete (int id) {
            var deletedCat = categoryRepository.GetOne(e => e.Id == id);
            if (deletedCat != null) {
                categoryRepository.Delete(deletedCat);
                categoryRepository.Commit();
                return RedirectToAction(nameof(Index));
            }
          
                return RedirectToAction("NotFoundPage", "Home"); 
        }
    }
}
