using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharchNews.Models
{
    public class Hashtag
    {
        [Key]
        public int HashtagId { get; set; }
        [MaxLength(50)]
        [Display(Name ="هشتگ")]
        public string HashtagName { get; set; }


        [Display(Name = "وضعیت")]
        public bool IsSuccess { get; set; } = false;

        [Display(Name = "آیدی کاربر")]
        public string? Id { get; set; }

        [ForeignKey("Id")]
        [Display(Name = "کاربر")]
        public virtual Users? Users { get; set; }
    }
}
