using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Models.ViewModels;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class AdminSideMenuComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public AdminSideMenuComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AdminSideMenuVM adminSideMenuVM = new AdminSideMenuVM()
            {
                NotConfirmedReportCount = _context.Reports.Where(r=> r.IsSuccess == false).Count(),
                NotConfirmedCommentCount = _context.Comments.Where(c=> c.IsSuccess == false).Count(),
                Name = _context.Users.First(u=> u.UserName == User.Identity.Name).Name,
                Family = _context.Users.First(u=> u.UserName == User.Identity.Name).Family,
                InRole = _context.Users.First(u=> u.UserName == User.Identity.Name).InRole,
                ProfileImage = _context.Users.First(u=> u.UserName == User.Identity.Name).ProfileImage
            };
            return View("/Pages/Components/_AdminSideMenu.cshtml", adminSideMenuVM);
        }
    }
}
