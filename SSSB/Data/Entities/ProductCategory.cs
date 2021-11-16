using SSSB.Auth.Model;
using SSSB.Data.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Data.Entities
{
    public class ProductCategory : IUserOwnedResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string UserId { get; set; }
        //public SSSBUser User { get; set; }
    }
}
