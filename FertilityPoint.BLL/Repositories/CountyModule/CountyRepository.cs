using AutoMapper;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.CountyModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.CountyModule
{
   public class CountyRepository : ICountyRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public CountyRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;
        }

        public async Task<CountyDTO> Create(CountyDTO countyDTO)
        {
            try
            {
                County county = mapper.Map<County>(countyDTO);

                context.Counties.Add(county);

                await context.SaveChangesAsync();

                return countyDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<List<CountyDTO>> GetAll()
        {
            try
            {
                var data = await context.Counties.ToListAsync();

                var counties = mapper.Map<List<County>, List<CountyDTO>>(data);

                return counties;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }

        public async Task<CountyDTO> GetById(Guid Id)
        {
            try
            {
                var data = await context.Counties.FindAsync(Id);

                var counties = mapper.Map<CountyDTO>(data);

                return counties;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
    }
}
