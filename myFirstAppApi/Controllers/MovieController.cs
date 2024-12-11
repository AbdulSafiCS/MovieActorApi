using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dataModel;

namespace myFirstAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MyFirstAppDatabaseContext _context;

        public MoviesController(MyFirstAppDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.Actors) // Include related actors
                .Select(m => new MovieDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate,
                    Genre = m.Genre,
                    Rating = m.Rating,
                    Actors = m.Actors.Select(a => new ActorDTO
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Age = a.Age,
                        CharacterName = a.CharacterName
                    }).ToList()
                })
                .ToListAsync();

            return movies;
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Actors) // Include related actors
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            // Map to DTO
            var movieDTO = new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Genre = movie.Genre,
                Rating = movie.Rating,
                Actors = movie.Actors.Select(a => new ActorDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Age = a.Age,
                    CharacterName = a.CharacterName
                }).ToList()
            };

            return movieDTO;
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieDTO movieDTO)
        {
            if (id != movieDTO.Id)
            {
                return BadRequest();
            }

            // Map DTO back to Entity
            var movie = await _context.Movies.Include(m => m.Actors).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            movie.Title = movieDTO.Title;
            movie.ReleaseDate = movieDTO.ReleaseDate;
            movie.Genre = movieDTO.Genre;
            movie.Rating = movieDTO.Rating;



            // Update actors
            movie.Actors.Clear();
            foreach (var actorDTO in movieDTO.Actors)
            {
                movie.Actors.Add(new Actor
                {
                    Id = actorDTO.Id,
                    Name = actorDTO.Name,
                    Age = actorDTO.Age,
                    CharacterName = actorDTO.CharacterName,
                    MovieId = movie.Id
                });
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // GET: api/Movies/{id}/Actors
        // GET: api/Movies/{id}/Actors
        [HttpGet("{id}/actors")]
        public async Task<ActionResult<IEnumerable<ActorDTO>>> GetActorsForMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var actorDTOs = movie.Actors.Select(a => new ActorDTO
            {
                Id = a.Id,
                Name = a.Name,
                Age = a.Age,
                CharacterName = a.CharacterName,
                MovieId = movie.Id,
                MovieTitle = movie.Title // Include the movie title
            }).ToList();

            return Ok(actorDTOs);
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<MovieDTO>> PostMovie(MovieDTO movieDTO)
        {
            // Map DTO to Entity
            var movie = new Movie
            {
                Title = movieDTO.Title,
                ReleaseDate = movieDTO.ReleaseDate,
                Genre = movieDTO.Genre,
                Rating = movieDTO.Rating,
                Actors = movieDTO.Actors.Select(a => new Actor
                {
                    Name = a.Name,
                    Age = a.Age,
                    CharacterName = a.CharacterName
                }).ToList()
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            // Map back to DTO for the response
            movieDTO.Id = movie.Id;

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movieDTO);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.Include(m => m.Actors).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }

    // DTO for Movie
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Rating { get; set; }
        public List<ActorDTO> Actors { get; set; } = new();
    }

    // DTO for Actor
    public class ActorDTO
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public int Age { get; set; } 
        public string CharacterName { get; set; } 
        public int MovieId { get; set; } 
        public string MovieTitle { get; set; }
    }
}
