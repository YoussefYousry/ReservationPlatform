using Physico_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.DTO
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan Time { get; set; }
        public bool IsAvailable { get; set; }
        public string? DoctorId { get; set; }
        public required string PatientName { get; set; }
        public required string PatientMobileNo { get; set; }
        public required string PatientEmail { get; set; }
        public required int PatientAge { get; set; }
    }
}
