
using FertilityPoint.BLL.Repositories.CountyModule;
using FertilityPoint.BLL.Repositories.SubCountyModule;
using FertilityPoint.BLL.Repositories.TimeSlotModule;
using FertilityPoint.DTO.ApplicationUserModule;
using FertilityPoint.DTO.AppointmentModule;
using FertilityPoint.DTO.EnquiryModule;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FertilityPoint.Services.EmailModule
{
    public class MailService : IMailService
    {
        private readonly IConfiguration config;

        private readonly IWebHostEnvironment env;     

  
        public MailService(IConfiguration config, IWebHostEnvironment env)
        {
            this.config = config;

            this.env = env;

        }
        public bool AccountEmailNotification(ApplicationUserDTO applicationUserDTO)
        {
            try
            {
                var SMTPEmailToNetwork = config.GetValue<string>("MailSettings:SMTPEmailToNetwork");

                var SMTPMailServer = config.GetValue<string>("MailSettings:SMTPMailServer");

                var SMTPPort = config.GetValue<string>("MailSettings:SMTPPort");

                var SMTPUserName = config.GetValue<string>("MailSettings:SMTPUserName");

                var Password = config.GetValue<string>("MailSettings:Password");

                var SMTPUseSSL = config.GetValue<string>("MailSettings:SMTPUseSSL");

                MailAddressCollection mailAddressesTo = new MailAddressCollection();

                mailAddressesTo.Add(new MailAddress(applicationUserDTO.Email));

                MailAddress mailAddressFrom = new MailAddress(SMTPUserName);

                MailMessage mailMessage = new MailMessage();

                mailMessage.From = mailAddressFrom;

                foreach (var to in mailAddressesTo)
                    mailMessage.To.Add(to);

                mailMessage.Subject = "Fertility Point: ";

                var templatePath = env.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "Templates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplate"
                           + Path.DirectorySeparatorChar.ToString()
                           + "UserAccountNotification.html";

                var builder = new BodyBuilder();

                using (StreamReader SourceReader = System.IO.File.OpenText(templatePath))
                {

                    builder.HtmlBody = SourceReader.ReadToEnd();

                }

                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                mailMessage.Body = string.Format(builder.HtmlBody,

                     applicationUserDTO.FullName,

                     applicationUserDTO.Email,

                     applicationUserDTO.Password,

                     applicationUserDTO.RoleName

                    );

                mailMessage.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = SMTPMailServer;
                    client.Port = int.Parse(SMTPPort);
                    if (SMTPUseSSL != string.Empty)
                    {
                        client.EnableSsl = bool.Parse(SMTPUseSSL);
                    }

                    client.UseDefaultCredentials = false;
                    bool bNetwork = bool.Parse(SMTPEmailToNetwork);
                    if (bNetwork)
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    }
                    else
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    }

                    client.Credentials = new NetworkCredential(SMTPUserName, Password);

                    client.ServicePoint.MaxIdleTime = 2;

                    client.ServicePoint.ConnectionLimit = 1;

                    client.Send(mailMessage);
                }

                return true;

            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public bool AppointmentEmailNotification(AppointmentDTO appointmentDTO)
        {
            try
            {
                var SMTPEmailToNetwork = config.GetValue<string>("MailSettings:SMTPEmailToNetwork");

                var SMTPMailServer = config.GetValue<string>("MailSettings:SMTPMailServer");

                var SMTPPort = config.GetValue<string>("MailSettings:SMTPPort");

                var SMTPUserName = config.GetValue<string>("MailSettings:SMTPUserName");

                var Password = config.GetValue<string>("MailSettings:Password");

                var SMTPUseSSL = config.GetValue<string>("MailSettings:SMTPUseSSL");

                MailAddressCollection mailAddressesTo = new MailAddressCollection
                {
                    new MailAddress(appointmentDTO.Email)
                };

                MailAddress mailAddressFrom = new MailAddress(SMTPUserName);

                MailMessage mailMessage = new MailMessage
                {
                    From = mailAddressFrom
                };

                foreach (var to in mailAddressesTo)
                    mailMessage.To.Add(to);

                mailMessage.Subject = "Fertility Point: ";

                var templatePath = env.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "Templates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplate"
                           + Path.DirectorySeparatorChar.ToString()
                           + "AppointmentNotification.html";

                var builder = new BodyBuilder();

                using (StreamReader SourceReader = File.OpenText(templatePath))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }

                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                mailMessage.Body = string.Format(builder.HtmlBody,

                     appointmentDTO.FullName,

                     appointmentDTO.AppointmentDate.ToShortDateString(),

                     appointmentDTO.TimeSlot,

                     appointmentDTO.ReceiptURL

                    );

                mailMessage.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = SMTPMailServer;
                    client.Port = int.Parse(SMTPPort);
                    if (SMTPUseSSL != string.Empty)
                    {
                        client.EnableSsl = bool.Parse(SMTPUseSSL);
                    }

                    client.UseDefaultCredentials = false;
                    bool bNetwork = bool.Parse(SMTPEmailToNetwork);
                    if (bNetwork)
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    }
                    else
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    }

                    client.Credentials = new NetworkCredential(SMTPUserName, Password);

                    client.ServicePoint.MaxIdleTime = 2;

                    client.ServicePoint.ConnectionLimit = 1;

                    client.Send(mailMessage);
                }

                return true;

            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public bool EnquiryNotification(EnquiryDTO enquiryDTO)
        {
            try
            {
                var SMTPEmailToNetwork = config.GetValue<string>("MailSettings:SMTPEmailToNetwork");

                var SMTPMailServer = config.GetValue<string>("MailSettings:SMTPMailServer");

                var SMTPPort = config.GetValue<string>("MailSettings:SMTPPort");

                var SMTPUserName = config.GetValue<string>("MailSettings:SMTPUserName");

                var Password = config.GetValue<string>("MailSettings:Password");

                var SMTPUseSSL = config.GetValue<string>("MailSettings:SMTPUseSSL");

                enquiryDTO.FertilityEmail = config.GetValue<string>("FertilityPoint:Email");

                MailAddressCollection mailAddressesTo = new MailAddressCollection
                {
                    new MailAddress(enquiryDTO.FertilityEmail)
                };

                MailAddress mailAddressFrom = new MailAddress(SMTPUserName);

                MailMessage mailMessage = new MailMessage
                {
                    From = mailAddressFrom
                };

                foreach (var to in mailAddressesTo)
                    mailMessage.To.Add(to);


                mailMessage.Subject = enquiryDTO.Subject;

                var templatePath = env.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "Templates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplate"
                           + Path.DirectorySeparatorChar.ToString()
                           + "EnquiryNotification.html";

                var builder = new BodyBuilder();

                using (StreamReader SourceReader = File.OpenText(templatePath))
                {

                    builder.HtmlBody = SourceReader.ReadToEnd();

                }

                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                mailMessage.Body = string.Format(builder.HtmlBody,

                     enquiryDTO.Name,

                     enquiryDTO.Message

                    );

                mailMessage.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = SMTPMailServer;

                    client.Port = int.Parse(SMTPPort);

                    if (SMTPUseSSL != string.Empty)
                    {
                        client.EnableSsl = bool.Parse(SMTPUseSSL);
                    }

                    client.UseDefaultCredentials = false;

                    bool bNetwork = bool.Parse(SMTPEmailToNetwork);

                    if (bNetwork)
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    }
                    else
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    }

                    client.Credentials = new NetworkCredential(SMTPUserName, Password);

                    client.ServicePoint.MaxIdleTime = 2;

                    client.ServicePoint.ConnectionLimit = 1;

                    client.Send(mailMessage);
                }

                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public Task<bool> FertilityPointEmailNotification(AppointmentDTO appointmentDTO)
        {
            try
            {
                var SMTPEmailToNetwork = config.GetValue<string>("MailSettings:SMTPEmailToNetwork");

                var SMTPMailServer = config.GetValue<string>("MailSettings:SMTPMailServer");

                var SMTPPort = config.GetValue<string>("MailSettings:SMTPPort");

                var SMTPUserName = config.GetValue<string>("MailSettings:SMTPUserName");

                var Password = config.GetValue<string>("MailSettings:Password");

                var SMTPUseSSL = config.GetValue<string>("MailSettings:SMTPUseSSL");

                appointmentDTO.FertilityEmail = config.GetValue<string>("FertilityPoint:Email");

                MailAddressCollection mailAddressesTo = new MailAddressCollection
                {
                    new MailAddress(appointmentDTO.FertilityEmail)
                };

                MailAddress mailAddressFrom = new MailAddress(SMTPUserName);

                MailMessage mailMessage = new MailMessage
                {
                    From = mailAddressFrom
                };

                foreach (var to in mailAddressesTo)
                    mailMessage.To.Add(to);


                mailMessage.Subject = "Appointment for " + appointmentDTO.FirstName + " : ";

                var templatePath = env.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "Templates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplate"
                           + Path.DirectorySeparatorChar.ToString()
                           + "FertilityPointNotification.html";

                var builder = new BodyBuilder();

                using (StreamReader SourceReader = File.OpenText(templatePath))
                {

                    builder.HtmlBody = SourceReader.ReadToEnd();

                }

                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                mailMessage.Body = string.Format(builder.HtmlBody,

                     appointmentDTO.FullName,

                     appointmentDTO.PhoneNumber,

                     appointmentDTO.Email,

                     appointmentDTO.AppointmentDate.ToShortDateString()

                    );

                mailMessage.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = SMTPMailServer;

                    client.Port = int.Parse(SMTPPort);

                    if (SMTPUseSSL != string.Empty)
                    {
                        client.EnableSsl = bool.Parse(SMTPUseSSL);
                    }

                    client.UseDefaultCredentials = false;

                    bool bNetwork = bool.Parse(SMTPEmailToNetwork);

                    if (bNetwork)
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    }
                    else
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    }

                    client.Credentials = new NetworkCredential(SMTPUserName, Password);

                    client.ServicePoint.MaxIdleTime = 2;

                    client.ServicePoint.ConnectionLimit = 1;

                    client.Send(mailMessage);
                }

                return Task.FromResult(true);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Task.FromResult(false);
            }
        }
        public bool PasswordResetEmailNotification(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var SMTPEmailToNetwork = config.GetValue<string>("MailSettings:SMTPEmailToNetwork");

                var SMTPMailServer = config.GetValue<string>("MailSettings:SMTPMailServer");

                var SMTPPort = config.GetValue<string>("MailSettings:SMTPPort");

                var SMTPUserName = config.GetValue<string>("MailSettings:SMTPUserName");

                var Password = config.GetValue<string>("MailSettings:Password");

                var SMTPUseSSL = config.GetValue<string>("MailSettings:SMTPUseSSL");

                MailAddressCollection mailAddressesTo = new MailAddressCollection();

                mailAddressesTo.Add(new MailAddress(resetPasswordDTO.Email));

                MailAddress mailAddressFrom = new MailAddress(SMTPUserName);

                MailMessage mailMessage = new MailMessage();

                mailMessage.From = mailAddressFrom;

                foreach (var to in mailAddressesTo)
                    mailMessage.To.Add(to);

                mailMessage.Subject = "Healthier Kenya: ";

                var templatePath = env.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "Templates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplate"
                           + Path.DirectorySeparatorChar.ToString()
                           + "PaswordResetNotification.html";

                var builder = new BodyBuilder();

                using (StreamReader SourceReader = System.IO.File.OpenText(templatePath))
                {

                    builder.HtmlBody = SourceReader.ReadToEnd();

                }

                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                mailMessage.Body = string.Format(builder.HtmlBody,

                     resetPasswordDTO.FullName,

                     resetPasswordDTO.Email,

                     resetPasswordDTO.Password

                    );

                mailMessage.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = SMTPMailServer;
                    client.Port = int.Parse(SMTPPort);
                    if (SMTPUseSSL != string.Empty)
                    {
                        client.EnableSsl = bool.Parse(SMTPUseSSL);
                    }

                    client.UseDefaultCredentials = false;
                    bool bNetwork = bool.Parse(SMTPEmailToNetwork);
                    if (bNetwork)
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    }
                    else
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    }

                    client.Credentials = new NetworkCredential(SMTPUserName, Password);

                    client.ServicePoint.MaxIdleTime = 2;

                    client.ServicePoint.ConnectionLimit = 1;

                    client.Send(mailMessage);
                }

                return true;

            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public bool AppointmentApprovalNotification(AppointmentDTO appointmentDTO)
        {
            //Fetching Settings from WEB.CONFIG file. 


            var emailSender = config.GetValue<string>("MailSettings:SMTPUserName");

            var emailSenderPassword = config.GetValue<string>("MailSettings:Password");

            var emailSenderHost = config.GetValue<string>("MailSettings:SMTPMailServer");

            int emailSenderPort = Convert.ToInt32(config.GetValue<string>("MailSettings:SMTPPort"));

            bool emailIsSSL = Convert.ToBoolean(config.GetValue<string>("MailSettings:SMTPUseSSL"));

            //Fetching Email Body Text from EmailTemplate File. 

            string FilePath = env.WebRootPath
                          + Path.DirectorySeparatorChar.ToString()
                          + "Templates"
                          + Path.DirectorySeparatorChar.ToString()
                          + "EmailTemplate"
                          + Path.DirectorySeparatorChar.ToString()
                          + "AppointmentApprovalNotification.html";


            StreamReader str = new StreamReader(FilePath);

            string MailText = str.ReadToEnd();

            str.Close();

            //Repalce [newusername] = signup user name   
            MailText = MailText.Replace("[FirstName]", appointmentDTO.FirstName.Trim());

            MailText = MailText.Replace("[AppointmentDate]", appointmentDTO.AppointmentDate.ToShortDateString());

            MailText = MailText.Replace("[TimeSlot]", appointmentDTO.TimeSlot.Trim());

            MailText = MailText.Replace("[CreateDate]", appointmentDTO.CreateDate.ToShortDateString());

            string subject = "Fertility Point : Appointment Approval";

            //Base class for sending email  
            MailMessage _mailmsg = new MailMessage();

            //Make TRUE because our body text is html  
            _mailmsg.IsBodyHtml = true;

            _mailmsg.From = new MailAddress(emailSender);

            _mailmsg.To.Add(appointmentDTO.Email.ToString());

            _mailmsg.Subject = subject;

            _mailmsg.Body = MailText;

            //Now set your SMTP   
            SmtpClient _smtp = new SmtpClient();
            {

                _smtp.Host = emailSenderHost;

                _smtp.Port = emailSenderPort;

                _smtp.EnableSsl = emailIsSSL;

                NetworkCredential _network = new NetworkCredential(emailSender, emailSenderPassword);

                _smtp.Credentials = _network;

                _smtp.Send(_mailmsg);
            }

            return true;
        }
    }
}
