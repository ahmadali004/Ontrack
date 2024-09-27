using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ontrack.Data;
using Ontrack.Models;
using Ontrack.ViewModels;

namespace Ontrack.Controllers
{
    public class ParentsController : Controller
    {
        private readonly SchoolContext _context;

        public ParentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Parents
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            // Query parents with their students and payments
            var parentsQuery = _context.Parents
                .Include(p => p.Students)
                .Include(p => p.Payments)
                .ThenInclude(pay => pay.Student)
                .AsQueryable();

            // Search by parent's first or last name
            if (!String.IsNullOrEmpty(searchString))
            {
                parentsQuery = parentsQuery
                    .Where(p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString));
            }

            var parentsWithPayments = await parentsQuery.ToListAsync();

            // Prepare the view model list
            var viewModelList = new List<ParentWithStudentsViewModel>();

            foreach (var parent in parentsWithPayments)
            {
                var children = parent.Payments.Select(p => p.Student).ToList();

                if (children.Count > 0)
                {
                    int numberOfChildren = children.Count;

                    // Set the appropriate fee based on the number of children
                    for (int i = 0; i < numberOfChildren; i++)
                    {
                        decimal fee = 1700;
                        if (i == 1 || i == 2)
                            fee = 1500;
                        else if (i >= 3)
                            fee = 1300;

                        var payment = parent.Payments.FirstOrDefault(p => p.Student.StudentID == children[i].StudentID);
                        if (payment != null)
                        {
                            payment.Amount = fee;
                        }
                    }
                }

                viewModelList.Add(new ParentWithStudentsViewModel
                {
                    Parent = parent,
                    Students = parent.Students.ToList(),
                    Payments = parent.Payments.ToList()
                });
            }

            return View(viewModelList);
        }

        // Show parents with students
        public async Task<IActionResult> WithStudents(string searchString)
        {
            var parentsWithStudents = await _context.Parents
                .Include(p => p.Students)
                .ThenInclude(s => s.Payments)
                .ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                parentsWithStudents = parentsWithStudents.Where(p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString)).ToList();
            }

            return View(parentsWithStudents);
        }

        // POST: Check if all payments are made
        [HttpPost]
        public IActionResult CheckPayments(int parentID)
        {
            var parent = _context.Parents
                .Include(p => p.Students)
                .ThenInclude(s => s.Payments)
                .FirstOrDefault(p => p.ParentID == parentID);

            if (parent == null)
            {
                return NotFound();
            }

            bool allPaid = parent.Students.All(student => student.Payments.Any(payment => payment.IsPaid));

            if (allPaid)
            {
                TempData["Message"] = $"{parent.FirstName} {parent.LastName} has paid for all children.";
            }
            else
            {
                TempData["Message"] = $"{parent.FirstName} {parent.LastName} has unpaid children.";
            }

            return RedirectToAction("WithStudents");
        }

        // GET: Parents/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // POST: Show children for a specific parent
        [HttpPost]
        public async Task<IActionResult> ShowChildren(int parentID)
        {
            var parentWithStudents = await _context.Parents
                .Include(p => p.Students)
                .ThenInclude(s => s.Class)
                .FirstOrDefaultAsync(p => p.ParentID == parentID);

            if (parentWithStudents == null)
            {
                return NotFound();
            }

            var viewModel = new ParentWithStudentsViewModel
            {
                Parent = parentWithStudents,
                Students = parentWithStudents.Students.ToList()
            };

            return View(viewModel);
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
