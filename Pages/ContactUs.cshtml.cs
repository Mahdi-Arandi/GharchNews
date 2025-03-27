using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GharchNews.Pages
{
    public class ContactUsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ContactUsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Address Address { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Address = await _context.Address.FirstAsync();
            return Page();
        }

        public ContactUs ContactUs { get; set; }
        public async Task<IActionResult> OnPostAsync(string name, string email, string PhoneNumber, string message, string subject)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var ContactMessage = new ContactUs()
            {
                Name = name,
                Email = email,
                PhoneNumber=PhoneNumber,
                Message = message,
                Subject = subject,
                CreateDate = DateTime.Now
            };

            await _context.ContactUs.AddAsync(ContactMessage);
            await _context.SaveChangesAsync();

            return Page();
        }
    }
}
