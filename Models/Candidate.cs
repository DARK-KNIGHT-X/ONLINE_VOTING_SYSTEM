namespace VoteHub.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public string Constituency { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int VoteCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
