using System.ComponentModel.DataAnnotations;

namespace GharchNews.Models
{
    public class Ads
    {
        [Key]
        public int AdsId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50)]
        [Display(Name = "نام شرکت / فرد")]
        public string? AdsName { get; set; }

        [MaxLength(50)]
        [Display(Name = "ایمیل")]
        public string? Email { get; set; }

        [MaxLength(20)]
        [Display(Name = "شماره تماس")]
        public string? PhoneNumber { get; set; }

        [MaxLength(150)]
        [Display(Name = "موضوع تبلیغات")]
        public string? Subject { get; set; }

        [Display(Name = "هزینه")]
        public int? Price { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تاریخ درج")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Display(Name = "تاریخ انقضاء")]
        public DateTime ExpireDate { get; set; }

        [MaxLength(200)]
        [Display(Name = "تصویر تبلیغ")]
        public string? Image { get; set; }
    }
}
