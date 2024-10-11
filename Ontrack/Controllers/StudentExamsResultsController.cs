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
    public class StudentExamsResultsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentExamsResultsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: StudentExamsResults
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.StudentExamsResults.Include(s => s.Examination).Include(s => s.Student);
            return View(await schoolContext.ToListAsync());
        }

        // GET: StudentExamsResults/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentExamsResult = await _context.StudentExamsResults
                .Include(s => s.Examination)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.StudentExamResultID == id);
            if (studentExamsResult == null)
            {
                return NotFound();
            }

            return View(studentExamsResult);
        }

        // GET: StudentExamsResults/Create
        public IActionResult Create()
        {
            ViewData["ExaminationID"] = new SelectList(_context.Examinations, "ExaminationID", "ExaminationID");
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "FirstName");
            return View();
        }

        // POST: StudentExamsResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentExamResultID,Score,StudentID,ExaminationID")] StudentExamsResult studentExamsResult)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentExamsResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExaminationID"] = new SelectList(_context.Examinations, "ExaminationID", "ExaminationID", studentExamsResult.ExaminationID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "FirstName", studentExamsResult.StudentID);
            return View(studentExamsResult);
        }

        // GET: StudentExamsResults/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentExamsResult = await _context.StudentExamsResults.FindAsync(id);
            if (studentExamsResult == null)
            {
                return NotFound();
            }
            ViewData["ExaminationID"] = new SelectList(_context.Examinations, "ExaminationID", "ExaminationID", studentExamsResult.ExaminationID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "FirstName", studentExamsResult.StudentID);
            return View(studentExamsResult);
        }

        // POST: StudentExamsResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentExamResultID,Score,StudentID,ExaminationID")] StudentExamsResult studentExamsResult)
        {
            if (id != studentExamsResult.StudentExamResultID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentExamsResult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExamsResultExists(studentExamsResult.StudentExamResultID))
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
            ViewData["ExaminationID"] = new SelectList(_context.Examinations, "ExaminationID", "ExaminationID", studentExamsResult.ExaminationID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "FirstName", studentExamsResult.StudentID);
            return View(studentExamsResult);
        }

        // GET: StudentExamsResults/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentExamsResult = await _context.StudentExamsResults
                .Include(s => s.Examination)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.StudentExamResultID == id);
            if (studentExamsResult == null)
            {
                return NotFound();
            }

            return View(studentExamsResult);
        }

        // POST: StudentExamsResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentExamsResult = await _context.StudentExamsResults.FindAsync(id);
            if (studentExamsResult != null)
            {
                _context.StudentExamsResults.Remove(studentExamsResult);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExamsResultExists(int id)
        {
            return _context.StudentExamsResults.Any(e => e.StudentExamResultID == id);
        }
    }
}
