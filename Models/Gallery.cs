using System.ComponentModel.DataAnnotations;

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
    }
}
