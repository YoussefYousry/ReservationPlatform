using Microsoft.EntityFrameworkCore;
using Physico_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Physico_DAL.Configurations
{
    public static class ExtentionModelBuilder
    {
        public static void AddInhertanceTaples(this ModelBuilder builder)
        {
            builder.Entity<User>().UseTptMappingStrategy().ToTable("Users");
            builder.Entity<Doctor>().ToTable("Doctors").HasBaseType<User>();
        }
        public static void AddOneToManyRelationship(this ModelBuilder builder)
        {
            builder.Entity<Appointment>().HasOne(d => d.DoctorObject).WithMany(a => a.Appointments).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<DoctorDays>().HasOne(d => d.DoctorObject).WithMany(d => d.Days).OnDelete(DeleteBehavior.NoAction);
        }
        public static void AddIndexes(this ModelBuilder builder)
        {
            builder.Entity<DoctorDays>().HasIndex(n => new { n.DoctorId  ,n.AppointmentDay}).IsUnique();
        }
    }
}
