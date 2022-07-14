using FertilityPoint.BLL.Repositories.PayPalModule;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace FertilityPoint.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PaypalPaymentReportsController : Controller
    {
        private readonly IPayPalRepository payPalRepository;

        public PaypalPaymentReportsController(IPayPalRepository payPalRepository)
        {
            this.payPalRepository = payPalRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DownloadAllPaypalPayment()
        {
            //var user1 = await userManager.FindByEmailAsync(User.Identity.Name);
            // Get the user list 

            var paypalPayment = await payPalRepository.GetAll();

            var stream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("paypalPayment");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 5;
                var row = startRow;

                //Create Headers and format them
                worksheet.Cells["A1,B1,C1,D1,E1,F1,G1,H1,I1,J1"].Value = "Paypal Payment Report";

                using (var r = worksheet.Cells["A1:J1"])
                {
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                }

                worksheet.Cells["A2"].Value = "PatientName";
                worksheet.Cells["B2"].Value = "Email";
                worksheet.Cells["C2"].Value = "CountryCode";
                worksheet.Cells["D2"].Value = "TransactionId";
                worksheet.Cells["E2"].Value = "MerchantId";
                worksheet.Cells["F2"].Value = "PaymentMethod";
                worksheet.Cells["G2"].Value = "AmountPaid";
                worksheet.Cells["H2"].Value = "Currency";
                worksheet.Cells["I2"].Value = "Status";
                worksheet.Cells["J2"].Value = "TransactionDate";


                worksheet.Cells["A2:J2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A2:J2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                worksheet.Cells["A2:J2"].Style.Font.Bold = true;

                row = 3;
                foreach (var user in paypalPayment)
                {

                    worksheet.Cells[row, 1].Value = user.PayerFullName;
                    worksheet.Cells[row, 2].Value = user.PayerEmail;
                    worksheet.Cells[row, 3].Value = user.PayerCountryCode;
                    worksheet.Cells[row, 4].Value = user.TransactionId;
                    worksheet.Cells[row, 5].Value = user.MerchantId;
                    worksheet.Cells[row, 6].Value = user.PaymentMethod;
                    worksheet.Cells[row, 7].Value = user.AmountPaid;
                    worksheet.Cells[row, 8].Value = user.Currency;
                    worksheet.Cells[row, 9].Value = user.Status;
                    worksheet.Cells[row, 10].Value = user.TransactionDate.ToShortDateString();


                    row++;
                }

                // set some core property values
                xlPackage.Workbook.Properties.Title = "Hf";
                xlPackage.Workbook.Properties.Author = "Hf";
                xlPackage.Workbook.Properties.Subject = "Hf";
                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PaypalPaymentReport.xlsx");
        }
    }
}
