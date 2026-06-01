using Microsoft.AspNetCore.Mvc;
using VoteHub.Data;
using VoteHub.Models;
using BCrypt.Net;

namespace VoteHub.Controllers
{
    public class VoterController : Controller
    {
        private readonly AppDbContext _db;

        public VoterController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Voter/Register
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("VoterId") != null)
                return RedirectToAction("Vote");
            return View();
        }

        // POST: /Voter/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(VoterRegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (_db.Voters.Any(v => v.EpicNumber == model.EpicNumber))
            {
                ModelState.AddModelError("EpicNumber", "This EPIC number is already registered.");
                return View(model);
            }

            var voter = new Voter
            {
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                EpicNumber = model.EpicNumber,
                Constituency = model.Constituency,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _db.Voters.Add(voter);
            _db.SaveChanges();

            TempData["Success"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        // GET: /Voter/Login
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("VoterId") != null)
                return RedirectToAction("Vote");
            return View();
        }

        // POST: /Voter/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(VoterLoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var voter = _db.Voters.FirstOrDefault(v => v.EpicNumber == model.EpicNumber);
            if (voter == null || !BCrypt.Net.BCrypt.Verify(model.Password, voter.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid EPIC number or password.");
                return View(model);
            }

            HttpContext.Session.SetString("VoterId", voter.Id.ToString());
            HttpContext.Session.SetString("VoterName", voter.FullName);
            return RedirectToAction("Vote");
        }

        // GET: /Voter/Vote
        public IActionResult Vote()
        {
            var voterId = HttpContext.Session.GetString("VoterId");
            if (voterId == null) return RedirectToAction("Login");

            var voter = _db.Voters.Find(int.Parse(voterId));
            if (voter == null) return RedirectToAction("Login");

            if (voter.HasVoted)
            {
                TempData["Info"] = "You have already cast your vote.";
                return RedirectToAction("Index", "Home");
            }

            var model = new VotingViewModel
            {
                Candidates = _db.Candidates.Where(c => c.Constituency == voter.Constituency).ToList(),
                VoterConstituency = voter.Constituency
            };

            return View(model);
        }

        // POST: /Voter/CastVote
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CastVote(int candidateId)
        {
            var voterId = HttpContext.Session.GetString("VoterId");
            if (voterId == null) return RedirectToAction("Login");

            var voter = _db.Voters.Find(int.Parse(voterId));
            if (voter == null || voter.HasVoted)
            {
                TempData["Error"] = "Vote already cast or invalid session.";
                return RedirectToAction("Index", "Home");
            }

            var candidate = _db.Candidates.Find(candidateId);
            if (candidate == null)
            {
                TempData["Error"] = "Invalid candidate.";
                return RedirectToAction("Vote");
            }

            candidate.VoteCount++;
            voter.HasVoted = true;
            _db.SaveChanges();

            TempData["Success"] = "Your vote has been cast successfully!";
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Voter/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
