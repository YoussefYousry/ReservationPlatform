using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_DAL.Models
{
    public class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan Time { get; set; }
        public bool IsAvailable { get; set; } = true;
        [ForeignKey(nameof(Doctor))]
        public  string? DoctorId{ get; set; }
        public Doctor? DoctorObject { get; set; }
        public required string PatientName { get; set; }
        public required string PatientMobileNo { get; set;}
        public required string PatientEmail { get; set;}
        public required int PatientAge { get; set; }
    }
}
