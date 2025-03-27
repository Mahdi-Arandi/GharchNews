using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharchNews.Models
{
    public class Links
    {
        [Key]
        public int LinkId { get; set; }

        [Display(Name ="نام لینک")]
        public string? LinkName { get; set; }

        [Display(Name = "آدرس لینک")]
        public string? LinkAddress { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsSuccess { get; set; } = false;

        [Display(Name = "آیدی کاربر")]
        public string? Id { get; set; }

        [ForeignKey("Id")]
        [Display(Name = "کاربر")]
        public virtual Users? Users { get; set; }
    }
}
