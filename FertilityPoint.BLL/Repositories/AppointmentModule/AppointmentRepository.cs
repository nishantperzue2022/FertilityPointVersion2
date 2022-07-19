using AutoMapper;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.AppointmentModule;
using FertilityPoint.DTO.PatientModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FertilityPoint.BLL.Repositories.AppointmentModule
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public AppointmentRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public async Task<AppointmentDTO> Create(AppointmentDTO appointmentDTO)
        {
            try
            {
                appointmentDTO.Id = Guid.NewGuid();

                var appointment = new Appointment
                {
                    CreateDate = DateTime.Now,

                    Id = appointmentDTO.Id,

                    Status = 0,

                    AppointmentDate = appointmentDTO.AppointmentDate,

                    PatientId = appointmentDTO.PatientId,

                    TimeSlotId = appointmentDTO.TimeSlotId,

                    TransactionNumber = appointmentDTO.TransactionNumber,

                    ApprovedBy = "",

                };

                context.Appointments.Add(appointment);

                await context.SaveChangesAsync();

                UpdateSlot(appointmentDTO);

                return appointmentDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public bool UpdateSlot(AppointmentDTO appointmentDTO)
        {
            var getSlot = context.TimeSlots.Find(appointmentDTO.TimeSlotId);

            if (getSlot != null)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    getSlot.IsBooked = 1;

                    getSlot.AppointmentDate = appointmentDTO.AppointmentDate;

                    transaction.Commit();

                    context.SaveChanges();
                }
                return true;
            }

            return false;
        }
        public bool UpdatePaymentStatus(AppointmentDTO appointmentDTO)
        {
            var getSlot = context.MpesaPayments.Where(x => x.TransactionNumber == appointmentDTO.TransactionNumber.Trim()).FirstOrDefault();

            if (getSlot != null)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    getSlot.IsPaymentUsed = 1;

                    transaction.Commit();

                    context.SaveChanges();
                }
                return true;
            }

            return false;
        }
        public async Task<List<AppointmentDTO>> GetAll()
        {
            try
            {
                var appointments = (from appointment in context.Appointments

                                    join patient in context.Patients on appointment.PatientId equals patient.Id

                                    join timslot in context.TimeSlots on appointment.TimeSlotId equals timslot.Id

                                    select new AppointmentDTO()
                                    {
                                        Id = appointment.Id,

                                        Status = appointment.Status,

                                        CreateDate = appointment.CreateDate,

                                        AppointmentDate = appointment.AppointmentDate,

                                        FirstName = patient.FirstName,

                                        LastName = patient.LastName,

                                        TimeSlotId = appointment.TimeSlotId,

                                        FromTime = timslot.FromTime,

                                        ToTime = timslot.ToTime,

                                        TimeSlot = timslot.FromTime.ToString("h:mm tt") + " - " + timslot.ToTime.ToString("h:mm tt"),



                                    }).ToListAsync();


                return await appointments;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
        public async Task<AppointmentDTO> GetById(Guid Id)
        {
            try
            {
                var appointments = (from appointment in context.Appointments

                                    join patient in context.Patients on appointment.PatientId equals patient.Id

                                    join timslot in context.TimeSlots on appointment.TimeSlotId equals timslot.Id

                                    where appointment.Id == Id

                                    select new AppointmentDTO()
                                    {
                                        Id = appointment.Id,

                                        Status = appointment.Status,

                                        CreateDate = appointment.CreateDate,

                                        AppointmentDate = appointment.AppointmentDate,

                                        PatientId = patient.Id,

                                        FirstName = patient.FirstName,

                                        PhoneNumber = patient.PhoneNumber,

                                        Email = patient.Email,

                                        LastName = patient.LastName,

                                        TimeSlotId = appointment.TimeSlotId,

                                        ToTime = timslot.ToTime,

                                        FromTime = timslot.FromTime,

                                        TimeSlot = timslot.FromTime.ToString("h:mm tt") + " - " + timslot.ToTime.ToString("h:mm tt"),

                                    }).FirstOrDefaultAsync();

                return await appointments;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
        public AppointmentDTO GetTransaction(Guid Id)
        {
            try
            {
                var appointments = (from appointment in context.Appointments

                                    join patient in context.Patients on appointment.PatientId equals patient.Id

                                    join timslot in context.TimeSlots on appointment.TimeSlotId equals timslot.Id

                                    //join payment in context.MpesaPayments on appointment.TransactionNumber equals payment.TransactionNumber

                                    where appointment.Id == Id

                                    select new AppointmentDTO()
                                    {
                                        Id = appointment.Id,

                                        Status = appointment.Status,

                                        CreateDate = appointment.CreateDate,

                                        AppointmentDate = appointment.AppointmentDate,

                                        FirstName = patient.FirstName,

                                        LastName = patient.LastName,

                                        PhoneNumber = patient.PhoneNumber,

                                        //PaidByNumber = payment.PhoneNumber,

                                        //TransactionNumber = payment.TransactionNumber,

                                        //TransactionDate = payment.TransactionDate,

                                        //Amount = payment.Amount,

                                        //ReceiptNo = payment.ReceiptNo,

                                        Email = patient.Email,

                                        TimeSlotId = appointment.TimeSlotId,

                                        ToTime = timslot.ToTime,

                                        FromTime = timslot.FromTime,

                                        TimeSlot = timslot.FromTime.ToString("h:mm tt") + " - " + timslot.ToTime.ToString("h:mm tt"),


                                    }).FirstOrDefault();

                return appointments;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> ApproveAppointment(AppointmentDTO appointmentDTO)
        {
            try
            {
                bool result = false;

                var getAppointment = await context.Appointments.FindAsync(appointmentDTO.Id);

                if (getAppointment != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        getAppointment.Status = 1;

                        getAppointment.ApprovedBy = appointmentDTO.ApprovedBy;

                        transaction.Commit();

                        await context.SaveChangesAsync();
                    }

                    return true;
                }

                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                bool result = false;

                var data = await context.Appointments.FindAsync(Id);

                if (data != null)
                {
                    context.Appointments.Remove(data);

                    await context.SaveChangesAsync();

                    result = true;
                }

                return result;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public async Task<AppointmentDTO> RescheduleAppointment(AppointmentDTO appointmentDTO)
        {
            try
            {
                var data = await context.Appointments.FindAsync(appointmentDTO.Id);

                if (data != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        data.AppointmentDate = appointmentDTO.AppointmentDate;

                        data.TimeSlotId = appointmentDTO.TimeSlotId;

                        transaction.Commit();
                    }

                    await context.SaveChangesAsync();

                    UpdateSlot(appointmentDTO);

                    VacateSlot(appointmentDTO.OldTimeSlotId);

                    return appointmentDTO;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public bool VacateSlot(Guid TimeSlotId)
        {
            var getSlot = context.TimeSlots.Find(TimeSlotId);

            if (getSlot != null)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    getSlot.IsBooked = 0;

                    transaction.Commit();

                    context.SaveChanges();
                }
                return true;
            }

            return false;
        }
    }
}
