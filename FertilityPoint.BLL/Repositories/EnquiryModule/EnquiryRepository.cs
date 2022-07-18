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

        public async Task<SentMailDTO> Reply(SentMailDTO sentMailDTO)
        {
            try
            {
                sentMailDTO.CreateDate = DateTime.Now;

                var mail = mapper.Map<SentMail>(sentMailDTO);

                context.SentMails.Add(mail);

                await context.SaveChangesAsync();

                await UpdateEnquiryStatus(sentMailDTO.EnquiryId);

                return sentMailDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<bool> UpdateEnquiryStatus(Guid Id)
        {
            try
            {
                var enquiry = await context.Enquiries.FindAsync(Id);

                if (enquiry != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        enquiry.Status = 1;

                        transaction.Commit();
                    }
                    await context.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
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

        public async Task<EnquiryDTO> GetById(Guid Id)
        {
            try
            {
                var data = await context.Enquiries.FindAsync(Id);

                var enquiry = mapper.Map<Enquiry, EnquiryDTO>(data);

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
