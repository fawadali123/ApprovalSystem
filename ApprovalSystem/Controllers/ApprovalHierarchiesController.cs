using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApprovalSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApprovalSystem.Controllers
{
    [Authorize]
    public class ApprovalHierarchiesController : Controller
    {
        private readonly ApprovalSystemContext _context;

        public ApprovalHierarchiesController(ApprovalSystemContext context)
        {
            _context = context;
        }

        // GET: ApprovalHierarchies
        public async Task<IActionResult> Index()
        {
            var approvalSystemContext = _context.ApprovalHierarchy.Include(a => a.ApprovalType);
            return View(await approvalSystemContext.ToListAsync());
        }

        // GET: ApprovalHierarchies/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalHierarchy = await _context.ApprovalHierarchy
                .Include(a => a.ApprovalType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (approvalHierarchy == null)
            {
                return NotFound();
            }

            return View(approvalHierarchy);
        }

        // GET: ApprovalHierarchies/Create
        public IActionResult Create()
        {
            ViewData["ApprovalTypeId"] = new SelectList(_context.ApprovalType, "Id", "Name");
            return View();
        }

        // POST: ApprovalHierarchies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,ApprovalTypeId,IsActive")] ApprovalHierarchy approvalHierarchy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(approvalHierarchy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApprovalTypeId"] = new SelectList(_context.ApprovalType, "Id", "Name", approvalHierarchy.ApprovalTypeId);
            return View(approvalHierarchy);
        }

        // GET: ApprovalHierarchies/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalHierarchy = await _context.ApprovalHierarchy.FindAsync(id);
            if (approvalHierarchy == null)
            {
                return NotFound();
            }
            ViewData["ApprovalTypeId"] = new SelectList(_context.ApprovalType, "Id", "Name", approvalHierarchy.ApprovalTypeId);
            return View(approvalHierarchy);
        }

        // POST: ApprovalHierarchies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Code,Name,ApprovalTypeId,IsActive")] ApprovalHierarchy approvalHierarchy)
        {
            if (id != approvalHierarchy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(approvalHierarchy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApprovalHierarchyExists(approvalHierarchy.Id))
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
            ViewData["ApprovalTypeId"] = new SelectList(_context.ApprovalType, "Id", "Name", approvalHierarchy.ApprovalTypeId);
            return View(approvalHierarchy);
        }

        // GET: ApprovalHierarchies/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalHierarchy = await _context.ApprovalHierarchy
                .Include(a => a.ApprovalType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (approvalHierarchy == null)
            {
                return NotFound();
            }

            return View(approvalHierarchy);
        }

        // POST: ApprovalHierarchies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var approvalHierarchy = await _context.ApprovalHierarchy.FindAsync(id);
            _context.ApprovalHierarchy.Remove(approvalHierarchy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApprovalHierarchyExists(long id)
        {
            return _context.ApprovalHierarchy.Any(e => e.Id == id);
        }
    }
}
