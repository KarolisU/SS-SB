using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Auth.Model
{
    public static class SSSBUserRoles
    {
        public const string Admin = nameof(Admin);
        public const string RegisteredUser = nameof(RegisteredUser);

        public static readonly IReadOnlyCollection<string> All = new[] { Admin, RegisteredUser };
    }
}
