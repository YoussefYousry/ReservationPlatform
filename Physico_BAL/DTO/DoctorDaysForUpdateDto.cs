using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.DTO
{
    public class DoctorDaysForUpdateDto
    {
        [DisplayFormat(DataFormatString = "{0:hh':'mm':'ss}", ApplyFormatInEditMode = true)]
        public TimeSpan Start { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh':'mm':'ss}", ApplyFormatInEditMode = true)]
        public TimeSpan End { get; set; }
    }
}
