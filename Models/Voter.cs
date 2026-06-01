namespace VoteHub.Models
{
    public class Voter
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EpicNumber { get; set; } = string.Empty;
        public string Constituency { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool HasVoted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
