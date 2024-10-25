using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ontrack.Areas.Identity.Data;
using Ontrack.Data;
using Ontrack.Models;

namespace Ontrack.Controllers
{
    public class ExaminationsController : Controller
    {
		private readonly SchoolContext _context;
		private readonly UserManager<OntrackUser> _userManager;

		// Single constructor that accepts both dependencies
		public ExaminationsController(SchoolContext context, UserManager<OntrackUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET: Examinations
		public async Task<IActionResult> Index()
        {
            var examinations = await _context.Examinations
                .Include(e => e.Class)
                .Include(e => e.Subject)
                .ToListAsync();
            return View(examinations);
        }

        // GET: Examinations/Create
        public IActionResult Create()
        {
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName");
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "Name");
            return View();
        }
		[HttpGet]
		public async Task<IActionResult> ChooseExamAction()
		{
			var user = await _userManager.GetUserAsync(User);
			var isTeacherOrAdmin = await _userManager.IsInRoleAsync(user, "Teacher") || await _userManager.IsInRoleAsync(user, "Admin");
			if (isTeacherOrAdmin)
			{
				// Show options for teachers or admins
				return View("ChooseExamAction");
			}
			else
			{
				// Redirect parents or other users to view-only exam results
				return RedirectToAction("ViewExams", "Examinations");
			}
		}

		// POST: Examinations/Create
		[HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([Bind("ExaminationID,ExamName,Date,Score,ClassID,SubjectID")] Examination examination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(examination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", examination.ClassID);
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "Name", examination.SubjectID);
            return View(examination);
        }

        // Additional CRUD actions (Edit, Details, Delete) would go here...

        // Example: GET: Examinations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Examinations == null)
            {
                return NotFound();
            }

            var examination = await _context.Examinations.FindAsync(id);
            if (examination == null)
            {
                return NotFound();
            }

            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", examination.ClassID);
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "Name", examination.SubjectID);
            return View(examination);
        }

        // POST: Examinations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExaminationID,ExamName,Date,Score,ClassID,SubjectID")] Examination examination)
        {
            if (id != examination.ExaminationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExaminationExists(examination.ExaminationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", examination.ClassID);
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "Name", examination.SubjectID);
            return View(examination);
        }

        private bool ExaminationExists(int id)
        {
            return _context.Examinations.Any(e => e.ExaminationID == id);
        }
    }
}
