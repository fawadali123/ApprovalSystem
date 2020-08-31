using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApprovalSystem.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ApprovalSystem.Controllers
{
    [Authorize]
    public class ApprovalsController : Controller
    {
        private readonly ApprovalSystemContext _context;
        private UserManager<IdentityUser<Int64>> _userManager;

        public ApprovalsController(ApprovalSystemContext context, UserManager<IdentityUser<Int64>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Approvals
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var approvalSystemContext = _context.Approval.Include(a => a.ApprovalStatus).Include(a => a.ApprovalType)
                .Include(a => a.CreatedByNavigation)
                .Include(a => a.UpdateByNavigation)
                .Include(a=>a.AssignedToNavigation)
                .Where(t => (t.CreatedBy == Convert.ToInt64(user.Id) || t.AssignedTo== Convert.ToInt64(user.Id)) && t.IsDeleted==false);
            return View(await approvalSystemContext.ToListAsync());
        }

        // GET: Approvals/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approval = await _context.Approval
                .Include(a => a.ApprovalStatus)
                .Include(a => a.ApprovalType)
                .Include(a => a.CreatedByNavigation)
                .Include(a => a.UpdateByNavigation)
                .Include(a=>a.AssignedToNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (approval == null)
            {
                return NotFound();
            }

            return View(approval);
        }

        // GET: Approvals/Create
        public IActionResult Create()
        {
            ViewData["ApprovalStatusId"] = new SelectList(_context.ApprovalStatus, "Id", "Name");
            ViewData["ApprovalTypeId"] = new SelectList(_context.ApprovalType, "Id", "Name");
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            ViewData["UpdateBy"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            return View();
        }

        // POST: Approvals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ApprovalTypeId,ApprovalStatusId,AssignedTo,CreatedOn,CreatedBy,UpdateOn,UpdateBy,IsDeleted")] Approval approval,IFormFile file)
        {
            var path = "";
            if (file!=null && file.Length > 0)
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Docs",
                    file.FileName);
                var filePath = Path.GetTempFileName();

                //using (var stream = System.IO.File.Create(filePath))
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    if (stream.Length < 2097152)
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                var hierarachy = _context.ApprovalHierarchy.SingleOrDefault(t => t.ApprovalTypeId == approval.ApprovalTypeId);
                var hierarchyDetail = _context.ApprovalHierarchyDetail.Where(t => t.ApprovalHierarchyId== hierarachy.Id).ToList();
                approval.AssignedTo = hierarchyDetail.Single(t=>t.Sequence ==hierarchyDetail.Min(tt => tt.Sequence)).UserId;
                approval.ApprovalStatusId = 1;
                approval.FilePath = path;
                
                foreach (var item in hierarchyDetail)
                {
                    ApprovalDetail approvalDetail = new ApprovalDetail() { ApprovalId = approval.Id, UserId = item.UserId, Sequence = item.Sequence, ApprovalStatusId = 1 };
                    //_context.Add(approvalDetail);
                    approval.ApprovalDetail.Add(approvalDetail);
                }
                _context.Add(approval);
                var record = await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApprovalStatusId"] = new SelectList(_context.ApprovalStatus, "Id", "Name", approval.ApprovalStatusId);
            ViewData["ApprovalTypeId"] = new SelectList(_context.ApprovalType, "Id", "Name", approval.ApprovalTypeId);
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "UserName", approval.CreatedBy);
            ViewData["UpdateBy"] = new SelectList(_context.AspNetUsers, "Id", "UserName", approval.UpdateBy);
            return View(approval);
        }

        // GET: Approvals/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var approval = await _context.Approval.FindAsync(id);
            if (approval == null)
            {
                return NotFound();
            }
            else if(approval.AssignedTo!= user.Id || approval.ApprovalStatusId==(Int64)EApprovalStatus.Approve)
            {
                return RedirectToAction("Details", "Approvals", new { Id = id });
            }
            ViewData["ApprovalStatusId"] = new SelectList(_context.ApprovalStatus, "Id", "Name", approval.ApprovalStatusId);
            ViewData["ApprovalTypeId"] = new SelectList(_context.ApprovalType, "Id", "Name", approval.ApprovalTypeId);
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "UserName", approval.CreatedBy);
            ViewData["UpdateBy"] = new SelectList(_context.AspNetUsers, "Id", "UserName", approval.UpdateBy);
            ViewData["AssignedTo"] = new SelectList(_context.AspNetUsers, "Id", "UserName", approval.AssignedTo);
            return View(approval);
        }

        // POST: Approvals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,Description,ApprovalTypeId,ApprovalStatusId,AssignedTo,CreatedOn,CreatedBy,UpdateOn,UpdateBy,IsDeleted,CommentsHistory,LastComment")] Approval approval,IFormFile file,string Approve,string Reject)
        {
            var user = await _userManager.GetUserAsync(User);

            if (id != approval.Id)
            {
                return NotFound();
            }
            var path = "";
            if (file != null && file.Length > 0 && Approve==null && Reject=="")
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Docs",file.FileName);
                var filePath = Path.GetTempFileName();

                //using (var stream = System.IO.File.Create(filePath))
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    if (stream.Length < 2097152)
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    approval.FilePath = path;
                    approval.IsDeleted = false;
                    var approvalDetail = _context.ApprovalDetail.FirstOrDefault(t => t.ApprovalId == id && t.UserId == approval.AssignedTo);
                    if (Approve!=null && Approve.Equals("Approve"))
                    {
                        approval.LastComment = $@"{Environment.NewLine} APPROVED ({user.UserName})  - ->  {approval.LastComment}";
                        if (_context.ApprovalDetail.Max(t => t.Sequence) != approvalDetail.Sequence)
                        {
                            var nextApprover = _context.ApprovalDetail.Single(t => t.ApprovalId == id && t.Sequence == approvalDetail.Sequence + 1);
                            approval.AssignedTo = nextApprover.UserId;
                            approval.ApprovalStatusId = (Int64)EApprovalStatus.InProcess;
                        }
                        else
                        {
                            approval.ApprovalStatusId = (Int64)EApprovalStatus.Approve;
                        }
                        approvalDetail.ApprovalStatusId = (Int64)EApprovalStatus.Approve;
                        approvalDetail.Comments = (String.IsNullOrWhiteSpace(approval.LastComment) ? "" : approval.LastComment);
                        approvalDetail.ApprovalDate = DateTime.Now;
                        _context.Update(approvalDetail);
                    }
                    else if (Reject!=null && Reject.Equals("Reject"))
                    {
                        approvalDetail.Comments = (String.IsNullOrWhiteSpace(approval.LastComment) ? "" : approval.LastComment);
                        approvalDetail.ApprovalDate = DateTime.Now;
                        approvalDetail.ApprovalStatusId = (Int64)EApprovalStatus.Reject;
                        approval.ApprovalStatusId = (Int64)EApprovalStatus.Reject;
                        approval.AssignedTo = approval.CreatedBy;
                        approval.LastComment = $@"{Environment.NewLine} REJECTED ({user.UserName})  - -> {approval.LastComment}";
                        _context.Update(approvalDetail);
                    }
                    else
                    {
                        var approvalDetail1 = _context.ApprovalDetail.FirstOrDefault(t => t.ApprovalId == id && t.Sequence==1);
                        approval.AssignedTo = approvalDetail1.UserId;
                        approval.CommentsHistory += $@"{Environment.NewLine} (RESUBMITTED) {user.UserName} - -> {approval.LastComment}";
                    }
                    approval.UpdateOn = DateTime.Now;
                    approval.UpdateBy = user.Id;
                    _context.Update(approval);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApprovalExists(approval.Id))
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
            ViewData["ApprovalStatusId"] = new SelectList(_context.ApprovalStatus, "Id", "Name", approval.ApprovalStatusId);
            ViewData["ApprovalTypeId"] = new SelectList(_context.ApprovalType, "Id", "Name", approval.ApprovalTypeId);
            ViewData["CreatedBy"] = new SelectList(_context.AspNetUsers, "Id", "UserName", approval.CreatedBy);
            ViewData["UpdateBy"] = new SelectList(_context.AspNetUsers, "Id", "UserName", approval.UpdateBy);
            return View(approval);
        }

        // GET: Approvals/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var approval = await _context.Approval
                .Include(a => a.ApprovalStatus)
                .Include(a => a.ApprovalType)
                .Include(a => a.CreatedByNavigation)
                .Include(a => a.UpdateByNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (approval == null)
            {
                return NotFound();
            }
            else if(approval.CreatedBy !=user.Id || approval.ApprovalStatusId == (Int64)EApprovalStatus.Approve || approval.ApprovalStatusId == (Int64)EApprovalStatus.InProcess || approval.ApprovalStatusId == (Int64)EApprovalStatus.Pending)
            {
                return Forbid();
            }
            return View(approval);
        }

        // POST: Approvals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var approval = await _context.Approval.FindAsync(id);
            approval.IsDeleted = true;
            //_context.Approval.Remove(approval);
            _context.Approval.Update(approval);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DMS(long? id)
        {
            var documents  = _context.Approval.Include(a => a.ApprovalStatus).Include(a => a.ApprovalType)
                .Include(a => a.CreatedByNavigation)
                .Include(a => a.UpdateByNavigation)
                .Include(a => a.AssignedToNavigation)
                .Where(t=>t.IsDeleted == false); ;
            return View(await documents.ToListAsync());
        }
        private bool ApprovalExists(long id)
        {
            return _context.Approval.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
