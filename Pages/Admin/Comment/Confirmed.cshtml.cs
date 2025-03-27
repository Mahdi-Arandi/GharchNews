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

namespace GharchNews.Pages.Admin.Comment
{
    [Authorize(Roles = SD.Admin)]
    public class ConfirmedModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public ConfirmedModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Comment> Comment { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Comment = await _context.Comments
                .Include(c => c.Report)
                .Where(c => c.IsSuccess == true).ToListAsync();

            ViewData["returnUrl"] = "Confirmed";
        }

        public async Task<IActionResult> OnGetSuccess(int id)
        {
            var success = await _context.Comments.FirstAsync(c => c.CommentId == id);
            success.IsSuccess = false;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Confirmed");
        }
    }
}
