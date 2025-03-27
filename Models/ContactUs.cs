using System.ComponentModel.DataAnnotations;

namespace GharchNews.Models
{
    public class ContactUs
    {
        [Key]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50)]
        [Display(Name = "نام")]
        public string Name { get; set; } = default!;

        [MaxLength(50)]
        [Display(Name = "ایمیل")]
        public string? Email { get; set; }

        [MaxLength(20)]
        [Display(Name = "شماره تماس")]
        public string? PhoneNumber { get; set; }

        [MaxLength(250)]
        [Display(Name = "موضوع")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "پیام")]
        public string Message { get; set; } = default!;

        [Display(Name = "زمان درج")]
        public DateTime CreateDate { get; set; }
    }
}
