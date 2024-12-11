namespace myFirstAppApi.Data
{
    // Class for the movies.csv file
    public class MovieCSV
    {
        public string Title { get; set; } = null!; 
        public string ReleaseDate { get; set; } = null!; 
        public string Genre { get; set; } = null!;
        public decimal Rating { get; set; } 
    }
}