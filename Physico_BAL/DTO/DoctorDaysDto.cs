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
    public class DoctorDaysDto
    {
        public int Id { get; set; }
        public string? DoctorId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly AppointmentDay { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh':'mm':'ss}", ApplyFormatInEditMode = true)]
        public TimeSpan Start { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh':'mm':'ss}", ApplyFormatInEditMode = true)]
        public TimeSpan End { get; set; }
    }
}
