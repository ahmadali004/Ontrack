using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ontrack.Data;
using Ontrack.Models;
using Ontrack.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ontrack.Controllers
{
    public class StudentExamsResultsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentExamsResultsController(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? selectedExamID)
        {
            var viewModel = new ClassExaminationViewModel
            {
                Exams = await _context.Examinations
                    .Select(e => new SelectListItem
                    {
                        Value = e.ExaminationID.ToString(),
                        Text = e.ExamName
                    }).ToListAsync(),
                SelectedExamID = selectedExamID ?? 0 // Default to 0 if null
            };

            if (selectedExamID.HasValue)
            {
                viewModel.Students = await _context.StudentExamsResult
                    .Include(r => r.Student) // Ensure you have Student property in StudentExamsResults
                    .Where(r => r.ExaminationID == selectedExamID.Value)
                    .Select(r => new StudentExamViewModel
                    {
                        StudentName = r.Student.FullName, // Assuming you have a FullName property
                        Score =(double)r.Score,
                        // Add other properties as needed
                    })
                    .ToListAsync();

                // Get exam details (optional, if you want to display exam information)
                var selectedExam = await _context.Examinations
                    .Include(e => e.Class)
                    .FirstOrDefaultAsync(e => e.ExaminationID == selectedExamID.Value);
                if (selectedExam != null)
                {
                    viewModel.ExamName = selectedExam.ExamName;
                    viewModel.ExamDate = selectedExam.Date; // Assuming you have a Date property
                    viewModel.ClassName = selectedExam.Class.ClassName; // Assuming you have Class navigation
                }
            }

            return View(viewModel);
        }



        // GET: StudentExamsResults/Create


        // Change the return type to Task<IActionResult> and mark it as async
        public async Task<IActionResult> Create()
        {
            var model = new ClassExaminationViewModel
            {
                ExamResult = new StudentExamsResult(),
                Classes = await _context.Classes.ToListAsync(), // Get actual Class entities
                Subjects = GetSubjects(),
                Students = await _context.Students
                    .Select(s => new StudentExamViewModel
                    {
                        StudentID = s.StudentID,
                        StudentName = s.FullName
                    }).ToListAsync(),
                Exams = GetExaminations() // Keep this as is, which is a List<SelectListItem>
            };

            return View(model);
        }






        // Add this method to populate examinations
        private List<SelectListItem> GetExaminations()
        {
            return _context.Examinations.Select(e => new SelectListItem
            {
                Value = e.ExaminationID.ToString(),
                Text = e.ExamName // Assuming ExamName is a property in your Examination model
            }).ToList();
        }


        // Modify your Create method to add the GetStudents method
        private List<SelectListItem> GetStudents()
        {
            return _context.Students.Select(s => new SelectListItem
            {
                Value = s.StudentID.ToString(),
                Text = s.FullName // Make sure FullName is a property in your Student model
            }).ToList();
        }


        private List<SelectListItem> GetClasses()
        {
            return _context.Classes
                           .Select(c => new SelectListItem
                           {
                               Value = c.ClassID.ToString(),
                               Text = c.ClassName // Assuming you have a ClassName property
                           })
                           .ToList();
        }


        private List<SelectListItem> GetSubjects()
        {
            return _context.Subjects
                           .Select(s => new SelectListItem
                           {
                               Value = s.SubjectID.ToString(),
                               Text = s.Name // Assuming you have a SubjectName property
                           })
                           .ToList();
        }


public async Task<IActionResult> EnterResults(int? selectedExamID)
{
    var viewModel = new ClassExaminationViewModel
    {
        Exams = await _context.Examinations
            .Select(e => new SelectListItem
            {
                Value = e.ExaminationID.ToString(),
                Text = e.ExamName
            }).ToListAsync(),
        Classes = await _context.Classes
            .Include(c => c.Students)
            .ToListAsync(),
        SelectedExamID = selectedExamID,
        Results = new Dictionary<int, StudentExamResultViewModel>()
    };

    // Initialize the results dictionary with student IDs and default scores
    foreach (var cls in viewModel.Classes)
    {
        foreach (var student in cls.Students)
        {
            viewModel.Results[student.StudentID] = new StudentExamResultViewModel
            {
                StudentID = student.StudentID,
                Score = 0 // Default score
            };
        }
    }

    return View(viewModel);
}


        [HttpPost]
        public async Task<IActionResult> SaveResults(ClassExaminationViewModel model)
        {
            if (model.Results != null)
            {
                foreach (var result in model.Results)
                {
                    var studentExamResult = new StudentExamsResult
                    {
                        StudentID = result.Value.StudentID,
                        ExaminationID = model.SelectedExamID.Value,
                        Score = result.Value.Score
                    };

                    _context.StudentExamsResult.Add(studentExamResult); // Save to the context
                }
                await _context.SaveChangesAsync(); // Save changes once outside the loop
            }
            return RedirectToAction("Index");
        }



        // POST: StudentExamsResults/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create(CreateStudentExamViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.StudentExamsResult.Add(model.ExamResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, repopulate the dropdowns
            model.Classes = GetClasses();
            model.Subjects = GetSubjects();
            model.Students = GetStudents(); // Repopulate students

            return View(model);
        }


        // GET: StudentExamsResults/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var studentExamsResult = await _context.StudentExamsResult.FindAsync(id);
            if (studentExamsResult == null)
            {
                return NotFound();
            }
            ViewData["ExaminationID"] = new SelectList(_context.Examinations, "ExaminationID", "ExamName", studentExamsResult.ExaminationID);
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", studentExamsResult.ClassID);
            return View(studentExamsResult);
        }

        // POST: StudentExamsResults/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentExamResultID,Score,StudentID,ExaminationID,ClassID")] StudentExamsResult studentExamsResult)
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
            ViewData["ExaminationID"] = new SelectList(_context.Examinations, "ExaminationID", "ExamName", studentExamsResult.ExaminationID);
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "ClassName", studentExamsResult.ClassID);
            return View(studentExamsResult);
        }

        // GET: StudentExamsResults/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var studentExamsResult = await _context.StudentExamsResult
                .Include(r => r.Student)
                .Include(r => r.Examination)
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
            var studentExamsResult = await _context.StudentExamsResult.FindAsync(id);
            _context.StudentExamsResult.Remove(studentExamsResult);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExamsResultExists(int id)
        {
            return _context.StudentExamsResult.Any(e => e.StudentExamResultID == id);
        }
        public IActionResult ViewAttendance(DateTime? attendanceDate)
        {
            // Default to today's date if none is provided
            if (attendanceDate == null)
            {
                attendanceDate = DateTime.Today;
            }

            var attendanceRecords = _context.Attendance
                .Include(a => a.Student)
                .ThenInclude(s => s.Class)
                .Where(a => a.Date.Date == attendanceDate.Value.Date)
                .Select(a => new AttendanceViewModel
                {
                    StudentName = a.Student.FirstName + " " + a.Student.LastName,
                    ClassName = a.Student.Class.ClassName,
                    IsPresent = a.IsPresent,
                    Date = a.Date
                })
                .ToList();

            // Grouping the records by ClassName
            var groupedRecords = attendanceRecords
                .GroupBy(a => a.ClassName)
                .ToDictionary(g => g.Key, g => g.ToList());

            return View(groupedRecords); // Pass the dictionary to the view
        }







        // GET: GetStudentsByClass
        public JsonResult GetStudentsByClass(int classId)
        {
            var students = _context.Students
                .Where(s => s.ClassID == classId)
                .Select(s => new
                {
                    s.StudentID,
                    FullName = s.FirstName + " " + s.LastName
                }).ToList();
            return Json(students);
        }
    }
}
