using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ontrack.Data;
using Ontrack.Models;
using Ontrack.ViewModels;

namespace Ontrack.Controllers
{
    public class TeachersController : Controller
    {
         private readonly SchoolContext _schoolContext;
    private readonly OntrackContext _ontrackContext;

    public TeachersController(SchoolContext schoolContext, OntrackContext ontrackContext)
    {
        _schoolContext = schoolContext;
        _ontrackContext = ontrackContext;
    }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            return View(await _schoolContext.Teachers.ToListAsync());
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _schoolContext.Teachers
                .FirstOrDefaultAsync(m => m.TeacherID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherID,FirstName,LastName,PhoneNumber,Email,Salary")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _schoolContext.Add(teacher);
                await _schoolContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _schoolContext.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherID,FirstName,LastName,PhoneNumber,Email,Salary")] Teacher teacher)
        {
            if (id != teacher.TeacherID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _schoolContext.Update(teacher);
                    await _schoolContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.TeacherID))
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
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _schoolContext.Teachers
                .FirstOrDefaultAsync(m => m.TeacherID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }


        // GET: Teachers/LandingPage

        public async Task<IActionResult> LandingPage(string selectedMonth)
        {
            // Get the currently logged-in teacher
            var teacherEmail = User.Identity.Name;

            // Retrieve the teacher along with their classes, students, attendance, and exam results
            var teacher = await _ontrackContext.Teachers
                .Include(t => t.Classes)
                    .ThenInclude(c => c.Students)
                        .ThenInclude(s => s.Attendances)
                .Include(t => t.Classes)
                    .ThenInclude(c => c.Students)
                        .ThenInclude(s => s.StudentExamsResult)
                            .ThenInclude(se => se.Examination)
                .FirstOrDefaultAsync(t => t.Email == teacherEmail);

            // Check if the teacher is found
            if (teacher == null)
            {
                return NotFound();
            }

            // Parse the selected month for filtering
            DateTime monthDate;
            bool isValidMonth = DateTime.TryParseExact(selectedMonth, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out monthDate);

            // Filter attendance and exam results for the selected month if valid
            if (isValidMonth)
            {
                foreach (var classItem in teacher.Classes)
                {
                    foreach (var student in classItem.Students)
                    {
                        // Filter attendances by the selected month
                        student.Attendances = student.Attendances
                            .Where(a => a.Date.Year == monthDate.Year && a.Date.Month == monthDate.Month)
                            .ToList();

                        // Filter exam results by the selected month
                        student.StudentExamsResult = student.StudentExamsResult
                            .Where(se => se.Examination.Date.Year == monthDate.Year && se.Examination.Date.Month == monthDate.Month)
                            .ToList();
                    }
                }
            }

            // Pass the teacher's classes and selected month to the view
            ViewData["SelectedMonth"] = selectedMonth;
            return View(teacher.Classes);
        }



















        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _schoolContext.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _schoolContext.Teachers.Remove(teacher);
            }

            await _schoolContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _schoolContext.Teachers.Any(e => e.TeacherID == id);
        }
    }
}
