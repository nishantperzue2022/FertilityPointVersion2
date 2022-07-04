
using FertilityPoint.BLL.Repositories.MpesaStkModule;
using FertilityPoint.DTO.MpesaC2BModule;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace FertilityPoint.Controllers
{
    public class PaybillCallBackController : Controller
    {

        private readonly ILogger<PaybillCallBackController> logger;

        private readonly IWebHostEnvironment environment;

        private readonly IPaymentRepository  paymentRepository;
        public PaybillCallBackController(IPaymentRepository paymentRepository,ILogger<PaybillCallBackController> logger, IWebHostEnvironment environment)
        {
            this.logger = logger;

            this.environment = environment;   
            
            this.paymentRepository = paymentRepository;           
        }

        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// C2B Payment validation
        /// </summary>
        /// <param name="customerToBusinessValidationCallback"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> C2BValidationCallback([FromBody] CustomerToBusinessValidationCallback customerToBusinessValidationCallback)
        {
            if (customerToBusinessValidationCallback is null)
            {
                return Ok(new
                {
                    ResultCode = 1,

                    ResultDesc = "Rejecting the transaction"
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(environment.WebRootPath, "Application_Files\\C2BValidationResults\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(environment.WebRootPath, "Application_Files\\C2BValidationResults\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(customerToBusinessValidationCallback, Formatting.Indented));

            logger.LogInformation(JsonConvert.SerializeObject(customerToBusinessValidationCallback, Formatting.Indented));

            return Ok(new
            {
                ResultCode = "0",

                ResponseDesc = "success"
            });
        }

        /// <summary>
        /// C2B Payment and confirmation can be processed here
        /// </summary>
        /// <param name="response">Response Payload</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<OkObjectResult> C2BPaymentCallback([FromBody] CustomerToBusinessCallback response)
        {
            if (response is null)
            {
                return Ok(new
                {
                    ResultCode = 1,

                    ResultDesc = "Rejecting the transaction"
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(environment.WebRootPath, "Application_Files\\C2BConfirmationResults\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(environment.WebRootPath, "Application_Files\\C2BConfirmationResults\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(response, Formatting.Indented));

            logger.LogInformation(JsonConvert.SerializeObject(response, Formatting.Indented));

            CustomerToBusinessCallback customerToBusinessCallback = new CustomerToBusinessCallback();

            paymentRepository.SaveLipaNaMpesa(customerToBusinessCallback);

            return Ok(new
            {
                ResultCode = "0",

                ResponseDesc = "success"
            });
        }
    }
}
