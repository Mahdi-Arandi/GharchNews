using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GharchNews.Roles;

namespace GharchNews.Pages.Admin.Hashtag
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public EditModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Hashtag Hashtag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            initHashtags(id);
            if (Hashtag == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Hashtag = await _context.Hashtags.FirstOrDefaultAsync(m => m.HashtagId == id);
            }
            else if (User.IsInRole(SD.User) && Hashtag.Id == userId && Hashtag.IsSuccess == false)
            {
                Hashtag = await _context.Hashtags.FirstOrDefaultAsync(m => m.HashtagId == id);
            }
            else
            {
                return NotFound();
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Hashtag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HashtagExists(Hashtag.HashtagId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        void initHashtags(int? id)
        {
            Hashtag = _context.Hashtags.FirstOrDefault(m => m.HashtagId == id);
        }

        private bool HashtagExists(int id)
        {
            return _context.Hashtags.Any(e => e.HashtagId == id);
        }
    }
}
