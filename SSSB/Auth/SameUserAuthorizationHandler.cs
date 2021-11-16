using Microsoft.AspNetCore.Authorization;
using SSSB.Auth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Auth
{
    public class SameUserAuthorizationHandler : AuthorizationHandler<SameUserRequirement, IUserOwnedResource>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameUserRequirement requirement, IUserOwnedResource resource)
        {
            //var a = context.User.FindFirst(CustomClaims.UserId).Value;
            ////var b = resource.UserId;
            //var c = 5;
            if (context.User.IsInRole(SSSBUserRoles.Admin) || context.User.FindFirst(CustomClaims.UserId).Value == resource/*.User.Id*/.UserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public record SameUserRequirement : IAuthorizationRequirement;
}
