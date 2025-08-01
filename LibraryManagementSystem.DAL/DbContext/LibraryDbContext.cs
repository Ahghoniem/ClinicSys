using LibraryManagementSystem.DAL.Configurations;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.Entities.Chat;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryManagementSystem.DAL.Entities.Products;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LibraryManagementSystem.DAL.DbContext
{
    public class LibraryDbContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> Brands { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }

        public DbSet<Clinic_schedule> Clinic_schedule { get; set; }
        public DbSet<Department> Department { get; set; }

        public DbSet<Patient_schedule> Patient_schedule { get; set; }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Conversation> Conversation { get; set; }
        public DbSet<ConversationUser> ConversationUser { get; set; }
        public DbSet<Message> Message { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ConversationUser>()
               .HasKey(cu => new { cu.ConversationId, cu.UserId });

            modelBuilder.Entity<ConversationUser>()
                .HasOne(cu => cu.Conversation)
                .WithMany(c => c.Users)
                .HasForeignKey(cu => cu.ConversationId);

            modelBuilder.Entity<ConversationUser>()
                .HasOne(cu => cu.User)
                .WithMany(u => u.Conversations)
                .HasForeignKey(cu => cu.UserId);
            modelBuilder.Entity<Message>()
                .Property(m => m.Status)
                .HasConversion<int>();

            modelBuilder.Entity<Doctor>()
                .ToTable("Doctors");
            modelBuilder.Entity<Admin>()
                .ToTable("Admins");
            modelBuilder.Entity<Patient>()
                .ToTable("Patients");
           // modelBuilder.Entity<ApplicationUser>()
           //.HasDiscriminator<int>("UserType")
           //.HasValue<Patient>(4);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
