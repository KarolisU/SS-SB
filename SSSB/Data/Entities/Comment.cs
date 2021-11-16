using SSSB.Auth.Model;
using SSSB.Data.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Data.Entities
{
    public class Comment : IUserOwnedResource
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public int Rating { get; set; }
        //public int ProductCategoryId { get; set; }
        //public ProductCategory ProductCategory { get; set; }
        public int AdvertisementId { get; set; }
        public Advertisement Advertisement { get; set; }

        [Required]
        public string UserId { get; set; }
        //public SSSBUser User { get; set; }

    }
}
