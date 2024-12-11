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
    public class ActorsController : ControllerBase
    {
        private readonly MyFirstAppDatabaseContext _context;

        public ActorsController(MyFirstAppDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Actors
        [HttpGet]
        public async Task<ActionResult<IList<ActorDTO>>> GetActors()
        {
            var actors = await _context.Actors
                .Include(a => a.Movie)
                .Select(a => new ActorDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Age = a.Age,
                    CharacterName = a.CharacterName,
                    MovieId = a.MovieId,
                    MovieTitle = a.Movie != null ? a.Movie.Title : "Unknown"
                })
                .ToListAsync();

            return Ok(actors);
        }

        // GET: api/Movies/{id}/actors
        [HttpGet("{id}/actors")]
        public async Task<ActionResult<IList<ActorDTO>>> GetActorsForMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var actors = movie.Actors.Select(a => new ActorDTO
            {
                Id = a.Id,
                Name = a.Name,
                Age = a.Age,
                CharacterName = a.CharacterName,
                MovieId = id,
                MovieTitle = movie.Title
            }).ToList();

            return Ok(actors);
        }



        // POST: api/Actors
        [HttpPost]
        public async Task<ActionResult<Actor>> PostActor(Actor actor)
        {
            // Check if the MovieId exists in the database
            var movie = await _context.Movies.FindAsync(actor.MovieId);
            if (movie == null)
            {
                return BadRequest(new { error = "The provided MovieId does not exist." });
            }

            // Associate the Movie with the Actor
            actor.Movie = movie;

            _context.Actors.Add(actor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ActorExists(actor.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
        }


        // DELETE: api/Actors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}
