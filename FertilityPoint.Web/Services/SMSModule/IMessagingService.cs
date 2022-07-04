
using FertilityPoint.DTO.AppointmentModule;
using System.Threading.Tasks;

namespace FertilityPoint.Services.SMSModule
{
    public interface IMessagingService
    {
        Task<AppointmentDTO> ApprovalNotificationSMS(AppointmentDTO appointmentDTO);
    }
}