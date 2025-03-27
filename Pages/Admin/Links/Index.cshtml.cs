using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GharchNews.Roles;

namespace GharchNews.Pages.Admin.Links
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public IndexModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Links> Links { get; set; } = default!;
        public Models.Links Link { get; set; }

        public async Task OnGetAsync()
        {
            ViewData["userId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Links = await _context.Links.Include(g => g.Users).ToListAsync();
            }
            else
            {
                Links = await _context.Links.Where(l => l.Id == userId).Include(g => g.Users).ToListAsync();
            }

        }

        public async Task<IActionResult> OnGetDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Link = await _context.Links.FindAsync(id);
            if (Link != null)
            {
                _context.Links.Remove(Link);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public IActionResult OnGetIsSuccess(int id)
        {
            var MyLink = _context.Links.FirstOrDefault(i => i.LinkId == id);

            if (MyLink.IsSuccess == true)
            {
                MyLink.IsSuccess = false;
                _context.SaveChanges();
                return RedirectToAction("./index");
            }

            MyLink.IsSuccess = true;
            _context.SaveChanges();
            return RedirectToAction("./index");
        }
    }
}
