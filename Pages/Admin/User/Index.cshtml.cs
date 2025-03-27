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

namespace GharchNews.Pages.Admin.User
{
    [Authorize(Roles = SD.Admin)]
    public class IndexModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public IndexModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Users> Users { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Users = await _context.Users.OrderByDescending(u => u.InRole).ToListAsync();
        }
    }
}
