using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ontrack.Data;
using Ontrack.Models;
using Ontrack.ViewModels;

namespace Ontrack.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly SchoolContext _context;

        public AttendancesController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Attendances
        public IActionResult Index()
        {
            var parents = _context.Parents.ToList();
            var students = _context.Students
                .Select(s => new StudentPaymentViewModel
                {
                    StudentID = s.StudentID,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    ClassName = s.Class.ClassName,
                    ParentID = s.ParentID,
                    HasPaid = false // or the appropriate logic for payment status
                }).ToList();

            var viewModel = new SelectedParentViewModel
            {
                Parents = parents,
                Students = students
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult MarkAttendance(List<int> attendanceList, DateTime attendanceDate)
        {
            foreach (var studentId in attendanceList)
            {
                var attendance = new Attendance
                {
                    StudentID = studentId,
                    Date = attendanceDate, // Use the selected date
                    IsPresent = true // Assuming checked means present
                };
                _context.Attendance.Add(attendance);
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult ViewAttendance(DateTime? attendanceDate)
        {
            // Default to today's date if none is provided
            if (attendanceDate == null)
            {
                attendanceDate = DateTime.Today;
            }

            var attendanceRecords = _context.Attendance
                .Where(a => a.Date.Date == attendanceDate.Value.Date)
                .Select(a => new AttendanceViewModel
                {
                    StudentName = a.Student.FirstName + " " + a.Student.LastName,
                    ClassName = a.Student.Class.ClassName,
                    IsPresent = a.IsPresent,
                    Date = a.Date
                })
                .ToList();

            return View(attendanceRecords);
        }

        [HttpPost]
        public IActionResult ProcessAttendance(Dictionary<int, bool> attendance)
        {
            foreach (var entry in attendance)
            {
                int studentId = entry.Key;
                bool isPresent = entry.Value;

              
            }

            return RedirectToAction("Index");
        }

        // GET: Attendances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendance
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AttendanceID == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendances/Create
        public IActionResult Create()
        {
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "FirstName");
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttendanceID,StudentID,Date,IsPresent")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "FirstName", attendance.StudentID);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendance.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "FirstName", attendance.StudentID);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttendanceID,StudentID,Date,IsPresent")] Attendance attendance)
        {
            if (id != attendance.AttendanceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.AttendanceID))
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
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "FirstName", attendance.StudentID);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendance
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AttendanceID == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendance.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendance.Remove(attendance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendance.Any(e => e.AttendanceID == id);
        }
    }
}
