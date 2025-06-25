using System.Diagnostics.Eventing.Reader;
using E_TicketMovies.Models;
using E_TicketMovies.Repositories;
using E_TicketMovies.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_TicketMovies.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class MovieController : Controller
    {
        IMovieRepository movieRepository;
        IActorRepository actorRepository;
        IActorMovieRepository actorMovieRepository;
        ICinemaRepository cinemaRepository;
        ICategoryRepository categoryRepository;
        public MovieController(IMovieRepository movieRepository, IActorRepository actorRepository , IActorMovieRepository actorMovieRepository ,ICategoryRepository categoryRepository, ICinemaRepository cinemaRepository )
        {
            this.movieRepository = movieRepository;
            this.actorRepository = actorRepository;
            this.actorMovieRepository = actorMovieRepository;
            this.categoryRepository = categoryRepository;
            this.cinemaRepository = cinemaRepository;
                
            
        }
        public IActionResult Index(string? query, int page)
        {
            var movies = movieRepository.Get();

            if(query!= null)
            {
                movies = movieRepository.Get(e=>e.Name.Contains(query));
            }

            int totalCount = movies.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            if (page > totalPages && totalPages > 0)
                return RedirectToAction("NotFoundPage", "Home", new { area = "End User" });

            movies = movies.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.totalPages = totalPages;

            return View(movies);
        }
        [HttpGet]
        public IActionResult Create()
        {

            var actors = actorRepository.Get().ToList();
            ViewBag.Actors = actors; 
            
            var cinema = cinemaRepository.Get().ToList();
            ViewBag.Cinema = cinema;    

            var category = categoryRepository.Get().ToList();
            ViewBag.Category = category;

            return View(new Movie ());
        }

        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile ImgUrl, List<int> actorsId)
        {
            var actors = actorRepository.Get().ToList();
            ViewBag.Actors = actors;
            var cinema = cinemaRepository.Get().ToList();
            ViewBag.Cinema = cinema;

            var category = categoryRepository.Get().ToList();
            ViewBag.Category = category;

            if (ModelState.IsValid)
            {
                if (ImgUrl != null && ImgUrl.Length > 0)
                {
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", imageName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ImgUrl.CopyTo(stream);
                    }
                    movie.ImgUrl = imageName;

                }
                movieRepository.Create(movie);
                movieRepository.Commit();
                               
                List<ActorMovie> actorMovies = new();
                foreach (var id in actorsId)
                {
                    actorMovies.Add(new ActorMovie
                    {
                        MovieId = movie.Id,
                        ActorId = id,
                        Actor = actorRepository.GetOne(e => e.Id == id),
                        Movie = movie
                    });
                }

                if (actorMovies.Count > 0)
                {
                    actorMovieRepository.Create(actorMovies);
                    actorMovieRepository.Commit();
                }
                return RedirectToAction(nameof(Index));

            }

            return View(movie);

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = movieRepository.GetOne(e=>e.Id == id);
            var actors = actorRepository.Get().ToList();
            ViewBag.Actors = actors;

            var cinema = cinemaRepository.Get().ToList();
            ViewBag.Cinema = cinema;

            var category = categoryRepository.Get().ToList();
            ViewBag.Category = category;
            var selectedActors = actorMovieRepository.Get(e => e.MovieId == id).Select(e => e.ActorId).ToList();
            ViewBag.SelectedActors = selectedActors;

            return View(movie);
        }
        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile ImgUrl, List<int> actorsId)
        {
            var actors = actorRepository.Get().ToList();
            ViewBag.Actors = actors;

            var cinema = cinemaRepository.Get().ToList();
            ViewBag.Cinema = cinema;

            var category = categoryRepository.Get().ToList();
            ViewBag.Category = category;
            var updatedMovie = movieRepository.GetOne(e => e.Id == movie.Id, tracked: false);
            var actorMovies = actorMovieRepository.Get(e => e.MovieId == movie.Id).ToList();
                if (ModelState.IsValid)
                {
                    if (ImgUrl != null && ImgUrl.Length > 0)
                    {
                        var imageName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", imageName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            ImgUrl.CopyTo(stream);
                        }
                          movie.ImgUrl = imageName;
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", updatedMovie.ImgUrl);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                        
                    }
                    else
                    {
                        movie.ImgUrl = updatedMovie.ImgUrl;
                    }

                    movieRepository.Update(movie);
                    movieRepository.Commit();

                    // handlle selected actors
                    var existingActorMovies = actorMovieRepository.Get(e => e.MovieId == movie.Id).ToList();
                    var existingActorsIds = existingActorMovies.Select(e => e.ActorId).ToList();
                    // remove unselected actors
                    var actorsToRemove = existingActorMovies.Where(e => !actorsId.Contains(e.ActorId)).ToList();
                    foreach (var actor in actorsToRemove)
                    {
                        actorMovieRepository.Delete(actor);
                    }

                    
                    var actorsToAdd = actorsId.Except(existingActorsIds) // Get new movies only.
                                       .Select(actorId => new ActorMovie { MovieId = movie.Id, ActorId = actorId })
                                       .ToList();

                    if (actorsToAdd.Any())
                    {
                        actorMovieRepository.Create(actorsToAdd);
                        actorMovieRepository.Commit();
                    }
                    return RedirectToAction(nameof(Index));
                }
                         
            return View(movie);
        }

        public IActionResult Delete(int id)
        {
            var deletedMovie = movieRepository.GetOne(e=>e.Id == id);
            var exsistMoviesActor = actorMovieRepository.Get(e => e.MovieId == id);
            if (exsistMoviesActor != null) {
                foreach (var item in exsistMoviesActor) {
                    actorMovieRepository.Delete(item);
                    actorMovieRepository.Commit();
                }
            }
            if (deletedMovie != null)
            {
                if (deletedMovie.ImgUrl != null) { 
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", deletedMovie.ImgUrl);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

                movieRepository.Delete(deletedMovie);
                movieRepository.Commit();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("NotFoundPage", "Home");

        }
    }
}
