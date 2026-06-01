namespace VoteHub.Models
{
    public class ElectionResult
    {
        public int Id { get; set; }
        public bool IsDeclared { get; set; } = false;
        public int? WinnerCandidateId { get; set; }
        public Candidate? Winner { get; set; }
        public DateTime? DeclaredAt { get; set; }
    }
}
