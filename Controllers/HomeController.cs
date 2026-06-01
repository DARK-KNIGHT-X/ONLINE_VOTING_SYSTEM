using Microsoft.AspNetCore.Mvc;
using VoteHub.Data;
using VoteHub.Models;

namespace VoteHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            ViewBag.TotalVotes = _db.Voters.Count(v => v.HasVoted);
            ViewBag.TotalCandidates = _db.Candidates.Count();
            return View();
        }

        public IActionResult Results()
        {
            var result = _db.ElectionResults.FirstOrDefault();
            var model = new ResultsViewModel
            {
                IsDeclared = result?.IsDeclared ?? false,
                AllCandidates = _db.Candidates.OrderByDescending(c => c.VoteCount).ToList(),
                TotalVotes = _db.Voters.Count(v => v.HasVoted)
            };

            if (result?.IsDeclared == true && result.WinnerCandidateId != null)
            {
                model.Winner = _db.Candidates.Find(result.WinnerCandidateId);
            }

            return View(model);
        }
    }
}
