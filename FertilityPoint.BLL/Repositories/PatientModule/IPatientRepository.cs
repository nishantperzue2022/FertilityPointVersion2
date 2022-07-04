using FertilityPoint.DTO.AppointmentModule;
using FertilityPoint.DTO.PatientModule;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.PatientModule
{
    public interface IPatientRepository
    {
        Task<PatientDTO> Create(PatientDTO patientDTO);
        Task<PatientDTO> Update(PatientDTO patientDTO);
        Task<bool> Delete(Guid Id);
        Task<PatientDTO> GetById(Guid Id);
        Task<List<PatientDTO>> GetAll();
     
    }
}