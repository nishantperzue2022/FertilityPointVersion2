using AutoMapper;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.EnquiryModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.EnquiryModule
{
    public class EnquiryRepository : IEnquiryRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public EnquiryRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;

            this.mapper = mapper;
        }

        public async Task<EnquiryDTO> Create(EnquiryDTO enquiryDTO)
        {
            try
            {
                enquiryDTO.CreateDate = DateTime.Now;

                var enquiry = mapper.Map<Enquiry>(enquiryDTO);

                context.Enquiries.Add(enquiry);

                await context.SaveChangesAsync();

                return enquiryDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<List<EnquiryDTO>> GetAll()
        {
            try
            {
                var data = await context.Enquiries.ToListAsync();

                var enquiry = mapper.Map<List<Enquiry>, List<EnquiryDTO>>(data);

                return enquiry;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
