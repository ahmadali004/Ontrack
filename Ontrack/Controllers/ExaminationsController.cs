using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ontrack.Data;
using Ontrack.Models;

namespace Ontrack.Controllers
{
    [Authorize(Roles = "Admin,Teacher,Parent")]
    public class ExaminationsController : Controller
    {
        private readonly SchoolContext _context;

        public ExaminationsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Examinations
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Examinations.Include(e => e.Class).Include(e => e.Subject);
            return View(await schoolContext.ToListAsync());
        }

        // GET: Examinations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examination = await _context.Examinations
                .Include(e => e.Class)
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(m => m.ExaminationID == id);
            if (examination == null)
            {
                return NotFound();
            }

            return View(examination);
        }

        // GET: Examinations/Create
        public IActionResult Create()
        {
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassID");
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID");
            return View();
        }

        // POST: Examinations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExaminationID,ExamName,Date,Score,ClassID,SubjectID")] Examination examination)
        {
            var classes = _context.Classes.ToList(); // Assuming _context is your DbContext
            ViewBag.ClassID = new SelectList(classes, "ClassID", "ClassName"); // Replace "ClassName" with the actual property name for display

            // Populate SubjectID dropdown
            var subjects = _context.Subjects.ToList(); // Assuming _context is your DbContext
            ViewBag.SubjectID = new SelectList(subjects, "SubjectID", "SubjectName"); // Replace "SubjectName" with the actual property name for display
            if (ModelState.IsValid)
            {
                _context.Add(examination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassID", examination.ClassID);
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID", examination.SubjectID);
            return View(examination);
        }

        // GET: Examinations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examination = await _context.Examinations.FindAsync(id);
            if (examination == null)
            {
                return NotFound();
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassID", examination.ClassID);
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID", examination.SubjectID);
            return View(examination);
        }

        // POST: Examinations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassID", examination.ClassID);
            ViewData["SubjectID"] = new SelectList(_context.Subject, "SubjectID", "SubjectID", examination.SubjectID);
            return View(examination);
        }

        // GET: Examinations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examination = await _context.Examinations
                .Include(e => e.Class)
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(m => m.ExaminationID == id);
            if (examination == null)
            {
                return NotFound();
            }

            return View(examination);
        }

        // POST: Examinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var examination = await _context.Examinations.FindAsync(id);
            if (examination != null)
            {
                _context.Examinations.Remove(examination);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExaminationExists(int id)
        {
            return _context.Examinations.Any(e => e.ExaminationID == id);
        }
    }
}
