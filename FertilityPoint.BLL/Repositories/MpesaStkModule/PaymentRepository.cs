using AutoMapper;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DAL.Utils;
using FertilityPoint.DTO.MpesaC2BModule;
using FertilityPoint.DTO.MpesaStkModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.MpesaStkModule
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public PaymentRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public async Task<List<MpesaPaymentDTO>> GetAll()
        {
            try
            {

                var mpesaPayments = (from payment in context.MpesaPayments

                                         //join appointment in context.Appointments on payment.TransactionNumber equals appointment.TransactionNumber

                                         //join patient in context.Patients on appointment.PatientId equals patient.Id

                                     select new MpesaPaymentDTO
                                     {
                                         ReceiptNo = payment.ReceiptNo,

                                         Id = payment.Id,

                                         Amount = payment.Amount,

                                         TransactionNumber = payment.TransactionNumber,

                                         PhoneNumber = payment.PhoneNumber,

                                         TransactionDate = payment.TransactionDate,


                                         //FullName = patient.FirstName + " " + patient.LastName,

                                     }).ToListAsync();

                return await mpesaPayments;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<MpesaPaymentDTO> GetByTransNumber(string TransactionId)
        {
            try
            {
                var data = await context.MpesaPayments.Where(x => x.TransactionNumber == TransactionId).FirstOrDefaultAsync();

                var slot = mapper.Map<MpesaPaymentDTO>(data);

                return slot;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public string GetTotalRevenue()
        {
            try
            {
                var payments = context.MpesaPayments.ToList();

                decimal sum_payments = Convert.ToDecimal(payments.Sum(x => x.Amount));

                var formatPayment = sum_payments.ToString("N");

                var totalAmount = formatPayment;

                return totalAmount;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public CheckoutRequestDTO GetCheckOutRequestById(string CheckoutRequestID)
        {
            throw new NotImplementedException();
        }
        public async Task<CheckoutRequestDTO> SaveCheckoutRequest(CheckoutRequestDTO checkoutRequestDTO)
        {
            try
            {
                checkoutRequestDTO.CreateDate = DateTime.Now;

                var speciality = mapper.Map<CheckoutRequest>(checkoutRequestDTO);

                context.CheckoutRequests.Add(speciality);

                await context.SaveChangesAsync();

                return checkoutRequestDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        private static DateTime? GetDateTimeFromInt(long? dateAsLong, bool hasTime = true)
        {
            if (dateAsLong.HasValue && dateAsLong > 0)
            {
                if (hasTime)
                {
                    // sometimes input is 14 digit and sometimes 16
                    var numberOfDigits = (int)Math.Floor(Math.Log10(dateAsLong.Value) + 1);

                    if (numberOfDigits > 14)
                    {
                        dateAsLong /= (int)Math.Pow(10, (numberOfDigits - 14));
                    }
                }

                if (DateTime.TryParseExact(dateAsLong.ToString(), hasTime ? "yyyyMMddHHmmss" : "yyyyMMdd",
                                          CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out DateTime dt))
                {
                    return dt;
                }
            }

            return null;
        }
        public void SaveSTKCallBackResponse(MpesaPaymentDTO mpesaPaymentDTO)
        {
            try
            {
                string code = ReceiptNumber.Generate_ReceiptNumber();

                var receiptNumber = "RN" + code;

                mpesaPaymentDTO.ReceiptNo = receiptNumber;

                long timestamp = long.Parse(mpesaPaymentDTO.TransactionDate);

                DateTime NewTransactionDate = GetDateTimeFromInt(timestamp).Value;

                mpesaPaymentDTO.TransactionDate = NewTransactionDate.ToString();

                mpesaPaymentDTO.IsPaymentUsed = 0;

                var transaction = new MpesaPayment
                {
                    CheckoutRequestID = mpesaPaymentDTO.CheckoutRequestID,

                    MerchantRequestID = mpesaPaymentDTO.MerchantRequestID,

                    ResultCode = mpesaPaymentDTO.ResultCode,

                    ResultDesc = mpesaPaymentDTO.ResultDesc,

                    Amount = mpesaPaymentDTO.Amount,

                    TransactionNumber = mpesaPaymentDTO.TransactionNumber,

                    TransactionDate = mpesaPaymentDTO.TransactionDate,

                    PhoneNumber = mpesaPaymentDTO.PhoneNumber,

                    ReceiptNo = mpesaPaymentDTO.ReceiptNo,

                    FirstName = "Not Specified",

                    LastName = "Not Specified",
                };

                context.MpesaPayments.Add(transaction);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }
        public bool IsTransactionExists(string TransactionNumber)
        {
            try
            {
                bool exists = context.MpesaPayments.Any(t => t.TransactionNumber == TransactionNumber & t.IsPaymentUsed == 0);

                return exists;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public bool SavePayBillCallBackResponse(MpesaPaymentDTO mpesaPaymentDTO)
        {
            try
            {

                string code = ReceiptNumber.Generate_ReceiptNumber();

                var receiptNumber = "RN" + code;

                mpesaPaymentDTO.ReceiptNo = receiptNumber;

                long timestamp = long.Parse(mpesaPaymentDTO.TransactionDate);

                DateTime NewTransactionDate = GetDateTimeFromInt(timestamp).Value;

                mpesaPaymentDTO.TransactionDate = NewTransactionDate.ToString();

                mpesaPaymentDTO.IsPaymentUsed = 0;

                var transaction = new MpesaPayment
                {
                    BillRefNumber = mpesaPaymentDTO.BillRefNumber,

                    BusinessShortCode = mpesaPaymentDTO.BusinessShortCode,

                    FirstName = mpesaPaymentDTO.FirstName,

                    MiddleName = mpesaPaymentDTO.MiddleName,

                    LastName = mpesaPaymentDTO.LastName,

                    InvoiceNumber = mpesaPaymentDTO.InvoiceNumber,

                    PhoneNumber = mpesaPaymentDTO.PhoneNumber,

                    OrgAccountBalance = mpesaPaymentDTO.OrgAccountBalance,

                    ThirdPartyTransID = mpesaPaymentDTO.ThirdPartyTransID,

                    Amount = mpesaPaymentDTO.Amount,

                    TransactionNumber = mpesaPaymentDTO.TransactionNumber,

                    TransactionType = mpesaPaymentDTO.TransactionType,

                    TransactionDate = mpesaPaymentDTO.TransactionDate,

                    ReceiptNo = mpesaPaymentDTO.ReceiptNo,
                };

                context.MpesaPayments.Add(transaction);

                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;

            }
        }
        public bool ValidatePayment(string TransactionNumber)
        {
            try
            {
                var serviceCharge = context.Services.FirstOrDefault();

                var getPayment = context.MpesaPayments.Where(x => x.TransactionNumber == TransactionNumber).FirstOrDefault();

                if (getPayment !=null)
                {
                    if (getPayment.Amount < serviceCharge.Amount)
                    {
                        return false;
                    }

                    if (getPayment.Amount >= serviceCharge.Amount)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }

        }
        public async Task<MpesaPaymentDTO> CreateMpesaPayment(MpesaPaymentDTO mpesaPaymentDTO)
        {
            try
            {
                string code = ReceiptNumber.Generate_ReceiptNumber();

                var receiptNumber = "RN" + code;

                mpesaPaymentDTO.ReceiptNo = receiptNumber;

                //long timestamp = long.Parse(mpesaPaymentDTO.TransactionDate);

                //DateTime NewTransactionDate = GetDateTimeFromInt(timestamp).Value;

                //mpesaPaymentDTO.TransactionDate = NewTransactionDate.ToString();

                mpesaPaymentDTO.IsPaymentUsed = 0;

                var transaction = new MpesaPayment
                {

                    Amount = mpesaPaymentDTO.Amount,

                    TransactionNumber = mpesaPaymentDTO.TransactionNumber,

                    TransactionDate = mpesaPaymentDTO.TransactionDate,

                    PhoneNumber = mpesaPaymentDTO.PhoneNumber,

                    ReceiptNo = mpesaPaymentDTO.ReceiptNo,

                    FirstName = mpesaPaymentDTO.FirstName,

                    LastName = mpesaPaymentDTO.LastName,

                    IsPaymentUsed = mpesaPaymentDTO.IsPaymentUsed,
                };

                context.MpesaPayments.Add(transaction);

                await context.SaveChangesAsync();

                return mpesaPaymentDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;

            }

        }

    }
}
