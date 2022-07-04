using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FertilityPoint.DAL.Modules
{
    public partial class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<County> Counties { get; set; }
        public virtual DbSet<SubCounty> SubCounties { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Speciality> Specialities { get; set; }
        public virtual DbSet<CheckoutRequest> CheckoutRequests { get; set; }
        public virtual DbSet<MpesaPayment> MpesaPayments { get; set; }
        public virtual DbSet<TimeSlot> TimeSlots { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<PaybillPayment> PaybillPayments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MpesaPayment>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Balance).HasColumnType("decimal(18,4)");

            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");

            });
        }


    }


}
