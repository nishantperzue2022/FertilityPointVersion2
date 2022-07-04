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

                             join speciality in context.Specialities on user.SpecialityId equals speciality.Id

                             select new ApplicationUserDTO
                             {
                                 Id = user.Id,

                                 FirstName = user.FirstName,

                                 LastName = user.LastName,

                                 Email = user.Email,

                                 isActive = user.isActive,

                                 PhoneNumber = user.PhoneNumber,

                                 RoleName = role.Name,

                                 SpecialityName = speciality.Name,

                                 CreateDate = user.CreateDate,
                             }

                            ).OrderBy(x=>x.CreateDate).ToListAsync();


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
                var users = (from user in context.AppUsers

                             join userInRole in context.UserRoles on user.Id equals userInRole.UserId

                             join role in context.Roles on userInRole.RoleId equals role.Id

                             join speciality in context.Specialities on user.SpecialityId equals speciality.Id

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

                                 SpecialityName = speciality.Name,

                                 CreateDate = user.CreateDate,
                             }
                            ).FirstOrDefaultAsync();


                return await users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
