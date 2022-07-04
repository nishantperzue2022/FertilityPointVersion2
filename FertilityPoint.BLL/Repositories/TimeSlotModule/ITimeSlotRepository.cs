using FertilityPoint.DTO.TimeSlotModule;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.TimeSlotModule
{
    public interface ITimeSlotRepository
    {
        Task<TimeSlotDTO> Create(TimeSlotDTO timeSlotDTO);
        Task<TimeSlotDTO> Update(TimeSlotDTO timeSlotDTO);
        Task<bool> Delete(Guid Id);    
        Task<List<TimeSlotDTO>> GetAll();
        TimeSlotDTO GetById(Guid Id);
    }
}