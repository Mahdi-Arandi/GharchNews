using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;

namespace GharchNews.Pages.Admin.Report
{
    public class IndexModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public IndexModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Report> Report { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Report = await _context.Reports
                .Include(r => r.ReportGroup).OrderByDescending(r => r.CreateDate).ToListAsync();
        }
    }
}
