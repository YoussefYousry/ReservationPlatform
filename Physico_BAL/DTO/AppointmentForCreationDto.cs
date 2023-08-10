using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.DTO
{
    public class AppointmentForCreationDto
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan Time { get; set; }
        public string? DoctorId { get; set; }
        public  string? PatientName { get; set; }
        public  string? PatientMobileNo { get; set; }
        public  string? PatientEmail { get; set; }
        public  int PatientAge { get; set; }
    }
}
