using EducationSystem.Entity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationSystem.Service.Context
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; } // DbSet for AppUser (inherited from IdentityUser)
        public DbSet<UserTask> Tasks { get; set; } // DbSet for UserTask
        public DbSet<Education> Educations { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Institution> Institutions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                    base.OnModelCreating(modelBuilder); // Ensure Identity tables are created

                    // Configure UserTask table and set schema and primary key
                    modelBuilder.Entity<UserTask>()
                 .ToTable("Table_Tasks")
                 .HasKey(t => t.Id);              // Set Id as primary key

                    // Relationship between UserTask and AppUser for CreatedBy
                    modelBuilder.Entity<UserTask>()
                        .HasOne(ut => ut.CreatedBy)               // Each task has one CreatedBy user
                        .WithMany(u => u.Tasks)                   // Each user can have many tasks they created
                        .HasForeignKey(ut => ut.CreatedById)      // Foreign key in UserTask for CreatedBy
                        .OnDelete(DeleteBehavior.Restrict);       // Prevent cascading delete

                    // Relationship between UserTask and AppUser for AssignedTo
                    modelBuilder.Entity<UserTask>()
                        .HasOne(ut => ut.AssignedTo)              // Each task has one AssignedTo user
                        .WithMany(u => u.AssignedTasks)           // Each user can have many tasks assigned to them
                        .HasForeignKey(ut => ut.AssignedToId)     // Foreign key in UserTask for AssignedTo
                        .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            //Education
            // Configure Education relationships
            modelBuilder.Entity<Education>()
                .HasOne(e => e.Degree)
                .WithMany()
                .HasForeignKey(e => e.DegreeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Education>()
                .HasOne(e => e.Institution)
                .WithMany()
                .HasForeignKey(e => e.InstitutionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Education>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }

        

            }
        
    }
    
