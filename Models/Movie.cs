using System.Text.Json.Serialization;

public class Movie
{
    [JsonPropertyName("Title")]
    public string Title { get; set; }

    [JsonPropertyName("Year")]
    public string Year { get; set; }

    [JsonPropertyName("Genre")]
    public string Genre { get; set; }

    [JsonPropertyName("Plot")]
    public string Plot { get; set; }

    [JsonPropertyName("Poster")]
    public string Poster { get; set; }

    [JsonPropertyName("imdbID")]
    public string ImdbID { get; set; }

    [JsonPropertyName("imdbRating")]
    public string ImdbRating { get; set; }

    // computed price
    public double Price { get; set; }
}