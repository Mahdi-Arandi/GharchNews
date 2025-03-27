using GharchNews.Data;
using GharchNews.Models.ViewModels;
using GharchNews.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using GharchNews.Roles;

namespace GharchNews.Pages.Admin
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<HighChartLineReportGroupVM> HighChartLines { get; set; }
        public void OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            HighChartLines = _context.Reports.Select(h => new HighChartLineReportGroupVM()
            {
                GroupName = h.ReportGroup.GroupName,
                GroupId = h.GroupId
            }).OrderByDescending(h=> h.GroupName).Distinct().ToList();

            List<HighChartLineReportGroupVM> ListHighChart = HighChartLines.ToList();
            List<object> ListObject = new List<object>();
            foreach(var item in ListHighChart)
            {
                object[] newChart = new object[2];
                newChart[0] = item.GroupName;
                if (User.IsInRole(SD.Admin))
                {
                    newChart[1] = _context.Reports.Where(r => r.GroupId == item.GroupId).ToList().Count();
                }
                else
                {
                    newChart[1] = _context.Reports.Where(r => r.GroupId == item.GroupId && r.Id == userId && r.IsSuccess == true).ToList().Count();
                }

                ListObject.Add(newChart);
            }

            ViewData["Result"] = ListObject;

            ViewData["AllReport"]= _context.Reports.ToList().Count();
            ViewData["AllComment"]= _context.Comments.ToList().Count();
            ViewData["AllImage"]= _context.Images.ToList().Count();
            ViewData["AllAds"]= _context.Ads.ToList().Count();



            ViewData["UserReport"] = _context.Reports.Where(r => r.Id == userId).ToList().Count();
            ViewData["UserGallery"] = _context.Galleries.Where(g => g.Id == userId).ToList().Count();
            ViewData["UserImage"] = _context.Images.Where(i => i.Id == userId).ToList().Count();
            ViewData["UserHashtag"] = _context.Hashtags.Where(h => h.Id == userId).ToList().Count();

        }
    }
}
