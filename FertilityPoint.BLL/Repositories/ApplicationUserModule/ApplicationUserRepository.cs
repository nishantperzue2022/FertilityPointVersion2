using AutoMapper;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.ApplicationUserModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.ApplicationUserModule
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public ApplicationUserRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public async Task<List<RoleDTO>> GetAll()
        {
            try
            {
                var getRolse = await context.Roles.ToListAsync();

                var roles = new List<RoleDTO>();

                foreach (var item in getRolse)
                {
                    var data = new RoleDTO
                    {
                        Id = item.Id,

                        Name = item.Name,
                    };

                    roles.Add(data);
                }
                return roles;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<List<ApplicationUserDTO>> GetAllUsers()
        {
            try
            {
                var users = (from user in context.AppUsers

                             join userInRole in context.UserRoles on user.Id equals userInRole.UserId

                             join role in context.Roles on userInRole.RoleId equals role.Id

                             join speciality in context.Specialities on user.SpecialityId equals speciality.Id into r_join

                             from rEmpty in r_join.DefaultIfEmpty()

                             select new ApplicationUserDTO
                             {
                                 Id = user.Id,

                                 FirstName = user.FirstName,

                                 LastName = user.LastName,

                                 Email = user.Email,

                                 isActive = user.isActive,

                                 PhoneNumber = user.PhoneNumber,

                                 RoleName = role.Name,

                                 SpecialityName = rEmpty.Name,

                                 CreateDate = user.CreateDate,
                             }

                            ).OrderBy(x => x.CreateDate).ToListAsync();


                return await users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
        public async Task<ApplicationUserDTO> GetById(string Id)
        {
            try
            {
                var data = (from user in context.AppUsers

                             join userInRole in context.UserRoles on user.Id equals userInRole.UserId

                             join role in context.Roles on userInRole.RoleId equals role.Id

                             where user.Id == Id

                             select new ApplicationUserDTO
                             {
                                 Id = user.Id,

                                 FirstName = user.FirstName,

                                 LastName = user.LastName,

                                 Email = user.Email,

                                 isActive = user.isActive,

                                 PhoneNumber = user.PhoneNumber,

                                 RoleName = role.Name,                 

                                 SpecialityId = user.SpecialityId,

                                 CreateDate = user.CreateDate,
                             }
                            ).FirstOrDefaultAsync();


                return await data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> Delete(string Id)
        {
            try
            {
                bool result = false;

                var user = await context.AppUsers.FindAsync(Id);

                if (user != null)
                {
                    context.Remove(user);

                    await context.SaveChangesAsync();

                    return true;
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public async Task<bool> DisableAccount(string Id)
        {
            try
            {
                bool result = false;

                var user = await context.AppUsers.FindAsync(Id);

                if (user != null)
                {

                    using (var transaction = context.Database.BeginTransaction())
                    {
                        user.isActive = false;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return true;
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public async Task<bool> EnableAccount(string Id)
        {
            try
            {
                bool result = false;

                var user = await context.AppUsers.FindAsync(Id);

                if (user != null)
                {

                    using (var transaction = context.Database.BeginTransaction())
                    {
                        user.isActive = true;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return true;
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public async Task<ApplicationUserDTO> Update(ApplicationUserDTO applicationUserDTO)
        {
            try
            {
                var user = await context.AppUsers.FindAsync(applicationUserDTO.Id);

                if (user != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        user.FirstName = applicationUserDTO.FirstName.Substring(0, 1).ToUpper() + applicationUserDTO.FirstName.Substring(1).ToLower().Trim();

                        user.LastName = applicationUserDTO.LastName.Substring(0, 1).ToUpper() + applicationUserDTO.LastName.Substring(1).ToLower().Trim();

                        user.PhoneNumber = applicationUserDTO.PhoneNumber;

                        user.Email = applicationUserDTO.Email.ToLower();

                        user.UserName = applicationUserDTO.Email.ToLower();

                        user.NormalizedEmail = applicationUserDTO.Email.ToUpper();

                        user.NormalizedUserName = applicationUserDTO.Email.ToUpper();

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }
                    return applicationUserDTO;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
