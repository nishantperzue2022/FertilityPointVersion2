using FertilityPoint.DTO.ApplicationUserModule;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.ApplicationUserModule
{
    public interface IApplicationUserRepository
    {
        Task<List<RoleDTO>> GetAll();
        Task<List<ApplicationUserDTO>> GetAllUsers();
        Task<ApplicationUserDTO> GetById(string Id);
    }
}