using FertilityPoint.DTO.CountyModule;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.CountyModule
{
    public interface ICountyRepository
    {
        Task<CountyDTO> Create(CountyDTO countyDTO);
        Task<List<CountyDTO>> GetAll();
        Task<CountyDTO> GetById(Guid Id);
    }
}