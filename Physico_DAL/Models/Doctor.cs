using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_DAL.Models
{
    public class Doctor : User
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set;}
        public ICollection<DoctorDays> Days { get; set; }
        public ICollection<Appointment> Appointments { get; set; }

        public Doctor()
        {
            Days = new HashSet<DoctorDays>();
            Appointments = new HashSet<Appointment>();
        }
    }
}
