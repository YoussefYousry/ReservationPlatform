using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.DTO
{
    public class DoctorDaysForCreationDto
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly AppointmentDay { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh':'mm':'ss}", ApplyFormatInEditMode = true)]
        public TimeSpan Start { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh':'mm':'ss}", ApplyFormatInEditMode = true)]
        public TimeSpan End { get; set; }
    }
}
