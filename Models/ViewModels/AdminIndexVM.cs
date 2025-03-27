namespace GharchNews.Models.ViewModels
{
    public class AdminIndexVM
    {
        public IEnumerable<Report> Reports { get; set; } = default!;
        public IEnumerable<Comment> Comments { get; set; } = default!;
        public IEnumerable<Image> Images { get; set; } = default!;
        public IEnumerable<Ads> Ads { get; set; } = default!;

        public int ReportCount { get; set; } = default!;
        public int CommentCount { get; set; } = default!;
        public int ImageCount { get; set; } = default!;
        public int AdsCount { get; set; } = default!;
    }
}
