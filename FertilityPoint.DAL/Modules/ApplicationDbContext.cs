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

        public virtual DbSet<AppUser> AppUsers { get; set; } = null!;
        public virtual DbSet<County> Counties { get; set; } = null!;
        public virtual DbSet<SubCounty> SubCounties { get; set; } = null!;
        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<Speciality> Specialities { get; set; } = null!;
        public virtual DbSet<CheckoutRequest> CheckoutRequests { get; set; } = null!;
        public virtual DbSet<MpesaPayment> MpesaPayments { get; set; } = null!;
        public virtual DbSet<TimeSlot> TimeSlots { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<PaybillPayment> PaybillPayments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MpesaPayment>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Balance).HasColumnType("decimal(18,2)");

            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");

            });



            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.TimeSlot)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.TimeSlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointments_TimeSlots");
            });

            modelBuilder.Entity<TimeSlot>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });


        }


    }


}
