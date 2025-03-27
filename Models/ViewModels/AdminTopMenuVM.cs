namespace GharchNews.Models.ViewModels
{
    public class AdminTopMenuVM
    {
        public int NotConfirmedReportCount { get; set; }
        public int NotConfirmedCommentCount { get; set; }
        public int ContactUsCount { get; set; }
        public string Name { get; set; } = default!;
        public string Family { get; set; } = default!;
        public string InRole { get; set; } = default!;
        public string ProfileImage { get; set; } = default!;

        public List<Report> Reports { get; set; } = default!;
        public List<Comment> Comments { get; set; } = default!;
        public List<ContactUs> Contacts { get; set; } = default!;
    }
}
