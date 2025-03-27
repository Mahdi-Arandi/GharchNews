namespace GharchNews.Models.ViewModels
{
    public class AdminSideMenuVM
    {
        public int NotConfirmedReportCount { get; set; }
        public int NotConfirmedCommentCount { get; set; }
        public string Name { get; set; } = default!;
        public string Family { get; set; } = default!;
        public string InRole { get; set; } = default!;
        public string ProfileImage { get; set; } = default!;
    }
}
