﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ontrack.Data;
using Ontrack.Models;
using Ontrack.ViewModels;

namespace Ontrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PaymentsController : Controller
    {
        private readonly SchoolContext _context;

        public PaymentsController(SchoolContext context)
        {
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    var viewModel = new SelectedParentViewModel
        //    {
        //        Months = GetMonthsSelectList(),
        //        Years = GetYearsSelectList(),
        //        Parents = _context.Parents
        //            .Include(p => p.Students) // Make sure Students are included
        //            .ToList(),
        //        SelectedMonth = DateTime.Now.Month,
        //        SelectedYear = DateTime.Now.Year
        //    };

        //    return View(viewModel);
        //}

        public async Task<IActionResult> Index(int? selectedMonth, int? selectedYear)
        {
            // Default to the current month and year if not provided
            var currentMonth = selectedMonth ?? DateTime.Now.Month;
            var currentYear = selectedYear ?? DateTime.Now.Year;

            var parents = await _context.Parents
                .Include(p => p.Students)
                .ThenInclude(s => s.Class)
                .ToListAsync();

            var viewModel = new SelectedParentViewModel
            {
                Parents = parents,
                Students = new List<StudentPaymentViewModel>(),
                SelectedMonth = currentMonth,
                SelectedYear = currentYear,
                Months = new SelectList(Enumerable.Range(1, 12).Select(m => new { Value = m, Text = new DateTime(1, m, 1).ToString("MMMM") }), "Value", "Text", currentMonth),
                Years = new SelectList(Enumerable.Range(2020, DateTime.Now.Year - 2020 + 1), currentYear)
            };

            // Populate students in the viewModel as in your original code...

            foreach (var parent in parents)
            {
                int childIndex = 1;

                foreach (var student in parent.Students)
                {
                    // Calculate payment amount based on child order
                    decimal paymentAmount = childIndex == 1 ? 1700 :
                                            childIndex <= 3 ? 1500 : 1300;

                    // Check if student has already paid for this month
                    bool hasPaid = _context.Payments
                        .Any(p => p.StudentID == student.StudentID
                                  && p.PaymentDate.Month == currentMonth
                                  && p.PaymentDate.Year == currentYear);

                    var studentPayment = new StudentPaymentViewModel
                    {
                        StudentID = student.StudentID,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        ClassName = student.Class?.ClassName ?? "N/A",
                        PaymentAmount = paymentAmount,
                        ParentID = parent.ParentID,
                        HasPaid = hasPaid // Add a flag indicating if they've paid
                    };

                    viewModel.Students.Add(studentPayment);
                    childIndex++;
                }
            }
            return View(viewModel);
        }





        private decimal CalculatePaymentAmount(Student student)
        {
            var studentCount = _context.Students.Count(s => s.ParentID == student.ParentID);
            if (studentCount == 1)
                return 1700;
            if (studentCount == 2 || studentCount == 3)
                return 1500;
            return 1300;
        }

        [HttpPost]
        public IActionResult LoadChildren(int parentId)
        {
            var parent = _context.Parents
                .Include(p => p.Students)
                .ThenInclude(s => s.Class)
                .FirstOrDefault(p => p.ParentID == parentId);

            if (parent == null)
            {
                return NotFound();
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
                    PaymentAmount = GetPaymentAmountForStudent(s.StudentID)
                }).ToList()
            };

            return View("Index", viewModel);
        }

    

        [HttpPost]
        public async Task<IActionResult> ProcessPayments(int parentId, List<int> selectedStudentIDs, Dictionary<int, decimal> studentTuition, int selectedMonth, int selectedYear)
        {
            if (selectedStudentIDs == null || !selectedStudentIDs.Any())
            {
                return RedirectToAction(nameof(Index), new { selectedMonth, selectedYear });
            }

            foreach (var studentID in selectedStudentIDs)
            {
                var student = await _context.Students.FindAsync(studentID);
                if (student != null)
                {
                    bool hasPaid = _context.Payments.Any(p => p.StudentID == studentID && p.PaymentDate.Month == selectedMonth && p.PaymentDate.Year == selectedYear);

                    if (!hasPaid && studentTuition.TryGetValue(studentID, out var paymentAmount))
                    {
                        var payment = new Payment
                        {
                            StudentID = studentID,
                            ParentID = parentId,
                            Amount = paymentAmount,
                            PaymentDate = new DateTime(selectedYear, selectedMonth, 1),
                            PaymentStatus = "Confirmed"
                        };
                        _context.Payments.Add(payment);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { selectedMonth, selectedYear });
        }





        private decimal GetPaymentAmountForStudent(int studentID)
        {
            var student = _context.Students.Find(studentID);
            var studentCount = _context.Students.Count(s => s.ParentID == student.ParentID);
            if (studentCount == 1)
                return 1700;
            if (studentCount == 2 || studentCount == 3)
                return 1500;
            return 1300;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Parent)
                .Include(p => p.Student)
                .FirstOrDefaultAsync(m => m.PaymentID == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        public IActionResult Create()
        {
            ViewData["ParentID"] = new SelectList(_context.Parents, "ParentID", "ParentID");
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "FirstName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentID,Amount,PaymentDate,PaymentStatus,StudentID,ParentID")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentID"] = new SelectList(_context.Parents, "ParentID", "ParentID", payment.ParentID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "FirstName", payment.StudentID);
            return View(payment);
        }
		public async Task<IActionResult> Transactions()
		{
			var transactions = await _context.Payments
				.Include(p => p.Parent)
				.Include(p => p.Student)
				.Select(p => new TransactionViewModel
				{
					PaymentID = p.PaymentID,
					ParentName = p.Parent.FirstName + " " + p.Parent.LastName,
					StudentName = p.Student.FirstName + " " + p.Student.LastName,
					Amount = p.Amount,
					PaymentDate = p.PaymentDate,
					PaymentStatus = p.PaymentStatus
				})
				.ToListAsync();

			return View(transactions);
		}

		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["ParentID"] = new SelectList(_context.Parents, "ParentID", "ParentID", payment.ParentID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "FirstName", payment.StudentID);
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentID,Amount,PaymentDate,PaymentStatus,StudentID,ParentID")] Payment payment)
        {
            if (id != payment.PaymentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentID))
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
            ViewData["ParentID"] = new SelectList(_context.Parents, "ParentID", "ParentID", payment.ParentID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "FirstName", payment.StudentID);
            return View(payment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Parent)
                .Include(p => p.Student)
                .FirstOrDefaultAsync(m => m.PaymentID == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private SelectList GetMonthsSelectList()
        {
            var months = Enumerable.Range(1, 12)
                .Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i)
                }).ToList();

            return new SelectList(months, "Value", "Text");
        }

        private SelectList GetYearsSelectList()
        {
            var currentYear = DateTime.Now.Year;
            var years = Enumerable.Range(currentYear - 10, 20) // Shows years from 10 years ago up to 10 years ahead
                .Select(y => new SelectListItem
                {
                    Value = y.ToString(),
                    Text = y.ToString()
                }).ToList();

            return new SelectList(years, "Value", "Text");
        }
        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentID == id);
        }
    }
}
