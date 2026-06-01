using Microsoft.AspNetCore.Mvc;
using VoteHub.Data;
using VoteHub.Models;
using BCrypt.Net;

namespace VoteHub.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        private bool IsAdminLoggedIn() => HttpContext.Session.GetString("AdminId") != null;

        // GET: /Admin/Register
        public IActionResult Register()
        {
            if (IsAdminLoggedIn()) return RedirectToAction("Dashboard");
            return View();
        }

        // POST: /Admin/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(AdminRegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (_db.Admins.Any(a => a.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already registered.");
                return View(model);
            }

            var admin = new Admin
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _db.Admins.Add(admin);
            _db.SaveChanges();

            TempData["Success"] = "Admin account created! Please login.";
            return RedirectToAction("Login");
        }

        // GET: /Admin/Login
        public IActionResult Login()
        {
            if (IsAdminLoggedIn()) return RedirectToAction("Dashboard");
            return View();
        }

        // POST: /Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var admin = _db.Admins.FirstOrDefault(a => a.Email == model.Email);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(model.Password, admin.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            HttpContext.Session.SetString("AdminId", admin.Id.ToString());
            HttpContext.Session.SetString("AdminName", admin.FullName);
            return RedirectToAction("Dashboard");
        }

        // GET: /Admin/Dashboard
        public IActionResult Dashboard()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var result = _db.ElectionResults.FirstOrDefault();
            var model = new AdminDashboardViewModel
            {
                CandidateCount = _db.Candidates.Count(),
                VotesCast = _db.Voters.Count(v => v.HasVoted),
                ResultStatus = result?.IsDeclared == true ? "Declared" : "Pending",
                Candidates = _db.Candidates.OrderByDescending(c => c.VoteCount).ToList(),
                VotersWhoVoted = _db.Voters.Where(v => v.HasVoted).ToList()
            };

            return View(model);
        }

        // POST: /Admin/AddCandidate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCandidate(AddCandidateViewModel model)
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all fields correctly.";
                return RedirectToAction("Dashboard");
            }

            var candidate = new Candidate
            {
                FullName = model.FullName,
                Party = model.Party,
                Constituency = model.Constituency,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _db.Candidates.Add(candidate);
            _db.SaveChanges();

            TempData["Success"] = "Candidate added successfully!";
            return RedirectToAction("Dashboard");
        }

        // POST: /Admin/DeclareResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeclareResult()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var winner = _db.Candidates.OrderByDescending(c => c.VoteCount).FirstOrDefault();
            if (winner == null)
            {
                TempData["Error"] = "No candidates found.";
                return RedirectToAction("Dashboard");
            }

            var result = _db.ElectionResults.FirstOrDefault();
            if (result == null)
            {
                result = new ElectionResult();
                _db.ElectionResults.Add(result);
            }

            result.IsDeclared = true;
            result.WinnerCandidateId = winner.Id;
            result.DeclaredAt = DateTime.Now;
            _db.SaveChanges();

            TempData["Success"] = $"Election result declared! Winner: {winner.FullName} ({winner.Party})";
            return RedirectToAction("Dashboard");
        }

        // GET: /Admin/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
