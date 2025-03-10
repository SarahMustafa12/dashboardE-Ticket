using E_TicketMovies.Models;
using E_TicketMovies.Repositories;
using E_TicketMovies.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_TicketMovies.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        IActorRepository actorRepository;
        IMovieRepository movieRepository;
        IActorMovieRepository actorMovieRepository;
        public ActorController(IActorRepository actorRepository, IMovieRepository movieRepository, IActorMovieRepository actorMovieRepository)
        {
            this.actorRepository = actorRepository;
            this.movieRepository = movieRepository;
            this.actorMovieRepository = actorMovieRepository;
        }
        public IActionResult Index()
        {
            var actors = actorRepository.Get().ToList();
            return View(actors);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var movies = movieRepository.Get().ToList();
            ViewBag.Movies = movies;

            return View();
        }
        [HttpPost]
        public IActionResult Create(Actor actor, IFormFile ProfilePicture, List<int> moviesId)
        {
  
            if (ModelState.IsValid)
            {
                if (ProfilePicture != null && ProfilePicture.Length > 0)
                {
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", imageName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ProfilePicture.CopyTo(stream);
                    }
                    actor.ProfilePicture = imageName;
                }
            
                actorRepository.Create(actor);
                actorRepository.Commit();

                List<ActorMovie> actorMovies = new();
                foreach (var id in moviesId)
                {
                    actorMovies.Add(new ActorMovie
                    {
                        MovieId = id,
                        ActorId = actor.Id,
                        Actor = actor,
                        Movie = movieRepository.GetOne(e => e.Id == id)
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
            var actor = actorRepository.GetOne(e=>e.Id == id);
            var movies = movieRepository.Get().ToList();
            ViewBag.Movies = movies;
            var selectedMovies = actorMovieRepository.Get(e=>e.ActorId == id).Select(e=>e.MovieId).ToList();
            ViewBag.SelectedMovies = selectedMovies;
            return View(actor);
        }
        [HttpPost]
        public IActionResult Edit(Actor actor, IFormFile ProfilePicture, List<int> moviesId)
        {
            var updatedActor = actorRepository.GetOne(e => e.Id == actor.Id ,tracked: false);
            var actorMovies = actorMovieRepository.Get(e => e.ActorId == actor.Id).ToList();

            if (updatedActor != null)
            {
                if (ModelState.IsValid)
                {
                   
                    if (ProfilePicture != null)
                    {
                        var imageName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", imageName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            ProfilePicture.CopyTo(stream);
                        }

                        
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", updatedActor.ProfilePicture);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }

                        actor.ProfilePicture = imageName;
                    }
                    else
                    {
                        actor.ProfilePicture = updatedActor.ProfilePicture;
                    }

                    actorRepository.Update(actor);
                    actorRepository.Commit();

                    // handlle selected movies
                    var existingActorMovies = actorMovieRepository.Get(e => e.ActorId == actor.Id).ToList();
                    var existingMovieIds = existingActorMovies.Select(e=>e.MovieId).ToList();
                    // remove unselected movies
                    var moviesToRemove = existingActorMovies.Where(e => !moviesId.Contains(e.MovieId)).ToList();
                    foreach (var movie in moviesToRemove)
                    {
                        actorMovieRepository.Delete(movie);
                    }

                    // add new selected movies
                    //the part of Except chatgpt modify it and gave me this code cause i uesd Contian method
                    //but i knew that wasn't correct and i understood the reason.
                    var moviesToAdd = moviesId.Except(existingMovieIds) // Get new movies only.
                                       .Select(movieId => new ActorMovie { ActorId = actor.Id, MovieId = movieId })
                                       .ToList();

                    if (moviesToAdd.Any())
                    {
                        actorMovieRepository.Create(moviesToAdd);
                        actorMovieRepository.Commit();
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    actor.ProfilePicture = updatedActor.ProfilePicture;
                }
            }
            return RedirectToAction("NotFoundPage", "Home");
        }




        public IActionResult Delete(int id)
        {
            var deletedActor = actorRepository.GetOne(e => e.Id == id);
            var exsistMoviesActor = actorMovieRepository.Get(e=>e.ActorId == id);
            if (exsistMoviesActor != null)
            {
                foreach (var item in exsistMoviesActor)
                {
                    actorMovieRepository.Delete(item);
                    actorMovieRepository.Commit();
                }
            }
            if (deletedActor != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", deletedActor.ProfilePicture);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                actorRepository.Delete(deletedActor);
                actorRepository.Commit();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("NotFoundPage", "Home");

        }
    }
}
