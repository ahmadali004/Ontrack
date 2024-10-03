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
    public class ParentsController : Controller
    {
        private readonly SchoolContext _context;

        public ParentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Parents
        public async Task<IActionResult> Index(string searchString, int? parentID)
        {
          
            var parentsQuery = from p in _context.Parents
                               select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                parentsQuery = parentsQuery.Where(p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString));
            }

            var parents = await parentsQuery.ToListAsync();

          
            List<Student> childrenList = new List<Student>();
            if (parentID.HasValue)
            {
                var selectedParent = await _context.Parents
                                        .Include(p => p.Students)
                                        .ThenInclude(s => s.Class)
                                        .FirstOrDefaultAsync(p => p.ParentID == parentID);

                if (selectedParent != null)
                {
                    childrenList = selectedParent.Students.ToList();
                }


                ViewData["SelectedParentID"] = parentID;
            }

          
            ViewData["ChildrenList"] = childrenList;

            return View(parents); 
        }

        [HttpPost]
        public async Task<IActionResult> LoadChildren(int parentID)
        {
            var parentWithStudents = await _context.Parents
                .Include(p => p.Students)
                .ThenInclude(s => s.Class)
                .FirstOrDefaultAsync(p => p.ParentID == parentID);

            if (parentWithStudents == null)
            {
                return NotFound();
            }

            // Pass the list of students to the main view via ViewData or ViewModel
            ViewData["SelectedParentID"] = parentID;
            ViewData["ChildrenList"] = parentWithStudents.Students.ToList();

            // Return to the main view
            return RedirectToAction("Index");
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
