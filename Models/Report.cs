using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharchNews.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required(ErrorMessage = "لطفا عنوان {0} را وارد کنید")]
        [MaxLength(100)]
        [Display(Name = "عنوان خبر")]
        public string title { get; set; } = default!;

        [MaxLength(1000)]
        [Display(Name = "توضیح مختصر")]
        public string? Description { get; set; }

        [Display(Name = "متن کامل")]
        public string? FullText { get; set; }

        [MaxLength(100)]
        [Display(Name = "تصویر خبر")]
        public string? Image { get; set; }

        [MaxLength(50)]
        [Display(Name = "توضیح تصویر")]
        public string? ImageAlt { get; set; }

        [MaxLength(100)]
        [Display(Name = "عنوان تصویر")]
        public string? ImageTitle { get; set; }

        [MaxLength(100)]
        [Display(Name = "منبع خبر")]
        public string? Source { get; set; }

        [MaxLength(500)]
        [Display(Name = "برچسب ها")]
        public string? Tags { get; set; }

        [Display(Name = "زمان درج")]
        public DateTime CreateDate { get; set; }=DateTime.Now;

        [MaxLength(100)]
        [Display(Name = "نویسنده خبر")]
        public string? Author { get; set; }

        [Display(Name = "بازدید")]
        public int? View { get; set; }

        [Display(Name = "اخبار داغ")]
        public bool IsHotNews { get; set; } = false;

        [Display(Name = "زمان درج اخبار داغ")]
        public DateTime? HotNewsDate { get; set; }

        [Display(Name = "گروه خبری")]
        public int GroupId { get; set; }=default!;

        [ForeignKey("GroupId")]
        [Display(Name = "گروه خبری")]
        public virtual ReportGroup? ReportGroup { get; set; }
    }
}
