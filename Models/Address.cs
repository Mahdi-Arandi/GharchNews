using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GharchNews.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [MaxLength(20)]
        [Display(Name ="تلفن")]
        public string? PhoneNumber { get; set; }

        [MaxLength(20)]
        [Display(Name = "فکس")]
        public string? Fax { get; set; }

        [MaxLength(20)]
        [Display(Name = "تلفن همراه")]
        public string? Mobile { get; set; }

        [MaxLength(100)]
        [Display(Name = "نشانی دفتر")]
        public string? Office { get; set; }

        [MaxLength(20)]
        [Display(Name = "کد پستی")]
        public string? PostalCode { get; set; }
    }
}
