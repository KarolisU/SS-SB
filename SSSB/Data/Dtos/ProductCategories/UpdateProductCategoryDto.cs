using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Data.Dtos.ProductCategories
{
    public record UpdateProductCategoryDto(
        [Required(ErrorMessage = "Name is required")] string Name,
        [Required(ErrorMessage = "Description is required")] string Description);
}
