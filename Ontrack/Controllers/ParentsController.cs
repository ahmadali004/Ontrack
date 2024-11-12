using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Ontrack.Areas.Identity.Data;
using Ontrack.Data;
using Ontrack.Models;
using Ontrack.ViewModels;

namespace Ontrack.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ParentsController : Controller
    {
        private readonly SchoolContext _context;
        private readonly UserManager<OntrackUser> _userManager;

        public ParentsController(UserManager<OntrackUser> userManager, SchoolContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        // GET: Parents/StudentDetails/{studentId}
        // GET: Parents/StudentDetails
        //[HttpGet("Parents/StudentDetails")]
        //public async Task<IActionResult> StudentDetails()
        //{

        //	// Get the user ID of the logged-in parent
        //	var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //	// Get the parent associated with the logged-in user
        //	var parent = await _context.Parents
        //		.Include(p => p.Students)
        //		.ThenInclude(s => s.Class)
        //		.FirstOrDefaultAsync(p => p.UserId == userId);

        //	if (parent == null)
        //	{
        //		return NotFound("Parent not found.");
        //	}

        //	var viewModel = new StudentDetailsViewModel
        //	{
        //		ParentFullName = $"{parent.FirstName} {parent.LastName}",
        //		Students = parent.Students.Select(s => new StudentDetailViewModel
        //		{
        //			StudentID = s.StudentID,
        //			FullName = $"{s.FirstName} {s.LastName}",
        //			ClassName = s.Class?.ClassName,
        //			AttendanceRecords = _context.Attendance
        //				.Where(a => a.StudentID == s.StudentID && a.Date.Month == DateTime.Now.Month)
        //				.ToList(),
        //			Payments = _context.Payments
        //				.Where(p => p.StudentID == s.StudentID)
        //				.ToList(),
        //			//ExamResults = _context.StudentExamsResult
        //			//	.Include(er => er.Examination)
        //			//	.Where(er => er.StudentID == s.StudentID)
        //			//	.ToList()
        //		}).ToList()
        //	};

        //	return View(viewModel);
        //}







        // In the Controller
        public async Task<IActionResult> StudentDetails(int? selectedMonth, int? selectedYear)
        {
            var userId = _userManager.GetUserId(User);
            var parent = await _context.Parents
                .Include(p => p.Students)
                    .ThenInclude(s => s.Attendances)
                .Include(p => p.Students)
                    .ThenInclude(s => s.Payments)
                .Include(p => p.Students)
                    .ThenInclude(s => s.StudentExamsResult)
                        .ThenInclude(er => er.Examination)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (parent == null || !parent.Students.Any())
            {
                return NotFound("No students found for this parent.");
            }

            var studentDetails = parent.Students.Select(s => new StudentDetailViewModel
            {
                StudentID = s.StudentID,
                FullName = s.FullName ?? "Unknown",
                ClassName = s.Class?.ClassName ?? "No class assigned",
                AttendanceRecords = s.Attendances?
                    .Where(a => (!selectedMonth.HasValue || a.Date.Month == selectedMonth.Value) &&
                                (!selectedYear.HasValue || a.Date.Year == selectedYear.Value))
                    .ToList() ?? new List<Attendance>(),
                Payments = s.Payments?
                    .Where(p => (!selectedMonth.HasValue || p.PaymentDate.Month == selectedMonth.Value) &&
                                (!selectedYear.HasValue || p.PaymentDate.Year == selectedYear.Value))
                    .ToList() ?? new List<Payment>(),
                ExamResults = s.StudentExamsResult?
                    .Where(er => (!selectedMonth.HasValue || er.Examination?.Date.Month == selectedMonth.Value) &&
                                 (!selectedYear.HasValue || er.Examination?.Date.Year == selectedYear.Value))
                    .Select(er => new StudentExamResultViewModel
                    {
                        ExamName = er.Examination?.ExamName ?? "N/A",
                        ExamDate = er.Examination?.Date.ToString("yyyy-MM-dd") ?? "N/A",
                        Score = (int?)er.Score ?? 0
                    })
                    .ToList() ?? new List<StudentExamResultViewModel>()
            }).ToList();

            var viewModel = new StudentDetailsViewModel
            {
                ParentFullName = parent.FullName ?? "Unknown Parent",
                Students = studentDetails,
                MonthOptions = GetMonthOptions(),
                SelectedMonth = selectedMonth ?? DateTime.Now.Month,
              
            };

            return View(viewModel);
        }


















        public async Task<IActionResult> GetStudentExamView(int studentId)
        {
            var exams = await _context.Examinations.ToListAsync(); // Fetch all exams

            var viewModel = new StudentExamViewModel
            {
                StudentID = studentId,
                StudentName = (await _context.Students.FindAsync(studentId))?.FullName,
                Exams = exams.Select(e => new SelectListItem
                {
                    Value = e.ExaminationID.ToString(),
                    Text = e.ExamName
                }).ToList()
            };

            return View(viewModel); // Return view for managing student exams
        }

        [HttpPost]
        public async Task<IActionResult> SaveExamScore(StudentExamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("GetStudentExamView", model); // Return to view if model is invalid
            }

            var studentExamResult = new StudentExamsResult
            {
                StudentID = model.StudentID,
                ExaminationID = model.SelectedExamID,
                Score = model.Score
            };

            _context.StudentExamsResult.Add(studentExamResult);
            await _context.SaveChangesAsync();

            return RedirectToAction("StudentDetails"); // Redirect to student details after saving
        }

        private List<SelectListItem> GetMonthOptions()
        {
            var months = new List<SelectListItem>
    {
        new SelectListItem { Text = "January", Value = "1" },
        new SelectListItem { Text = "February", Value = "2" },
        new SelectListItem { Text = "March", Value = "3" },
        new SelectListItem { Text = "April", Value = "4" },
        new SelectListItem { Text = "May", Value = "5" },
        new SelectListItem { Text = "June", Value = "6" },
        new SelectListItem { Text = "July", Value = "7" },
        new SelectListItem { Text = "August", Value = "8" },
        new SelectListItem { Text = "September", Value = "9" },
        new SelectListItem { Text = "October", Value = "10" },
        new SelectListItem { Text = "November", Value = "11" },
        new SelectListItem { Text = "December", Value = "12" }
    };

            return months;
        }

















        public async Task<IActionResult> Index()
		{
			if (!User.Identity.IsAuthenticated || !User.IsInRole("Parent"))
			{
				return RedirectToAction("Login", "Account");
			}

			// Get the user ID of the logged-in parent
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			// Get the parent associated with the logged-in user
			var parent = await _context.Parents
				.Include(p => p.Students)
				.FirstOrDefaultAsync(p => p.UserId == userId);

			if (parent == null)
			{
				return NotFound("Parent not found.");
			}

			var viewModel = new SelectedParentViewModel
			{
				Parent = parent,
				Students = parent.Students.Select(s => new StudentPaymentViewModel
				{
					StudentID = s.StudentID,
					FirstName = s.FirstName,
					LastName = s.LastName,
					ClassName = s.Class?.ClassName ?? "N/A",
					PaymentAmount = CalculatePaymentAmountForStudent(s)
				}).ToList()
			};

			return View(viewModel);
		}


		// Calculate payment amount based on sibling count
		private decimal CalculatePaymentAmountForStudent(Student student)
        {
            var siblingsCount = _context.Students.Count(s => s.ParentID == student.ParentID);

            switch (siblingsCount)
            {
                case 1:
                    return 1700m;
                case 2:
                case 3:
                    return 1500m;
                case 4:
                    return 1300m;
                default:
                    return 0m;
            }
        }

        // LoadChildren - To fetch details for a specific parent
        [HttpPost]
        [Route("LoadChildren")]
        public async Task<IActionResult> LoadChildren(int? ParentID)
        {
            if (ParentID == null)
            {
                return NotFound();
            }

            var selectedParent = await _context.Parents
                                               .Include(p => p.Students)
                                               .ThenInclude(s => s.Class)
                                               .FirstOrDefaultAsync(p => p.ParentID == ParentID);

            if (selectedParent == null)
            {
                return NotFound();
            }

            var viewModel = new SelectedParentViewModel
            {
                Parent = selectedParent,
                Students = selectedParent.Students.Select(s => new StudentPaymentViewModel
                {
                    StudentID = s.StudentID,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    ClassName = s.Class?.ClassName ?? "N/A",
                    PaymentAmount = CalculatePaymentAmountForStudent(s)
                }).ToList(),
                Parents = await _context.Parents.ToListAsync()
            };

            return View("Index", viewModel); // Return SelectedParentViewModel
        }

        // Details - To show details for a specific parent
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents
                .Include(p => p.Students)
                .FirstOrDefaultAsync(m => m.ParentID == id);

            if (parent == null)
            {
                return NotFound();
            }

            // Here, return the Parent model directly to the Details view
            return View(parent);
        }


        // GET: Parents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParentID,FirstName,LastName,PhoneNumber,Email")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parent);
        }

        // GET: Parents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents.FindAsync(id);
            if (parent == null)
            {
                return NotFound();
            }
            return View(parent);
        }

        // POST: Parents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParentID,FirstName,LastName,PhoneNumber,Email")] Parent parent)
        {
            if (id != parent.ParentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParentExists(parent.ParentID))
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
            return View(parent);
        }


        // GET: Parents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents
                .FirstOrDefaultAsync(m => m.ParentID == id);
            if (parent == null)
            {
                return NotFound();
            }

            return View(parent);
        }


        // POST: Parents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent != null)
            {
                _context.Parents.Remove(parent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParentExists(int id)
        {
            return _context.Parents.Any(e => e.ParentID == id);
        }
    }
}
