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

namespace GharchNews.Pages.Admin.Hashtag
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public IndexModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Hashtag> Hashtags { get; set; } = default!;
        public Models.Hashtag Hashtag { get; set; }

        public async Task OnGetAsync()
        {
            ViewData["userId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Hashtags = await _context.Hashtags.Include(h => h.Users).ToListAsync();
            }
            else
            {
                Hashtags = await _context.Hashtags.Where(h => h.Id == userId).Include(h => h.Users).ToListAsync();
            }
        }

        public async Task<IActionResult> OnGetDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Hashtag = await _context.Hashtags.FindAsync(id);
            if (Hashtag != null)
            {
                _context.Hashtags.Remove(Hashtag);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public IActionResult OnGetIsSuccess(int id)
        {
            var MyHashtag = _context.Hashtags.FirstOrDefault(i => i.HashtagId == id);

            if (MyHashtag.IsSuccess == true)
            {
                MyHashtag.IsSuccess = false;
                _context.SaveChanges();
                return RedirectToAction("./index");
            }

            MyHashtag.IsSuccess = true;
            _context.SaveChanges();
            return RedirectToAction("./index");
        }

    }
}
