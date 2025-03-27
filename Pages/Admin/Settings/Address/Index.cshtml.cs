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

namespace GharchNews.Pages.Admin.Settings.Address
{
    [Authorize(Roles = SD.Admin)]
    public class IndexModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public IndexModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Address> Address { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Address = await _context.Address.ToListAsync();
        }

        public async Task<IActionResult> OnGetDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address.FirstAsync(a => a.AddressId == id);
            if (address != null)
            {
                _context.Address.Remove(address);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("index");
        }

    }
}
