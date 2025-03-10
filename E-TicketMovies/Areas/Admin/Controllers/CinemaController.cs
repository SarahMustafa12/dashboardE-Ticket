using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace E_TicketMovies.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CinemaController : Controller
    {
        ICinemaRepository cinemaRepository;
        public CinemaController(ICinemaRepository cinemaRepository)
        {
            this.cinemaRepository = cinemaRepository;
            
        }
        public IActionResult Index()
        {
            var cinema = cinemaRepository.Get().ToList();
            return View(cinema);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var cinema = cinemaRepository.Get().ToList();
            ViewBag.Cinema = cinema;    
            return View(new Cinema());
        }
        [HttpPost]
        public IActionResult Create(Cinema cinema,IFormFile CinemaLogo)
        {
            if (ModelState.IsValid)
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(CinemaLogo.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinema", imageName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    CinemaLogo.CopyTo(stream);
                }
                cinema.CinemaLogo = imageName;

                cinemaRepository.Create(cinema);
                cinemaRepository.Commit();
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cinema = cinemaRepository.GetOne(e => e.Id == id);
            return View(cinema);
        }
        [HttpPost]
        public IActionResult Edit(Cinema cinema, IFormFile? CinemaLogo)
        {
            var updatedCinema = cinemaRepository.GetOne(e => e.Id == cinema.Id,tracked:false);
            if (updatedCinema != null) { 

            if (ModelState.IsValid)
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(CinemaLogo?.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinema", imageName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    CinemaLogo?.CopyTo(stream);
                }
                cinema.CinemaLogo = imageName;

                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinema", updatedCinema.CinemaLogo);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                cinemaRepository.Update(cinema);
                cinemaRepository.Commit();
            }
            else
            {
                cinema.CinemaLogo = updatedCinema.CinemaLogo;
            }
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);
        }
        public IActionResult Delete(int id) {
            var deletedCinema = cinemaRepository.GetOne(e=> e.Id == id);
            if (deletedCinema != null) {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinema", deletedCinema.CinemaLogo);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                cinemaRepository.Delete(deletedCinema);
                cinemaRepository.Commit();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("NotFoundPage", "Home");

        }
    }
}
