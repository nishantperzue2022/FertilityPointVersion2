using FertilityPoint.DTO.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.AppointmentModule
{
    public interface IAppointmentRepository
    {
        Task<List<AppointmentDTO>> GetAll();
        Task<AppointmentDTO> GetById(Guid Id);
        Task<AppointmentDTO> Create(AppointmentDTO appointmentDTO);
        AppointmentDTO GetTransaction(Guid Id);
        Task<bool> ApproveAppointment(AppointmentDTO appointmentDTO);
    }
}