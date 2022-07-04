

using FertilityPoint.DTO.ApplicationUserModule;
using FertilityPoint.DTO.AppointmentModule;
using FertilityPoint.DTO.EnquiryModule;
using System.Threading.Tasks;

namespace FertilityPoint.Services.EmailModule
{
    public interface IMailService
    {
        bool AppointmentEmailNotification(AppointmentDTO appointmentDTO);    
        Task<bool> FertilityPointEmailNotification(AppointmentDTO appointmentDTO);
        bool EnquiryNotification(EnquiryDTO enquiryDTO);
        bool PasswordResetEmailNotification(ResetPasswordDTO resetPasswordDTO);
        bool AccountEmailNotification(ApplicationUserDTO applicationUserDTO);
        bool AppointmentApprovalNotification(AppointmentDTO appointmentDTO);
    }
}