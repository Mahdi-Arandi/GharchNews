using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;

namespace GharchNews.Pages.Admin.ReportGroup
{
    public class DeleteModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public DeleteModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.ReportGroup ReportGroup { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ReportGroup = await _context.ReportGroups.FirstOrDefaultAsync(m => m.GroupId == id);

            if (ReportGroup == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ReportGroup = await _context.ReportGroups.FindAsync(id);
            if (ReportGroup != null)
            {
                if(ReportGroup.GroupImage != null)
                {
                    string deletePath= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/ReportGroupImages" ,ReportGroup.GroupImage);
                    if (System.IO.File.Exists(deletePath))
                        System.IO.File.Delete(deletePath);
                }

                _context.ReportGroups.Remove(ReportGroup);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
