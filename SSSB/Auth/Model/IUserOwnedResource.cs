using SSSB.Data.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Auth.Model
{
    public interface IUserOwnedResource
    {
        public string UserId { get;/* set;*/ }
        //public SSSBUser User { get; set; }
    }
}
