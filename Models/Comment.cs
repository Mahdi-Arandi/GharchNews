using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharchNews.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "{0} را وارد کنید")]
        [Display(Name = "نام")]
        [MaxLength(50)]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "{0} را وارد کنید")]
        [Display(Name = "ایمیل")]
        [MaxLength(50)]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "{0} را وارد کنید")]
        [Display(Name = "نظر")]
        public string CommentText { get; set; } = default!;

        [Display(Name = "نمایش نظر")]
        public bool IsSuccess { get; set; }

        [Display(Name = "زمان درج")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "صفحه خبر")]
        public int ReportId { get; set; }

        [ForeignKey("ReportId")]
        [Display(Name = "صفحه خبر")]
        public virtual Report? Report { get; set; }
    }
}
