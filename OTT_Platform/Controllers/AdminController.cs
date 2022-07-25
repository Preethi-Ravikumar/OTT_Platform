using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OTT_Platform.Models;
using OTT_Platform.Producer;

namespace OTT_Platform.Controllers
{
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly OTTPlatformDbContext _context;

        public AdminController(IConfiguration configuration, OTTPlatformDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("UsersList"), Authorize(Roles = "Admin")]
        public ActionResult GetUsers()
        {
            var user = _context.Profiles.Where(x => x.Role == "User");

            if (user != null)
            {
                var users = user.Select(u => new
                {
                    Id = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                });
                return Ok(users);
            }
            else
            {
                return BadRequest("No Users");
            }
        }
        [HttpGet]
        [Route("moviesList"), Authorize(Roles = "Admin")]
        public ActionResult GetMovies()
        {
            var movie = _context.Movies;  //.Where(x => x.MovieId != null)

            if (movie != null)
            {
                var movies = movie.Select(m => new
                {
                    Id = m.MovieId,
                    MovieName = m.MovieName,
                    Description = m.Description,
                    Duration = m.Duration,
                    MovieCategory = m.MovieCategory,
                    Language = m.Language,
                    Actors = m.Actors
                });
                return Ok(movies);
            }
            else
            {
                return BadRequest("No Movie");
            }
        }
        [HttpPost]
        [Route("addMovie"), Authorize(Roles = "Admin")]
        public string AddMovie([FromBody] Movie movie)
        {
            try
            {
                var checkMovie = _context.Movies.SingleOrDefault(x => x.MovieName == movie.MovieName);
                if (checkMovie == null)
                {
                    _context.Movies.Add(movie);
                    _context.SaveChanges();
                    return "Movie Added Successfully";
                }
                else
                {
                    return "Movie already exists !";
                }
            }
            catch (Exception ex)
            {
                return "Exception! + " + ex;
            }
        }

        [HttpPut]
        [Route("updateMovie/{id}"), Authorize(Roles = "Admin")]
        public string UpdateMovie([FromBody] Movie movie,int? id)
        {
            try
            {
                var update = _context.Movies.Where(x => x.MovieId == id).SingleOrDefault();
                update.MovieName = movie.MovieName;
                update.Description = movie.Description;
                update.Duration = movie.Duration;
                update.MovieCategory = movie.MovieCategory;
                update.Language = movie.Language;
                update.Actors = movie.Actors;
                Send.Producer(update.MovieName);
                _context.SaveChanges();
                return "Movie : " + movie.MovieName + " is Updated";
            }
            catch (Exception ex)
            {
                return "Error Occured " + ex;
            }

        }

        [HttpDelete]
        [Route("deleteMovie/{id}"), Authorize(Roles = "Admin")]
        public string DeleteMovie(int? id)
        {
            try
            {
                var movie = _context.Movies.Where(e => e.MovieId == id).SingleOrDefault();
                _context.Movies.Remove(movie);
                _context.SaveChanges();

                return "Course with Id=" + id + " is deleted successfully";
            }
            catch (Exception ex)
            {
                return "Exception occurred: " + ex;
            }
        }


    }
}
