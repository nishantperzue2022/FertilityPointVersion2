
using FertilityPoint.DAL.Modules;
using Microsoft.AspNetCore.Identity;
using System;

namespace FertilityPoint.SeedAppUsers
{
    public static class SeedUsers
    {
        public static void Seed(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            CreateRoles(roleManager);

            CreateUsers(userManager);
        }

        private static void CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                if (!roleManager.RoleExistsAsync("Admin").Result)
                {
                    var role = new IdentityRole
                    {
                        Name = "Admin"
                    };

                    roleManager.CreateAsync(role).Wait();
                }

                if (!roleManager.RoleExistsAsync("SuperAdmin").Result)
                {
                    var role = new IdentityRole
                    {
                        Name = "SuperAdmin"
                    };

                    roleManager.CreateAsync(role).Wait();
                }


                if (!roleManager.RoleExistsAsync("Agent").Result)
                {
                    var role = new IdentityRole
                    {
                        Name = "Doctor"
                    };

                    roleManager.CreateAsync(role);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void CreateUsers(UserManager<AppUser> userManager)
        {
            try
            {
                #region Admin
                var admin = userManager.FindByEmailAsync("admin@gmail.com");

                if (admin.Result == null)
                {
                    var user = new AppUser
                    {
                        UserName = "admin@gmail.com",

                        Email = "admin@gmail.com",

                        PhoneNumber = "0704509484",

                        FirstName = "Alex",

                        LastName = "Jobs",

                        EmailConfirmed = true,

                        isActive = true,

                        CreateDate = DateTime.Now
                    };

                    string userPWD = "Admin@2022";

                    var chkUser = userManager.CreateAsync(user, userPWD);

                    //Add default User to Role Admin    
                    if (chkUser.Result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();

                    }
                }
                #endregion

                #region Doctor
                var doctor = userManager.FindByEmailAsync("doctor@gmail.com");

                if (doctor.Result == null)
                {
                    var user = new AppUser
                    {
                        UserName = "doctor@gmail.com",

                        Email = "doctor@gmail.com",

                        PhoneNumber = "0704509484",

                        FirstName = "Agent",

                        LastName = "Steve",

                        EmailConfirmed = true,

                        isActive = true,

                        CreateDate = DateTime.Now
                    };

                    string userPWD = "Doctor@2022";

                    var chkUser = userManager.CreateAsync(user, userPWD);

                    //Add default User to Role Admin    
                    if (chkUser.Result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Doctor").Wait();

                    }
                }
                #endregion


                #region superadmin
                var superadmin = userManager.FindByEmailAsync("peterspecimen@gmail.com");

                if (superadmin.Result == null)
                {
                    var user = new AppUser
                    {
                        UserName = "peterspecimen@gmail.com",

                        Email = "peterspecimen@gmail.com",

                        PhoneNumber = "0704509484",

                        FirstName = "Peter",

                        LastName = "Kachezi",

                        EmailConfirmed = true,

                        isActive = true,

                        CreateDate = DateTime.Now
                    };

                    string userPWD = "Super@2022";

                    var chkUser = userManager.CreateAsync(user, userPWD);

                    //Add default User to Role Admin    
                    if (chkUser.Result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "SuperAdmin").Wait();

                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

    }
}
