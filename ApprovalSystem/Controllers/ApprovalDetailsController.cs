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
    public class ApprovalDetailsController : Controller
    {
        private readonly ApprovalSystemContext _context;

        public ApprovalDetailsController(ApprovalSystemContext context)
        {
            _context = context;
        }

        // GET: ApprovalDetails
        [Authorize]
        public async Task<IActionResult> Index(Int64? id)
        {
            if (id == null)
            {
                var approvalSystemContext = _context.ApprovalDetail.Include(a => a.Approval).Include(a => a.ApprovalStatus).Include(a => a.User);
                return View(await approvalSystemContext.ToListAsync());
            }
            else
            {
                var approvalSystemContext = _context.ApprovalDetail
                    .Include(a => a.Approval)
                    .Include(a => a.ApprovalStatus)
                    .Include(a => a.User).Where(t=>t.ApprovalId==id);
                return View(await approvalSystemContext.ToListAsync());
            }
        }

        // GET: ApprovalDetails/Details/5
        [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalDetail = await _context.ApprovalDetail
                .Include(a => a.Approval)
                .Include(a => a.ApprovalStatus)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (approvalDetail == null)
            {
                return NotFound();
            }

            return View(approvalDetail);
        }

        [Authorize(Roles = "SuperAdmin")]
        // GET: ApprovalDetails/Create
        public IActionResult Create()
        {
            ViewData["ApprovalId"] = new SelectList(_context.Approval, "Id", "Id");
            ViewData["ApprovalStatusId"] = new SelectList(_context.ApprovalStatus, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        // POST: ApprovalDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApprovalId,UserId,Sequence,ApprovalStatusId,ApprovalDate")] ApprovalDetail approvalDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(approvalDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApprovalId"] = new SelectList(_context.Approval, "Id", "Id", approvalDetail.ApprovalId);
            ViewData["ApprovalStatusId"] = new SelectList(_context.ApprovalStatus, "Id", "Id", approvalDetail.ApprovalStatusId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", approvalDetail.UserId);
            return View(approvalDetail);
        }
        [Authorize(Roles = "SuperAdmin")]
        // GET: ApprovalDetails/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalDetail = await _context.ApprovalDetail.FindAsync(id);
            if (approvalDetail == null)
            {
                return NotFound();
            }
            ViewData["ApprovalId"] = new SelectList(_context.Approval, "Id", "Id", approvalDetail.ApprovalId);
            ViewData["ApprovalStatusId"] = new SelectList(_context.ApprovalStatus, "Id", "Id", approvalDetail.ApprovalStatusId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", approvalDetail.UserId);
            return View(approvalDetail);
        }

        // POST: ApprovalDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(long id, [Bind("Id,ApprovalId,UserId,Sequence,ApprovalStatusId,ApprovalDate")] ApprovalDetail approvalDetail)
        {
            if (id != approvalDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(approvalDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApprovalDetailExists(approvalDetail.Id))
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
            ViewData["ApprovalId"] = new SelectList(_context.Approval, "Id", "Id", approvalDetail.ApprovalId);
            ViewData["ApprovalStatusId"] = new SelectList(_context.ApprovalStatus, "Id", "Id", approvalDetail.ApprovalStatusId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", approvalDetail.UserId);
            return View(approvalDetail);
        }

        // GET: ApprovalDetails/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalDetail = await _context.ApprovalDetail
                .Include(a => a.Approval)
                .Include(a => a.ApprovalStatus)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (approvalDetail == null)
            {
                return NotFound();
            }

            return View(approvalDetail);
        }

        // POST: ApprovalDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var approvalDetail = await _context.ApprovalDetail.FindAsync(id);
            _context.ApprovalDetail.Remove(approvalDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApprovalDetailExists(long id)
        {
            return _context.ApprovalDetail.Any(e => e.Id == id);
        }
    }
}
