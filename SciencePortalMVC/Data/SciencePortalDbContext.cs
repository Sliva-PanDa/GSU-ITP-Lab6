using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using SciencePortalMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace SciencePortalMVC.Data
{
    public class SciencePortalDbContext : IdentityDbContext<IdentityUser>
    {
        public SciencePortalDbContext(DbContextOptions<SciencePortalDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Publication> Publications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<Publication>()
                .HasMany(p => p.Teachers)
                .WithMany(t => t.Publications)
                .UsingEntity(j => j.ToTable("PublicationAuthor"));
        }
    }
}