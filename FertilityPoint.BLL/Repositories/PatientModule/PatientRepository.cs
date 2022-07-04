using AutoMapper;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.AppointmentModule;
using FertilityPoint.DTO.PatientModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.PatientModule
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public PatientRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public async Task<PatientDTO> Create(PatientDTO patientDTO)
        {
            try
            {
                var patient = mapper.Map<Patient>(patientDTO);

                context.Patients.Add(patient);

                await context.SaveChangesAsync();

                return patientDTO;
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

                var patient = await context.Patients.FindAsync(Id);

                if (patient != null)
                {
                    context.Patients.Remove(patient);

                    await context.SaveChangesAsync();

                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public async Task<List<PatientDTO>> GetAll()
        {
            try
            {
                var data = await context.Patients.ToListAsync();

                var patient = mapper.Map<List<Patient>, List<PatientDTO>>(data);

                return patient;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
        public async Task<PatientDTO> GetById(Guid Id)
        {
            try
            {
                var data = await context.Patients.FindAsync(Id);

                var patient = mapper.Map<PatientDTO>(data);

                return patient;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
        public Task<PatientDTO> Update(PatientDTO patientDTO)
        {
            throw new NotImplementedException();
        }
        
    }
}
