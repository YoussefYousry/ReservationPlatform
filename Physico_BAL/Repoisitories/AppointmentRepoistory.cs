using Physico_BAL.Contracts;
using Physico_DAL.Data;
using Physico_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.Repoisitories
{
    public class AppointmentRepoistory : RepositoryBase<Appointment> , IAppointmentRepoistory
    {
        public AppointmentRepoistory(AppDbContext context):base(context)
        {
            
        }
    }
}
