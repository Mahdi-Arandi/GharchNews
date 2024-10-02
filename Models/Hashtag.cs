using System.ComponentModel.DataAnnotations;

namespace GharchNews.Models
{
    public class Hashtag
    {
        [Key]
        public int HashtagId { get; set; }
        [MaxLength(50)]
        [Display(Name ="هشتگ")]
        public string HashtagName { get; set; }
    }
}
