using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.Contracts
{
    public interface IRepositoryManager
    {
        IDoctorDaysRepoistory DoctorDays { get; }
        IDoctorRepoistory Doctor { get; }
        IAppointmentRepoistory Appointment { get; }
        Task SaveChangesAsync();
    }
}
