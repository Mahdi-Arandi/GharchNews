using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharchNews.Models
{
    public class Gallery
    {
        [Key]
        public int GalleryId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [Display(Name = "نام گالری")]
        [MaxLength(50)]
        public string GalleryName { get; set; } = default!;

        [Display(Name = "توضیح مختصر")]
        [MaxLength(300)]
        public string? Description { get; set; }
        public virtual List<Image>? Images { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsSuccess { get; set; } = false;


        [Display(Name = "آیدی کاربر")]
        public string? Id { get; set; }

        [ForeignKey("Id")]
        [Display(Name = "کاربر")]
        public virtual Users? Users { get; set; }
    }
}
