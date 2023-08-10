using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.RequestFeatures
{
    public class AppointmentParamters : RequestParamters
    {
        public string? SearchTerm { get; set; }
        public DateOnly Date { get; set; }
        
    }
}
