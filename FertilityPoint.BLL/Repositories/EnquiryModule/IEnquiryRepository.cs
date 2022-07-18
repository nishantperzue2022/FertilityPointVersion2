using FertilityPoint.DTO.EnquiryModule;

namespace FertilityPoint.BLL.Repositories.EnquiryModule
{
    public interface IEnquiryRepository
    {
        Task<EnquiryDTO> Create(EnquiryDTO enquiryDTO);
        Task<List<EnquiryDTO>> GetAll();
        Task<EnquiryDTO> GetById(Guid Id);
        Task<SentMailDTO> Reply(SentMailDTO sentMailDTO);
    }
}