using AspNetCoreHero.ToastNotification.Abstractions;
using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GharchNews.Pages
{
    public class ReportModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public ReportModel(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public Report Report { get; set; }
        public IEnumerable<Report> Reports { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

        public IActionResult OnGet(int id)
        {
            try
            {
                Report = _context.Reports.Include(r => r.ReportGroup).Where(r => r.ReportId == id && r.IsSuccess == true).FirstOrDefault();
                Report.View += 1;
                _context.Update(Report);
                _context.SaveChanges();
            }
            catch
            {
                return NotFound();
            }

            int take = 10;
            string groupName = Report.ReportGroup.GroupName;
            Reports = _context.Reports.Where(r => r.ReportGroup.GroupName == groupName && r.ReportId != id && r.IsSuccess == true).OrderByDescending(r => r.View).Take(take).ToList();

            Comments = _context.Comments
                .Where(c => c.ReportId == id && c.IsSuccess == true)
                .Select(c => new Comment
                {
                    Name = c.Name,
                    Email = c.Email,
                    CommentText = c.CommentText,
                    CreateDate = c.CreateDate
                }).OrderByDescending(c => c.CreateDate).ToList();

            return Page();
        }


        public Comment Comment { get; set; }

        public async Task<IActionResult> OnPost(int id, string name, string email, string commentText)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var AddComment = new Comment()
            {
                Name = name,
                Email = email,
                CommentText = commentText,
                IsSuccess = false,
                CreateDate = DateTime.Now,
                ReportId = id,
            };

            await _context.Comments.AddAsync(AddComment);
            await _context.SaveChangesAsync();
            OnGet(id);
            _notyf.Success("نظر شما ثبت شد و پس از تایید نمایش داده خواهد شد.");

            return Page();
        }
    }
}
