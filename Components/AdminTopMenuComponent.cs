using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Models.ViewModels;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class AdminTopMenuComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public AdminTopMenuComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AdminTopMenuVM adminTopMenuVM = new AdminTopMenuVM()
            {
                NotConfirmedReportCount = _context.Reports.Where(r => r.IsSuccess == false).Count(),
                NotConfirmedCommentCount = _context.Comments.Where(c => c.IsSuccess == false).Count(),
                ContactUsCount = _context.ContactUs.Count(),
                Name = _context.Users.First(u => u.UserName == User.Identity.Name).Name,
                Family = _context.Users.First(u => u.UserName == User.Identity.Name).Family,
                InRole = _context.Users.First(u => u.UserName == User.Identity.Name).InRole,
                ProfileImage = _context.Users.First(u => u.UserName == User.Identity.Name).ProfileImage,
                Reports = await _context.Reports.Include(r=> r.Users).Where(r=> r.IsSuccess == false).Take(20).OrderByDescending(r=> r.CreateDate).ToListAsync(),
                Comments = await _context.Comments.Where(r=> r.IsSuccess == false).Take(20).OrderByDescending(r=> r.CreateDate).ToListAsync(),
                Contacts = await _context.ContactUs.Take(10).OrderByDescending(r=> r.CreateDate).ToListAsync(),
            }; 
            
            return View("/Pages/Components/_AdminTopMenu.cshtml", adminTopMenuVM);
        }
    }
}
