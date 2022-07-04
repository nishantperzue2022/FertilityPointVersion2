using AutoMapper;
using FertilityPoint.DAL.Modules;
using FertilityPoint.DTO.ApplicationUserModule;
using FertilityPoint.DTO.AppointmentModule;

using FertilityPoint.DTO.CountyModule;
using FertilityPoint.DTO.MpesaStkModule;
using FertilityPoint.DTO.PatientModule;
using FertilityPoint.DTO.ServiceModule;
using FertilityPoint.DTO.SpecialityModule;
using FertilityPoint.DTO.SubCountyModule;
using FertilityPoint.DTO.TimeSlotModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace FertilityPoint.DAL.MapperProfiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<County, CountyDTO>().ReverseMap();

            CreateMap<SubCounty, SubCountyDTO>().ReverseMap();

            CreateMap<Appointment, AppointmentDTO>().ReverseMap();

            CreateMap<Speciality, SpecialityDTO>().ReverseMap();

            CreateMap<CheckoutRequest, CheckoutRequestDTO>().ReverseMap();

            CreateMap<AppUser, ApplicationUserDTO>().ReverseMap();

            CreateMap<MpesaPayment, MpesaPaymentDTO>().ReverseMap();

            CreateMap<TimeSlot, TimeSlotDTO>().ReverseMap();

            CreateMap<Patient, PatientDTO>().ReverseMap();

            CreateMap<Service, ServiceDTO>().ReverseMap();

        }
    }
}
