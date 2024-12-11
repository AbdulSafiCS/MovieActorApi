namespace myFirstAppApi.DTO
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; } = null!;
        public decimal Rating { get; set; }
        public List<ActorDTO> Actors { get; set; } = new List<ActorDTO>();
    }
}