using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GenerateReportBlissHospitals()
        {

            try
            {
                var endDate = ((DateTime)toDate).AddHours(23).AddMinutes(59).AddSeconds(59);

                ReportDocument rd = new ReportDocument();

                rd.Load(Path.Combine(Server.MapPath("~/Reports"), "VaccinationBookingReport.rpt"));

                var list = _appointmentService.GetPaymentReportsAsync();

                ReportDataSet dataSet1 = new ReportDataSet();

                foreach (var item in list)
                {
                    dataSet1.VaccinationBookingReport.AddVaccinationBookingReportRow(

                      item.Amount.ToString(),

                      item.BookingDate.ToShortDateString(),

                      item.MpesaReference,

                      item.PhoneNumber,

                      item.PatientName,

                      item.ClinicName,

                      item.StartTime.ToShortTimeString() + " - " + item.EndTime.ToShortTimeString(),

                      item.EndTime.ToShortTimeString(),

                      item.DoseNumber.ToString(),

                      item.TotalAmount.ToString(),

                      item.AppointmentDate.ToShortDateString(),

                      item.PhoneNumberUsedForPayment
                      );
                }
                bool isEmpty = !list.Any();

                if (isEmpty)
                {
                    TempData["Bliss_Error"] = "No data was found between  the selected date period!";
                    return RedirectToAction("Index", "Reports");
                }
                else
                {
                    string ClinicName = "";

                    if (clinicId.HasValue)
                    {
                        var clinics = await (_clinicService.GetClinicByIdAsync(clinicId.ToString()));
                        ClinicName = clinics?.Name;
                    }
                    else
                    {
                        ClinicName = "All Clinics";
                    }

                    rd.SetDataSource(dataSet1);
                    rd.SetParameterValue("UserName", ClinicName);
                    rd.SetParameterValue("DateFrom", fromDate.Value.ToShortDateString());
                    rd.SetParameterValue("DateTo", toDate.Value.ToShortDateString());

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    //rd.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                    //rd.PrintOptions.ApplyPageMargins(new CrystalDecisions.Shared.PageMargins(5, 5, 5, 5));
                    //rd.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5;

                    if (documentType == "pdf")

                    {
                        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);
                        return File(stream, "application/pdf", "Booking Report From " + fromDate.Value.ToShortDateString() + " To " + toDate.Value.ToShortDateString() + ".pdf");
                    }

                    else
                    {
                        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.ExcelRecord);
                        stream.Seek(0, SeekOrigin.Begin);
                        return File(stream, "application/pdf", "Booking Report From " + fromDate.Value.ToShortDateString() + " To " + toDate.Value.ToShortDateString() + ".xls");
                    }

                }

            }
            catch (TypeInitializationException ex)
            {
                TempData["Booking_Error"] = "File Not Found, kindly contact your administrator!";
                return RedirectToAction("Index", "Reports");
            }
            catch (Exception ex)
            {
                TempData["Booking_Error"] = "An error occured, kindly contact your administrator!";
                return RedirectToAction("Index", "Reports");
            }
        }











    }
}
