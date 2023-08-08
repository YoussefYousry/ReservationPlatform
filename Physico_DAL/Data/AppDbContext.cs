using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Physico_DAL.Configurations;
using Physico_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_DAL.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorDays> DoctorDays { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {


            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfigurations());
            builder.AddInhertanceTaples();
            builder.AddOneToManyRelationship();

        }
    }
}
