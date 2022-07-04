using AutoMapper;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.SubCountyModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.SubCountyModule
{
    public class SubCountyRepository : ISubCountyRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;

        public SubCountyRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public async Task<SubCountyDTO> Create(SubCountyDTO subCountyDTO)
        {
            try
            {
                subCountyDTO.Id = Guid.NewGuid();

                subCountyDTO.CreateDate = DateTime.Now;

                var subcounty = mapper.Map<SubCounty>(subCountyDTO);

                context.SubCounties.Add(subcounty);

                await context.SaveChangesAsync();

                return subCountyDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<List<SubCountyDTO>> GetAll()
        {
            try
            {
                var data = await context.SubCounties.ToListAsync();

                var subcounties = mapper.Map<List<SubCounty>, List<SubCountyDTO>>(data);

                return subcounties;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<SubCountyDTO> GetById(Guid Id)
        {
            try
            {
                var data = await context.SubCounties.FindAsync(Id);

                var sub_county = mapper.Map<SubCountyDTO>(data);

                return sub_county;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
    }
}
