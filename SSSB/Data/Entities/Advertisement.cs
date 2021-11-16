using SSSB.Auth.Model;
using SSSB.Data.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Data.Entities
{
    public class Advertisement : IUserOwnedResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        //[Required(ErrorMessage = "Mobile no. is required")]
        //[RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        public string PhoneNumber { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        [Required]
        public string UserId { get; set; }
        //public SSSBUser User { get; set; }
    }
}
