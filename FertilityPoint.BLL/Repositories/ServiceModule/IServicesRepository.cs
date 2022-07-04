
using FertilityPoint.DTO.ServiceModule;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.ServiceModule
{
    public interface IServicesRepository
    {
        Task<ServiceDTO> Create(ServiceDTO serviceDTO);
        Task<ServiceDTO> Update(ServiceDTO serviceDTO);
        Task<ServiceDTO> GetById(Guid Id);
        Task<ServiceDTO> GetDefault();
        Task<List<ServiceDTO>> GetAll();
        Task<bool> Delete(Guid Id);
    }
}