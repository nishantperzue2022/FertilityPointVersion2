using AutoMapper;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.ServiceModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.ServiceModule
{
    public class ServicesRepository : IServicesRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;

        public ServicesRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;
        }

        public async Task<ServiceDTO> Create(ServiceDTO serviceDTO)
        {
            try
            {
                var package = mapper.Map<Service>(serviceDTO);

                context.Services.Add(package);

                await context.SaveChangesAsync();

                return serviceDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                bool result = false;

                var package = await context.Services.FindAsync(Id);

                if (package != null)
                {
                    context.Services.Remove(package);

                    await context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public async Task<ServiceDTO> GetById(Guid Id)
        {
            try
            {
                var data = await context.Services.FindAsync(Id);

                var package = mapper.Map<ServiceDTO>(data);

                return package;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }     
        
        public async Task<ServiceDTO> GetDefault()
        {
            try
            {
                var data = await context.Services.FirstOrDefaultAsync();

                var package = mapper.Map<ServiceDTO>(data);

                return package;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<ServiceDTO> Update(ServiceDTO serviceDTO)
        {
            try
            {
                var getData = await context.Services.FindAsync(serviceDTO.Id);

                if (getData != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        var data = getData;

                        mapper.Map(serviceDTO, data);

                        context.Entry(data).State = EntityState.Modified;

                        transaction.Commit();

                        await context.SaveChangesAsync();

                    }

                    return serviceDTO;
                }

                return null;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<List<ServiceDTO>> GetAll()
        {
            try
            {
                var data = await context.Services.ToListAsync();

                var package = mapper.Map<List<Service>, List<ServiceDTO>>(data);

                return package;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}

