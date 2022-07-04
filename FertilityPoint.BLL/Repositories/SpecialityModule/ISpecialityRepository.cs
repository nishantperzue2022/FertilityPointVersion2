

using FertilityPoint.DTO.SpecialityModule;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.SpecialityModule
{
    public interface ISpecialityRepository
    {
        Task<SpecialityDTO> Create(SpecialityDTO specialityDTO);
        Task<SpecialityDTO> Update(SpecialityDTO specialityDTO);
        Task<bool> Delete(Guid Id);
        Task<List<SpecialityDTO>> GetAll();
        Task<SpecialityDTO> GetById(Guid Id);
    }
}