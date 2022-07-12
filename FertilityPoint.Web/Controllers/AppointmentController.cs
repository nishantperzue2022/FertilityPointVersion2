using FertilityPoint.DTO.AppointmentModule;
using FertilityPoint.Web.Extensions;
using FertilityPoint.Services.EmailModule;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FertilityPoint.BLL.Repositories.AppointmentModule;

using System.Linq;
using FertilityPoint.BLL.Repositories.SpecialityModule;
using System.Net.Http;
using FertilityPoint.DTO.MpesaStkModule;
using Newtonsoft.Json;
using System.Text;
using FertilityPoint.BLL.Repositories.MpesaStkModule;
using FertilityPoint.BLL.Repositories.TimeSlotModule;
using FertilityPoint.BLL.Repositories.PatientModule;
using FertilityPoint.DTO.PatientModule;
using System.Data;
using System.Collections.Generic;

using Microsoft.AspNetCore.Hosting;
using FertilityPoint.DTO.TimeSlotModule;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using FertilityPoint.BLL.Repositories.ServiceModule;


namespace FertilityPoint.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository appointmentRepository;

        private readonly IMailService mailService;

        private readonly IPaymentRepository paymentRepository;

        private readonly ITimeSlotRepository timeSlotRepository;

        private readonly IPatientRepository patientRepository;

        private readonly IWebHostEnvironment env;

        private readonly IConfiguration config;

        private readonly IServicesRepository servicesRepository;
        public AppointmentController(

            IConfiguration config,

            ITimeSlotRepository timeSlotRepository,

            IPaymentRepository paymentRepository,

            ISpecialityRepository specialityRepository,

            IMailService mailService,

            IPatientRepository patientRepository,

            IWebHostEnvironment env,

            IServicesRepository servicesRepository,

            IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;

            this.mailService = mailService;

            this.timeSlotRepository = timeSlotRepository;

            this.paymentRepository = paymentRepository;

            this.patientRepository = patientRepository;

            this.servicesRepository = servicesRepository;

            this.env = env;

            this.config = config;

        }
        public IActionResult Index()
        {
            try
            {

                return View();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

               // TempData["Error"] = "Something went wrong";

                return RedirectToAction("", "Home", new { area = "" });
            }
        }
        public async Task<IActionResult> Test(DateTime AppointmentDate)
        {
            try
            {
                var timeslot = (await timeSlotRepository.GetAll()).Where(x => x.AppointmentDate == AppointmentDate);

                return View(timeslot);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<IActionResult> Get()
        {
            try
            {
                var app = await appointmentRepository.GetAll();

                return Json(app);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public IActionResult Receipt(Guid Id)
        {
            try
            {
                var TransactionDetails = appointmentRepository.GetTransaction(Id);

                return View(TransactionDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                //TempData["Error"] = "Something went wrong";

                return RedirectToAction("", "Home", new { area = "" });
            }
        }

        //public ActionResult DownloadReceipt(Guid Id)
        //{
        //    try
        //    {
        //        var dt = new DataTable();

        //        var data = GetTransactionDetails(Id);

        //        dt = data;

        //        string mimetype = "";

        //        int extension = 1;

        //        var path = $"{env.WebRootPath}\\Reports\\Invoice.rdlc";

        //        Dictionary<string, string> parameters = new Dictionary<string, string>();

        //        LocalReport localReport = new LocalReport(path);

        //        localReport.AddDataSource("PaymentInvoice", dt);

        //        var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

        //        return File(result.MainStream, "application/pdf");

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);

        //        TempData["Error"] = "Something went wrong";

        //        return RedirectToAction("", "Home", new { area = "" });
        //    }

        //}
        public DataTable GetTransactionDetails(Guid Id)
        {
            try
            {
                var data = appointmentRepository.GetTransaction(Id);

                var dt = new DataTable();

                dt.Columns.Add("Id");

                dt.Columns.Add("FullName");

                dt.Columns.Add("Email");

                dt.Columns.Add("PhoneNumber");

                dt.Columns.Add("TimeSlot");

                dt.Columns.Add("AppointmentDate");

                dt.Columns.Add("PaidByNumber");

                dt.Columns.Add("TransactionNumber");

                dt.Columns.Add("TransactionDate");

                dt.Columns.Add("Amount");

                dt.Columns.Add("ReceiptNo");

                DataRow row;

                row = dt.NewRow();

                row["Id"] = data.Id.ToString().ToUpper();

                row["FullName"] = data.FullName.ToString();

                row["Email"] = data.Email.ToString();

                row["PhoneNumber"] = data.PhoneNumber.ToString();

                row["TimeSlot"] = data.TimeSlot.ToString();

                row["AppointmentDate"] = data.AppointmentDate.ToString();

                row["PaidByNumber"] = data.PaidByNumber.ToString();

                row["TransactionNumber"] = data.TransactionNumber.ToString();

                row["TransactionDate"] = data.TransactionDate.ToString();

                row["Amount"] = data.Amount.ToString("N");

                row["ReceiptNo"] = data.ReceiptNo.ToString();

                dt.Rows.Add(row);

                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<IActionResult> Create(AppointmentDTO appointmentDTO)
        {
            try
            {
                var appointmentDate = appointmentDTO.NewAppointmentDate;

                DateTime oDate = DateTime.ParseExact(appointmentDate, "M/d/yyyy", CultureInfo.InvariantCulture);

                appointmentDTO.AppointmentDate = oDate;

                if (appointmentDTO.Email == null || appointmentDTO.Email == string.Empty)
                {
                    return Json(new { success = false, responseText = "Email is a required field" });
                }

                if (appointmentDTO.TimeId == Guid.Empty)
                {
                    return Json(new { success = false, responseText = "Please select appointment time" });
                }

                //if (appointmentDTO.TransactionNumber == null || appointmentDTO.Email == string.Empty)
                //{
                //    return Json(new { success = false, responseText = "Mpesa Transaction Number is a required field" });
                //}

                if (appointmentDTO.AppointmentDate.GetHashCode() == 0)
                {
                    return Json(new { success = false, responseText = "Appointment date is a required field" });
                }

                //var isPaymentExists = paymentRepository.IsTransactionExists(appointmentDTO.TransactionNumber);

                //if (isPaymentExists == false)
                //{
                //    return Json(new { success = false, responseText = "Payment details could not be found ! " });
                //}

                var validateEmail = ValidateEmail.Validate(appointmentDTO.Email);

                var get_slot = timeSlotRepository.GetById(appointmentDTO.TimeId);

                if(get_slot == null)
                {
                    return Json(new { success = false, responseText = "Sorry , this slot has been booked, please select another one! " });

                }
                appointmentDTO.TimeSlot = get_slot.TimeSlot;

                var validatePayment = paymentRepository.ValidatePayment(appointmentDTO.TransactionNumber);

                //if (validatePayment == false)
                //{
                //    return Json(new { success = false, responseText = "Sorry ! booking not successfull.Please make full payment" });

                //}

                if (validateEmail.Success == true)
                {
                    var getPatient = await patientRepository.GetAll();

                    var IsPatientExist = getPatient.Where(x => x.Email == appointmentDTO.Email && x.PhoneNumber == appointmentDTO.PhoneNumber).FirstOrDefault();

                    if (IsPatientExist != null)
                    {
                        appointmentDTO.PatientId = IsPatientExist.Id;
                    }
                    else
                    {
                        appointmentDTO.PatientId = Guid.NewGuid();

                        PatientDTO patientDTO = new PatientDTO()
                        {
                            Id = appointmentDTO.PatientId,

                            CreateDate = DateTime.Now,

                            FirstName = appointmentDTO.FirstName,

                            LastName = appointmentDTO.LastName,

                            Email = appointmentDTO.Email,

                            PhoneNumber = appointmentDTO.PhoneNumber,
                        };

                        var createPatient = await patientRepository.Create(patientDTO);
                    }

                    var results = await appointmentRepository.Create(appointmentDTO);

                    if (results != null)
                    {
                        var url = Url.Action("DownloadReceipt", "Appointment", new { Id = appointmentDTO.Id }, Request.Scheme);

                        appointmentDTO.ReceiptURL = url;

                        var sendClientEmail = mailService.AppointmentEmailNotification(appointmentDTO);

                        var sendFertilityPointEmail = await mailService.FertilityPointEmailNotification(appointmentDTO);

                        return Json(new { success = true, responseText = appointmentDTO.Id });
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "Failed to submitted appointment" });
                    }

                }
                else
                {
                    return Json(new { success = false, responseText = "You have entered invalid email" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        [HttpPost]
        public async Task<ActionResult> MpesaSTKPush(LipaMpesa lipaMpesa)
        {
            try
            {
                if (lipaMpesa.PhoneNumber == null || lipaMpesa.PhoneNumber == "")
                {
                    return Json(new { success = false, responseText = "Please Enter Phone Number" });
                }

                var get_Service_Details = await servicesRepository.GetDefault();

                int amount = Convert.ToInt32(get_Service_Details.Amount);

                var msisdn = FormatPhoneNumber(lipaMpesa.PhoneNumber);

                string url = @"https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest";

                HttpClient client = new HttpClient();

                var key = "5CAjwH782Y2gKvDfUQvAXuDDjKkXBJTe";

                var secrete = "25CuZHuXfaG3gTij";

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await GenerateAccessToken(key, secrete));

                var mpesaCallbackURL = config.GetValue<string>("MpesaSTKCallBackURL:URL");

                var mpesaExpressRequestDTO = new MpesaExpressRequestDTO
                {
                    BusinessShortCode = 174379,

                    Password = GeneratePassword(),

                    Timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"),

                    TransactionType = "CustomerPayBillOnline",

                    Amount = amount,

                    PartyA = msisdn,

                    PartyB = 174379,

                    PhoneNumber = msisdn,

                    CallBackURL = mpesaCallbackURL,

                    AccountReference = "Fertility Point",

                    TransactionDesc = "Appointment"

                };

                string post_params = JsonConvert.SerializeObject(mpesaExpressRequestDTO);

                HttpContent content = new StringContent(post_params, Encoding.UTF8, "application/json");

                HttpResponseMessage result = await client.PostAsync(url, content);

                result.EnsureSuccessStatusCode();

                var response = await result.Content.ReadAsStringAsync();

                var mpesaExpressResponse = JsonConvert.DeserializeObject<CheckoutRequestDTO>(response);

                var mpesaResponse = new CheckoutRequestDTO
                {
                    MerchantRequestID = mpesaExpressResponse.MerchantRequestID,

                    CheckoutRequestID = mpesaExpressResponse.CheckoutRequestID,

                    ResponseCode = mpesaExpressResponse.ResponseCode,

                    ResponseDescription = mpesaExpressResponse.ResponseDescription,

                    CustomerMessage = mpesaExpressResponse.CustomerMessage,
                };

                var h = await paymentRepository.SaveCheckoutRequest(mpesaResponse);

                return Json(new { success = true, responseText = "Payment request has been successfull send ,please check your phone for an stk push prompt" });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new
                {
                    success = false,

                    responseText = "Payment request was not successfull ," +

                    " please check your mobile number and try again after 5 minutes or click on the Mpesa Paybill otpion and follow the payment procedure "
                });

            }

        }
        public static string GeneratePassword()
        {
            try
            {
                var lipa_time = DateTime.Now.ToString("yyyyMMddHHmmss");

                var passkey = "bfb279f9aa9bdbcf158e97dd71a467cd2e0c893059b10f78e6b72ada1ed2c919";

                int BusinessShortCode = 174379;

                var timestamp = lipa_time;

                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(BusinessShortCode + passkey + timestamp);

                var password = Convert.ToBase64String(passwordBytes);

                return password;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<string> GenerateAccessToken(string key, string secrete)
        {
            try
            {
                var url = @"https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials";

                HttpClient client = new HttpClient();

                var byteArray = System.Text.Encoding.ASCII.GetBytes(key + ":" + secrete);

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                HttpResponseMessage response = await client.GetAsync(url);

                HttpContent content = response.Content;

                string result = await content.ReadAsStringAsync();

                var mpesaAccessToken = JsonConvert.DeserializeObject<MpesaAccessTokenDTO>(result);

                return mpesaAccessToken.access_token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;

            string formatted = "";

            if (phoneNumber.StartsWith("0"))
                formatted = "254" + phoneNumber.Substring(1, phoneNumber.Length - 1);

            if (phoneNumber.StartsWith("7"))
                formatted = "254" + phoneNumber;

            if (phoneNumber.StartsWith("254"))
                formatted = phoneNumber;

            if (phoneNumber.StartsWith("254"))
                formatted = phoneNumber;

            return formatted;
        }

        public IActionResult GetSlotById(Guid Id)
        {
            try
            {
                var data = timeSlotRepository.GetById(Id);

                if (data != null)
                {
                    TimeSlotDTO file = new TimeSlotDTO
                    {
                        Id = data.Id,

                        FromTime = data.FromTime,

                        ToTime = data.ToTime,

                        CreateDate = data.CreateDate,
                    };

                    return Json(new { data = file });
                }
                return Json(new { data = false });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<IActionResult> GetSlots()
        {
            try
            {
                DateTime todaysDate = DateTime.Now;

                var slotDate = todaysDate.Date;

                var getSlot = await timeSlotRepository.GetAll();

                var timeslot = getSlot.Where(x => x.IsBooked == 0 && x.AppointmentDate == slotDate).OrderBy(x => x.FromTime);

                return Json(timeslot.Select(x => new
                {
                    SlotId = x.Id,

                    SlotName = x.TimeSlot

                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("", "Home", new { area = "" });
            }

        }
        public async Task<IActionResult> GetSlotsByAppointmentDate(string AppointmentDate)
        {
            try
            {
                DateTime appDate = DateTime.ParseExact(AppointmentDate, "M/d/yyyy", CultureInfo.InvariantCulture);

                var getSlot = await timeSlotRepository.GetAll();

                var timeslot = getSlot.Where(x => x.IsBooked == 0 && x.AppointmentDate == appDate).OrderBy(x => x.FromTime);

                return Json(timeslot.Select(x => new
                {
                    SlotId = x.Id,

                    SlotName = x.TimeSlot

                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("", "Home", new { area = "" });
            }

        }

        public async Task<IActionResult> GetSlotsByQuickSearch(byte QuickSearch)
        {
            if (QuickSearch == 1)
            {
                return await GetSlots();
            }
            else
            {
                return await GetSlots();
            }


        }

    }

    public class LipaMpesa
    {
        public Guid PatientId { get; set; }
        public string PhoneNumber { get; set; }

    }
}
