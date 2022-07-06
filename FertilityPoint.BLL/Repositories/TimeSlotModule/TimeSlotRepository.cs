using AutoMapper;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.TimeSlotModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.TimeSlotModule
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public TimeSlotRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public async Task<TimeSlotDTO> Create(TimeSlotDTO timeSlotDTO)
        {
            try
            {
                timeSlotDTO.Id = Guid.NewGuid();

                timeSlotDTO.IsBooked = 0;

                timeSlotDTO.CreateDate = DateTime.Now;

                timeSlotDTO.UpdatedBy = timeSlotDTO.CreateBy;

                (Convert.ToDateTime(timeSlotDTO.FromTime)).ToString("HH:mm:ss tt");

                (Convert.ToDateTime(timeSlotDTO.ToTime)).ToString("HH:mm:ss tt");

                var slot = mapper.Map<TimeSlot>(timeSlotDTO);

                context.TimeSlots.Add(slot);

                await context.SaveChangesAsync();

                return timeSlotDTO;
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

                var slot = await context.TimeSlots.FindAsync(Id);

                if (slot != null)
                {
                    context.TimeSlots.Remove(slot);

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

        public async Task<List<TimeSlotDTO>> GetAll()
        {
            try
            {
                var data = await context.TimeSlots.ToListAsync();

                var slots = mapper.Map<List<TimeSlot>, List<TimeSlotDTO>>(data);

                return slots;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public TimeSlotDTO GetById(Guid Id)
        {
            try
            {
                var data = context.TimeSlots.Find(Id);

                var slot = mapper.Map<TimeSlotDTO>(data);

                return slot;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<TimeSlotDTO> Update(TimeSlotDTO timeSlotDTO)
        {
            try
            {
                var getData = await context.TimeSlots.FindAsync(timeSlotDTO.Id);

                if (getData != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        var data = getData;

                        mapper.Map(timeSlotDTO, data);

                        context.Entry(data).State = EntityState.Modified;

                        transaction.Commit();

                        await context.SaveChangesAsync();

                    }

                    return timeSlotDTO;
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
