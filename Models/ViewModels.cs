using System.ComponentModel.DataAnnotations;

namespace VoteHub.Models
{
    public class VoterRegisterViewModel
    {
        [Required] public string FullName { get; set; } = string.Empty;
        [Required] public string PhoneNumber { get; set; } = string.Empty;
        [Required] public string EpicNumber { get; set; } = string.Empty;
        [Required] public string Constituency { get; set; } = string.Empty;
        [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    }

    public class VoterLoginViewModel
    {
        [Required] public string EpicNumber { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }

    public class AdminRegisterViewModel
    {
        [Required] public string FullName { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string PhoneNumber { get; set; } = string.Empty;
        [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    }

    public class AdminLoginViewModel
    {
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }

    public class AddCandidateViewModel
    {
        [Required] public string FullName { get; set; } = string.Empty;
        [Required] public string Party { get; set; } = string.Empty;
        [Required] public string Constituency { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }

    public class AdminDashboardViewModel
    {
        public int CandidateCount { get; set; }
        public int VotesCast { get; set; }
        public string ResultStatus { get; set; } = "Pending";
        public List<Candidate> Candidates { get; set; } = new();
        public List<Voter> VotersWhoVoted { get; set; } = new();
        public AddCandidateViewModel NewCandidate { get; set; } = new();
    }

    public class VotingViewModel
    {
        public List<Candidate> Candidates { get; set; } = new();
        public string VoterConstituency { get; set; } = string.Empty;
    }

    public class ResultsViewModel
    {
        public bool IsDeclared { get; set; }
        public Candidate? Winner { get; set; }
        public List<Candidate> AllCandidates { get; set; } = new();
        public int TotalVotes { get; set; }
    }
}
