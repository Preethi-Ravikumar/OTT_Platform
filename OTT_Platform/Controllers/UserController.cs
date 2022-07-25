using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OTT_Platform.Models;

namespace OTT_Platform.Controllers
{

    [ApiController]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly OTTPlatformDbContext _context;

        public UserController(IConfiguration configuration, OTTPlatformDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("ViewMoviesList"), Authorize(Roles = "User")]
        public ActionResult GetMovies()
        {
            var movie = _context.Movies;

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
        [Route("addToWatchlist"), Authorize(Roles = "User")]
        public string AddToWatchlist([FromBody] int movieId)
        {
            try
            {
                var TokenVariables = HttpContext.User;
                var id = 0;
                if (TokenVariables?.Claims != null)
                {
                    foreach (var claim in TokenVariables.Claims)
                    {
                        id = Int16.Parse(claim.Value);
                        break;
                    }
                }
                var movie = _context.Movies.Where(e => e.MovieId == movieId).SingleOrDefault();
                var user = _context.Profiles.Where(u => u.UserId == id).SingleOrDefault();

                user.WatchList = user.WatchList + "," + movie.MovieName;
                _context.SaveChanges();
                return movie.MovieName + "Added to watchlist";
            }
            catch (Exception ex)
            {
                return "Error Occured " + ex;
            }
        }

        [HttpDelete]
        [Route("removefromWatchlist/{id}"), Authorize(Roles = "User")]
        public string RemoveMovie([FromBody] int movieId)
        {
            try
            {
                var movie = _context.Movies.Where(e => e.MovieId == movieId).SingleOrDefault();
                var TokenVariables = HttpContext.User;
                var id = 0;
                var count = 0;
                if (TokenVariables?.Claims != null)
                {
                    foreach (var claim in TokenVariables.Claims)
                    {
                        id = Int16.Parse(claim.Value);
                        break;
                    }
                }
                var user = _context.Profiles.Where(u => u.UserId == id).SingleOrDefault();
                string[] wlist = user.WatchList.Split(',');
                foreach (var w in wlist)
                {
                    if (movie.MovieName.Equals(w))
                    {
                        break;
                    }
                    else
                    {
                        count += 1;
                    }
                }
                var append = "";
                foreach (var w in wlist)
                {
                    if (w == wlist[count])
                    {
                        continue;
                    }
                    else
                    {
                        append += w + ",";
                    }
                }
                user.WatchList = append;
                _context.SaveChanges();

                return "Movie with Id= " + movieId + " ,MovieName= "+ movie.MovieName+" is removed from watchlist";
            }
            catch (Exception ex)
            {
                return "Exception occurred: " + ex;
            }
        }

        [HttpGet]
        [Route("watchlist"), Authorize(Roles = "User")]
        public ActionResult WatchList()
        {
            var TokenVariables = HttpContext.User;
            var id = 0;
            if (TokenVariables?.Claims != null)
            {
                foreach (var claim in TokenVariables.Claims)
                {
                    id = Int16.Parse(claim.Value);
                    break;
                }
            }

            var user = _context.Profiles.Where(u => u.UserId == id).SingleOrDefault();

            if (user != null)
            {
                string[] warray = user.WatchList.Split(',');
                List<string> wlist=new List<string>();
                foreach (string w in warray)
                {
                    if(w!="")
                    wlist.Add(w);
                }
                return Ok(wlist);
            }
            else
                return BadRequest("No such users");

        }
    }
}
