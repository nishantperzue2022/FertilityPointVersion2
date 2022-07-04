using FertilityPoint.DTO.SubCountyModule;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.SubCountyModule
{
    public interface ISubCountyRepository
    {
        Task<SubCountyDTO> Create(SubCountyDTO subCountyDTO);
        Task<List<SubCountyDTO>> GetAll();
        Task<SubCountyDTO> GetById(Guid Id);
    }
}