using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using GharchNews.Roles;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GharchNews.Pages.Admin.Report
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public DetailsModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
            else if (User.IsInRole(SD.User) && Report.Id == userId)
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

        void initReport(int? id)
        {
            Report = _context.Reports.Include(g => g.ReportGroup).FirstOrDefault(m => m.ReportId == id);
        }
    }
}
