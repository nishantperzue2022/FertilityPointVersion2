using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace FertilityPoint.Report
{
    public class Invoice
    {

        public string GenerateReportBlissHospitals()
        {

            try
            {
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


                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "Booking Report From " + fromDate.Value.ToShortDateString() + " To " + toDate.Value.ToShortDateString() + ".pdf");




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

        public string ExportCustomers()
        {
            List<Customer> allCustomer = new List<Customer>();

            allCustomer = context.Customers.ToList();


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "ReportCustomer.rpt"));

            rd.SetDataSource(allCustomer);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "CustomerList.pdf");
        }



    }
}
