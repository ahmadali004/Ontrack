using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ontrack.Data;
using Ontrack.Models;
using Ontrack.ViewModels;

namespace Ontrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ParentsController : Controller
    {
        private readonly SchoolContext _context;

        public ParentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Parents
        public async Task<IActionResult> Index()
        {
            var parents = await _context.Parents
                .Include(p => p.Students)
                .ToListAsync();

            var viewModel = new SelectedParentViewModel
            {
                Parents = parents,
                Students = parents.SelectMany(p => p.Students)
                    .Select(s => new StudentPaymentViewModel
                    {
                        StudentID = s.StudentID,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        ClassName = s.Class?.ClassName ?? "N/A",
                        PaymentAmount = CalculatePaymentAmountForStudent(s)
                    }).ToList()
            };

            return View(viewModel);  // Return SelectedParentViewModel
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
