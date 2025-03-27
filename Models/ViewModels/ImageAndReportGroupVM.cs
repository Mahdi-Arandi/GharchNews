namespace GharchNews.Models.ViewModels
{
    public class ImageAndReportGroupVM
    {
        public IEnumerable<Image> Images { get; set; } = default!;
        public IEnumerable<string> ReportGroups { get; set; } = default!;
    }
}
