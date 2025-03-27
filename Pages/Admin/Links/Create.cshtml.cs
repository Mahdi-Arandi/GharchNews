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
using GharchNews.Roles;
using System.Security.Claims;

namespace GharchNews.Pages.Admin.Links
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
        public Models.Links Links { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Links.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Links.IsSuccess = true;
            }
            else
            {
                Links.IsSuccess = false;
            }

            _context.Links.Add(Links);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
