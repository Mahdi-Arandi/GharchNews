using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharchNews.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [Display(Name = "نام تصویر")]
        [MaxLength(50)]
        public string? ImageName { get; set; } = default!;

        [Display(Name = "زمان درج")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "نام گالری")]
        public int GalleryId { get; set; }

        [ForeignKey("GalleryId")]
        [Display(Name = "نام گالری")]
        public virtual Gallery? Gallery { get; set; }
    }
}
