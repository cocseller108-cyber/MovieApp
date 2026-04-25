using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private static List<Movie> watchlist = new List<Movie>();
    private static List<Movie> purchasedMovies = new List<Movie>();

    private readonly string apiKey = "d70c5038";

    private readonly List<string> popularTitles = new List<string>
    {
        "The Shawshank Redemption",
        "The Godfather",
        "The Dark Knight",
        "Inception",
        "Fight Club",
        "Pulp Fiction",
        "Forrest Gump",
        "The Matrix",
        "The Lord of the Rings: The Return of the King",
        "Interstellar"
    };

    public async Task<IActionResult> Index()
    {
        ViewBag.Watchlist = watchlist;
        ViewBag.Purchased = purchasedMovies;

        var topMovies = new List<Movie>();

        using (HttpClient client = new HttpClient())
        {
            foreach (var title in popularTitles)
            {
                var response = await client.GetStringAsync(
                    $"https://www.omdbapi.com/?t={title}&apikey={apiKey}"
                );

                var movie = JsonSerializer.Deserialize<Movie>(response);

                if (movie != null && movie.Title != null)
                {
                    double rating = 0;
                    double.TryParse(movie.ImdbRating, out rating);

                    movie.Price = rating * 50;

                    topMovies.Add(movie);
                }
            }
        }

        ViewBag.Top10 = topMovies
            .OrderByDescending(m => m.Price)
            .Take(10)
            .ToList();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Search(string title)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync(
                $"https://www.omdbapi.com/?t={title}&apikey={apiKey}"
            );

            var movie = JsonSerializer.Deserialize<Movie>(response);

            if (movie == null || movie.Title == null)
            {
                return View("Index");
            }

            double rating = 0;
            double.TryParse(movie.ImdbRating, out rating);

            movie.Price = rating * 50;

            ViewBag.Movie = movie;
            ViewBag.Watchlist = watchlist;
            ViewBag.Purchased = purchasedMovies;

            return View("Index");
        }
    }

    public IActionResult Add(string title, string year, string genre, string plot,
        string poster, string imdbID, double price)
    {
        if (!watchlist.Any(m => m.ImdbID == imdbID))
        {
            watchlist.Add(new Movie
            {
                Title = title,
                Year = year,
                Genre = genre,
                Plot = plot,
                Poster = poster,
                ImdbID = imdbID,
                Price = price
            });
        }

        return RedirectToAction("Index");
    }

    // 🛒 BUY SYSTEM (USER INPUT MONEY)
   [HttpPost]
public IActionResult BuyMovie(string imdbID, string title, string poster, double price, double userMoney)
{
    // 🔍 rebuild movie from form (NO lookup needed)
    var movie = new Movie
    {
        ImdbID = imdbID,
        Title = title,
        Poster = poster,
        Price = price
    };

    // ❌ already purchased check
    if (purchasedMovies.Any(m => m.ImdbID == imdbID))
    {
        TempData["Message"] = "You already own this movie!";
        return RedirectToAction("Index");
    }

    // ❌ insufficient money
    if (userMoney < price)
    {
        TempData["Message"] = $"Not enough money! You need ₱{price}";
        return RedirectToAction("Index");
    }

    // ✔ success
    purchasedMovies.Add(movie);

    double change = userMoney - price;

    TempData["Message"] =
        $"Purchase successful! Change: ₱{change}";

    return RedirectToAction("Index");
}
}