namespace APISteam.Models
{
    public class SteamGame
    {
        public int AppId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PlayerCount { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Genres { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public string HeaderImage { get; set; } = string.Empty;
        public int UserReviews { get; set; }
        public string Website { get; set; } = string.Empty;
    }
}