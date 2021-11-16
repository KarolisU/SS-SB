using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Data.Dtos.Advertisements
{
    public record UpdateAdvertisementDto(string Name,
        [Required(ErrorMessage = "Description text is required")] string Description,
        [Required(ErrorMessage = "Price is required")] int Price,
        [Required(ErrorMessage = "Phone number is required")][RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{9}$", ErrorMessage = "Please enter valid phone number")] string PhoneNumber);
}
