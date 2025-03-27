using System.ComponentModel.DataAnnotations;

namespace GharchNews.Models
{
    public class AboutUs
    {
        [Key]
        public int AboutId { get; set; }

        [MaxLength(2000)]
        [Display(Name = "درباره ما")]
        public string? AboutText { get; set; }
    }
}
