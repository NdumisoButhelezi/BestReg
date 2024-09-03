
﻿using Amazon.SimpleSystemsManagement.Model;

using BestReg.Models; // Ensure you have this using directive for your models

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BestReg.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<SupplierOrder> SupplierOrders { get; set; }
        public DbSet<VetAppointment> VetAppointments { get; set; }
        public DbSet<AppointmentType> AppointmentTypes { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DiagnosisRecord> DiagnosisRecords { get; set; }
        public DbSet<VaccinationSchedule> VaccinationSchedules { get; set; }
        public DbSet<FeedingPlan> FeedingPlans { get; set; }
        public DbSet<NutritionStock> NutritionStocks { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Event> Events { get; set; }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; } // Add InventoryItem DbSet
        public DbSet<SupplierOrder> SupplierOrders { get; set; } // Add SupplierOrder DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)

  

        // If you have an Animal model, add it as well
        public DbSet<Animal> Animals { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)

        {
            base.OnModelCreating(modelBuilder);


            // Example of configuring relationships and constraints
            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Animal)
                .WithMany(a => a.MedicalRecords)
                .HasForeignKey(m => m.AnimalId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Item)
                .WithMany()
                .HasForeignKey(o => o.ItemId);

        // Entities
        public DbSet<CheckInRecord> CheckInRecords { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }


            modelBuilder.Entity<SupplierOrder>()
                .HasMany(s => s.Items)
                .WithOne()
                .HasForeignKey(o => o.SupplierOrderId);
        }
    }
}
