using E_TicketMovies.Models;
using E_TicketMovies.Repositories;
using E_TicketMovies.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace E_TicketMovies.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public IActionResult Index()
        {
            var movies = movieRepository.Get().ToList();
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

            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile ImgUrl, List<int> actorsId)
        {
            if (!ModelState.IsValid)
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
            return RedirectToAction("NotFoundPage", "Home");

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
            var updatedMovie = movieRepository.GetOne(e => e.Id == movie.Id, tracked: false);
            var actorMovies = actorMovieRepository.Get(e => e.MovieId == movie.Id).ToList();

            if (updatedMovie != null)
            {
                if (!ModelState.IsValid)
                {

                    if (ImgUrl != null && ImgUrl.Length > 0)
                    {
                        var imageName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", imageName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                           ImgUrl.CopyTo(stream);
                        }


                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", updatedMovie.ImgUrl);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }

                        movie.ImgUrl = imageName;
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

                    // add new selected movies
                    //the part of Except chatgpt modify it and gave me this code cause i uesd Contian method
                    //but i knew that wasn't correct and i understood the reason.
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
                else
                {
                    movie.ImgUrl = updatedMovie.ImgUrl;
                }
            }
            return RedirectToAction("NotFoundPage", "Home");
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
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", deletedMovie.ImgUrl);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                movieRepository.Delete(deletedMovie);
                movieRepository.Commit();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("NotFoundPage", "Home");

        }
    }
}
