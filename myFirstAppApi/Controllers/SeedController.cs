using CsvHelper;
using CsvHelper.Configuration;
using dataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Globalization;

namespace myFirstAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly MyFirstAppDatabaseContext _context;
        private readonly IHostEnvironment _environment;
        private readonly string _moviesPath;
        private readonly string _actorsPath;

        public SeedController(MyFirstAppDatabaseContext context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _moviesPath = Path.Combine(environment.ContentRootPath, "Data/movies.csv");
            _actorsPath = Path.Combine(environment.ContentRootPath, "Data/actors.csv");
        }

        // POST: api/Seed/Movies
        [HttpPost("Movies")]
        public async Task<IActionResult> ImportMoviesAsync()
        {
            var moviesByTitle = _context.Movies
                .AsNoTracking()
                .ToDictionary(x => x.Title.Trim().ToLower(), x => x);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            using StreamReader reader = new(_moviesPath);
            using CsvReader csv = new(reader, config);

            var records = csv.GetRecords<MovieCSV>().ToList();
            foreach (var record in records)
            {
                var title = record.Title.Trim().ToLower();
                if (moviesByTitle.ContainsKey(title)) continue;

                var movie = new Movie
                {
                    Title = record.Title.Trim(),
                    ReleaseDate = DateTime.Parse(record.ReleaseDate),
                    Genre = record.Genre.Trim(),
                    Rating = record.Rating
                };

                await _context.Movies.AddAsync(movie);
                moviesByTitle.Add(title, movie);
            }

            await _context.SaveChangesAsync();
            return new JsonResult(moviesByTitle.Count);
        }

        // POST: api/Seed/Actors
        [HttpPost("Actors")]
        public async Task<IActionResult> ImportActorsAsync()
        {
            // Create a lookup dictionary for existing movies
            Dictionary<string, Movie> movies = await _context.Movies
                .ToDictionaryAsync(m => m.Title.Trim().ToLower(), m => m);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            int actorCount = 0;

            using StreamReader reader = new(_actorsPath);
            using CsvReader csv = new(reader, config);

            var records = csv.GetRecords<ActorCSV>();
            foreach (var record in records)
            {
                var movieTitle = record.MovieTitle.Trim().ToLower();
                if (!movies.TryGetValue(movieTitle, out var movie))
                {
                    Console.WriteLine($"Movie not found for actor: {record.Name}, MovieTitle: {record.MovieTitle}");
                    continue;
                }

                Actor actor = new()
                {
                    Name = record.Name.Trim(),
                    Age = record.Age,
                    CharacterName = record.CharacterName.Trim(),
                    MovieId = movie.Id
                };

                _context.Actors.Add(actor);
                actorCount++;
            }

            await _context.SaveChangesAsync();
            Console.WriteLine($"Actors Imported: {actorCount}");
            return new JsonResult(actorCount);
        }


        // Movie CSV Structure
        public class MovieCSV
        {
            public string Title { get; set; }
            public string ReleaseDate { get; set; }
            public string Genre { get; set; }
            public decimal Rating { get; set; }
        }

        // Actor CSV Structure
        public class ActorCSV
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string CharacterName { get; set; }
            public string MovieTitle { get; set; }
        }
    }
}
