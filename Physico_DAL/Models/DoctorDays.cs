using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_DAL.Models
{
    public class DoctorDays
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Doctor))]
        public string? DoctorId { get; set; }
        public Doctor? DoctorObject { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public DateOnly AppointmentDay { get; set; }
    }
}
