using Microsoft.AspNetCore.Identity;
using SSSB.Auth.Model;
using SSSB.Data.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSB.Data
{
    public class DatabaseSeeder
    {
        private readonly UserManager<SSSBUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseSeeder(UserManager<SSSBUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            foreach (var role in SSSBUserRoles.All)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var newAdminUser = new SSSBUser
            {
                UserName = "admin1",
                Email = "admin1@admin1.com"
            };

            var newAdminUser2 = new SSSBUser
            {
                UserName = "admin2",
                Email = "admin2@admin2.com"
            };

            var existingAdminUser = await _userManager.FindByNameAsync(newAdminUser.UserName);
            if (existingAdminUser == null)
            {
                var createAdminUserResult = await _userManager.CreateAsync(newAdminUser, "VerySafePassword1!");
                if (createAdminUserResult.Succeeded)
                {
                    await _userManager.AddToRolesAsync(newAdminUser, SSSBUserRoles.All);
                }
            }

            var existingAdminUser2 = await _userManager.FindByNameAsync(newAdminUser2.UserName);
            if (existingAdminUser2 == null)
            {
                var createAdminUserResult = await _userManager.CreateAsync(newAdminUser2, "VerySafePassword2!");
                if (createAdminUserResult.Succeeded)
                {
                    await _userManager.AddToRolesAsync(newAdminUser2, SSSBUserRoles.All);
                }
            }
        }
    }
}
