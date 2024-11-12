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

        public async Task<IActionResult> LandingPage()
        {
            // Get the currently logged-in teacher
            var teacherEmail = User.Identity.Name;

            var teacher = await _ontrackContext.Teachers
                .Include(t => t.Classes)                          // Include Classes for the teacher
                    .ThenInclude(c => c.Students)                  // Include Students for each class
                .FirstOrDefaultAsync(t => t.Email == teacherEmail);

            // Check if the teacher is found
            if (teacher == null)
            {
                return NotFound();
            }

            // Now, separately fetch Attendances and StudentExamsResults for the students
            foreach (var classItem in teacher.Classes)
            {
                foreach (var student in classItem.Students)
                {
                    // Fetch Attendance for the student
                    student.Attendances = await _schoolContext.Attendance
                        .Where(a => a.StudentID == student.StudentID)
                        .ToListAsync();

                    // Fetch StudentExamsResults for the student
                    student.StudentExamsResult = await _schoolContext.StudentExamsResult
                        .Where(se => se.StudentID == student.StudentID)
                        .ToListAsync();
                }
            }

            // Return the teacher's classes to the view
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
