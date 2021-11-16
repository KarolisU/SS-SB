using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Data.Dtos.Auth
{
    public class SSSBUser : IdentityUser/*<Guid>*/
    {
        [PersonalData]
        public string AdditionalInfo { get; set; }
    }
}
