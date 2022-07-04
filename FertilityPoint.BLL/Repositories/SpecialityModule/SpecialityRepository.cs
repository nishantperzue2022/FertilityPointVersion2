


using AutoMapper;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.SpecialityModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.SpecialityModule
{
    public class SpecialityRepository : ISpecialityRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public SpecialityRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public async Task<SpecialityDTO> Create(SpecialityDTO specialityDTO)
        {
            try
            {
                specialityDTO.CreateDate = DateTime.Now; 
                
                var speciality = mapper.Map<Speciality>(specialityDTO);

                context.Specialities.Add(speciality);

                await context.SaveChangesAsync();

                return specialityDTO;
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

                var speciality = await context.Specialities.FindAsync(Id);

                if (speciality != null)
                {
                    context.Specialities.Remove(speciality);

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

        public async Task<List<SpecialityDTO>> GetAll()
        {
            try
            {
                var data = await context.Specialities.ToListAsync();

                var speciality = mapper.Map<List<Speciality>, List<SpecialityDTO>>(data);

                return speciality;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }

        public async Task<SpecialityDTO> GetById(Guid Id)
        {
            try
            {
                var data = await context.Specialities.FindAsync(Id);

                var speciality = mapper.Map<SpecialityDTO>(data);

                return speciality;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }


        public async Task<SpecialityDTO> Update(SpecialityDTO specialityDTO)
        {
            try
            {
                var getData = await context.Specialities.FindAsync(specialityDTO.Id);

                if (getData != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        var data = getData;

                        mapper.Map(specialityDTO, data);

                        context.Entry(data).State = EntityState.Modified;

                        transaction.Commit();

                        await context.SaveChangesAsync();

                    }

                    return specialityDTO;
                }

                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
