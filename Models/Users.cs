using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GharchNews.Models
{
    public class Users: IdentityUser
    {
        [Display(Name ="نام کاربری")]
        public override string? UserName { get => base.UserName; set => base.UserName = value; }

        [MaxLength(50)]
        [Display(Name = "نام")]
        public string? Name { get; set; }

        [MaxLength(50)]
        [Display(Name = "نام خانوادگی")]
        public string? Family { get; set; }

        [MaxLength(50)]
        [Display(Name = "شماره تماس")]
        public override string? PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        [MaxLength(50)]
        [Display(Name = "عکس پروفایل")]
        public string? ProfileImage { get; set; }

        [MaxLength(50)]
        [Display(Name = "نقش کاربر")]
        public string InRole { get; set; }

        public virtual List<Report>? Reports { get; set; }
        public virtual List<Image>? Images { get; set; }
        public virtual List<Gallery>? Galleries { get; set; }
        public virtual List<Links>? Links { get; set; }
        public virtual List<Hashtag>? Hashtags { get; set; }
    }
}
