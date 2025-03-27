namespace GharchNews.Models.ViewModels
{
    public class FooterVM
    {
        public IEnumerable<Report> Reports { get; set; } = default!;
        public IEnumerable<Comment> Comments { get; set; } = default!;
        public IEnumerable<Image> Images { get; set; } = default!;
        public AboutUs AboutUs { get; set; } = default!;
    }
}
