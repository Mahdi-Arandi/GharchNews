using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Roles;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace GharchNews.Pages.Admin.Report
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public DeleteModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Report Report { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id, string? ReturnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            initReport(id);

            if (Report == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Report = await _context.Reports.Include(g => g.ReportGroup).FirstOrDefaultAsync(m => m.ReportId == id);
            }
            else if (User.IsInRole(SD.User) && Report.Id == userId && Report.IsSuccess == false)
            {
                Report = await _context.Reports.Include(g => g.ReportGroup).FirstOrDefaultAsync(m => m.ReportId == id);
            }
            else
            {
                return NotFound();
            }

            if (ReturnUrl != null)
            {
                ViewData["ReturnUrl"] = ReturnUrl;
            }
            else
            {
                ViewData["ReturnUrl"] = "Index";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string? ReturnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            Report = await _context.Reports.FindAsync(id);
            if (Report != null)
            {
                if (Report.Image != null)
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/ReportImages", Report.Image);
                    if (System.IO.File.Exists(deletePath))
                        System.IO.File.Delete(deletePath);
                }

                _context.Reports.Remove(Report);
                await _context.SaveChangesAsync();
            }

            if (ReturnUrl != null)
            {
                return RedirectToPage(ReturnUrl);
            }
            return RedirectToPage("./Index");
        }

        void initReport(int? id)
        {
            Report = _context.Reports.Include(g => g.ReportGroup).FirstOrDefault(m => m.ReportId == id);
        }
    }
}
