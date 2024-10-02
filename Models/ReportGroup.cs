using System.ComponentModel.DataAnnotations;

namespace GharchNews.Models
{
    public class ReportGroup
    {
        [Key]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "لطفا نام {0} را وارد کنید")]
        [MaxLength(100)]
        [Display(Name = "گروه خبری")]
        public string GroupName { get; set; } = default!;

        [MaxLength(100)]
        [Display(Name = "تصویر گروه")]
        public string? GroupImage { get; set; } = default!;

        public virtual List<Report>? Reports { get; set; } = default!;
    }
}
