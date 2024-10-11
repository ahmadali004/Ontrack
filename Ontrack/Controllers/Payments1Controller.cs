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
    [Authorize(Roles = "Admin")]
    public class Payments1Controller : Controller
    {
        private readonly SchoolContext _context;

        public Payments1Controller(SchoolContext context)
        {
            _context = context;
        }

        // GET: Payments1
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Payments.Include(p => p.Parent).Include(p => p.Student);
            return View(await schoolContext.ToListAsync());
        }

        // GET: Payments1/Details/5
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

        // GET: Payments1/Create
        public IActionResult Create()
        {
            ViewData["ParentID"] = new SelectList(_context.Parents, "ParentID", "ParentID");
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "FirstName");
            return View();
        }

        // POST: Payments1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentID,Amount,TuitionAmount,PaymentDate,PaymentStatus,StudentID,ParentID")] Payment payment)
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

        // GET: Payments1/Edit/5
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

        // POST: Payments1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentID,Amount,TuitionAmount,PaymentDate,PaymentStatus,StudentID,ParentID")] Payment payment)
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

        // GET: Payments1/Delete/5
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

        // POST: Payments1/Delete/5
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

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentID == id);
        }
    }
}
