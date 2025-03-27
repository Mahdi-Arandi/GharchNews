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
using GharchNews.Roles;
using System.Security.Claims;

namespace GharchNews.Pages.Admin.Links
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
        public Models.Links Links { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            initLinks(id);
            if(Links == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Links = await _context.Links.FirstOrDefaultAsync(m => m.LinkId == id);
            }
            else if (User.IsInRole(SD.User) && Links.Id == userId && Links.IsSuccess == false)
            {
                Links = await _context.Links.FirstOrDefaultAsync(m => m.LinkId == id);
            }
            else
            {
                return NotFound();
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Links).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinksExists(Links.LinkId))
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

        void initLinks(int? id)
        {
            Links = _context.Links.FirstOrDefault(m => m.LinkId == id);
        }


        private bool LinksExists(int id)
        {
            return _context.Links.Any(e => e.LinkId == id);
        }
    }
}
