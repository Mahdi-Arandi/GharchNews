using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GharchNews.Roles;

namespace GharchNews.Pages.Admin.Gallery
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public CreateModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.Gallery Gallery { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Gallery.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Gallery.IsSuccess = true;
            }
            else
            {
                Gallery.IsSuccess = false;
            }

            _context.Galleries.Add(Gallery);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
