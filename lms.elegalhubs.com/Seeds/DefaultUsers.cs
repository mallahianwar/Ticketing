using library.elegalhubs.com.lms.Admin;
using lms.elegalhubs.com.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new Users
            {
                UserName = "basicuser@gmail.com",
                Email = "basicuser@gmail.com",
                FullName = "basic",
                PhoneNumber = "059",
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                }
            }
        }
        public static async Task SeedEmployeeUserAsync(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new Users
            {
                UserName = "amallahee@gmail.com",
                Email = "amallahee@gmail.com",
                FullName = "An",
                PhoneNumber = "059",
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "An@1995#SS");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Employee.ToString());
                }
            }
        }
        public static async Task SeedSuperAdminAsync(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new Users
            {
                UserName = "superadmin@gmail.com",
                Email = "superadmin@gmail.com",
                FullName = "super",
                PhoneNumber = "059",
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }
        private async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
            //await roleManager.AddPermissionClaim(adminRole, "Products");
            //await roleManager.AddPermissionClaim(adminRole, "Chat");
            //await roleManager.AddPermissionClaim(adminRole, "Users");
            //await roleManager.AddPermissionClaim(adminRole, "Projects");
            //await roleManager.AddPermissionClaim(adminRole, "Departments");
            //await roleManager.AddPermissionClaim(adminRole, "Tickets");
        }
        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
    }
}
