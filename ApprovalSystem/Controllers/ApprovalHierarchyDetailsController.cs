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
    public class ApprovalHierarchyDetailsController : Controller
    {
        private readonly ApprovalSystemContext _context;

        public ApprovalHierarchyDetailsController(ApprovalSystemContext context)
        {
            _context = context;
        }

        // GET: ApprovalHierarchyDetails
        public async Task<IActionResult> Index(Int64? id)
        {
            if (id != null)
            {
                var approvalSystemContext = _context.ApprovalHierarchyDetail.Where(t=>t.ApprovalHierarchyId==id)
                    .Include(a => a.ApprovalHierarchy)
                    .Include(a => a.User);
                return View(await approvalSystemContext.ToListAsync());
            }
            return NotFound();
        }

        // GET: ApprovalHierarchyDetails/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalHierarchyDetail = await _context.ApprovalHierarchyDetail
                .Include(a => a.ApprovalHierarchy)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (approvalHierarchyDetail == null)
            {
                return NotFound();
            }

            return View(approvalHierarchyDetail);
        }

        // GET: ApprovalHierarchyDetails/Create
        public IActionResult Create(Int64? approvalHierarchyId)
        {
            ViewData["ApprovalHierarchyId"] = new SelectList(_context.ApprovalHierarchy, "Id", "Name", approvalHierarchyId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            return View();
        }

        // POST: ApprovalHierarchyDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApprovalHierarchyId,UserId,Sequence")] ApprovalHierarchyDetail approvalHierarchyDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(approvalHierarchyDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ApprovalHierarchies");
            }
            ViewData["approvalHierarchyId"] = new SelectList(_context.ApprovalHierarchy, "Id", "Name", approvalHierarchyDetail.Id);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "UserName", approvalHierarchyDetail.UserId);
            return View(approvalHierarchyDetail);
        }

        // GET: ApprovalHierarchyDetails/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalHierarchyDetail = await _context.ApprovalHierarchyDetail.FindAsync(id);
            if (approvalHierarchyDetail == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.ApprovalHierarchy, "Id", "Name", approvalHierarchyDetail.Id);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "UserName", approvalHierarchyDetail.UserId);
            return View(approvalHierarchyDetail);
        }

        // POST: ApprovalHierarchyDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,ApprovalHierarchyId,UserId,Sequence")] ApprovalHierarchyDetail approvalHierarchyDetail)
        {
            if (id != approvalHierarchyDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(approvalHierarchyDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApprovalHierarchyDetailExists(approvalHierarchyDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "ApprovalHierarchies");
            }
            ViewData["Id"] = new SelectList(_context.ApprovalHierarchy, "Id", "Name", approvalHierarchyDetail.Id);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "UserName", approvalHierarchyDetail.UserId);
            return View(approvalHierarchyDetail);
        }

        // GET: ApprovalHierarchyDetails/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalHierarchyDetail = await _context.ApprovalHierarchyDetail
                .Include(a => a.ApprovalHierarchy)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (approvalHierarchyDetail == null)
            {
                return NotFound();
            }

            return View(approvalHierarchyDetail);
        }

        // POST: ApprovalHierarchyDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var approvalHierarchyDetail = await _context.ApprovalHierarchyDetail.FindAsync(id);
            _context.ApprovalHierarchyDetail.Remove(approvalHierarchyDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApprovalHierarchyDetailExists(long id)
        {
            return _context.ApprovalHierarchyDetail.Any(e => e.Id == id);
        }
    }
}
