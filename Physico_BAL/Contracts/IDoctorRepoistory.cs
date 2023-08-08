using Physico_BAL.DTO;
using Physico_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.Contracts
{
    public interface IDoctorRepoistory
    {
        Task<DoctorDto?> GetDoctorById(string doctorId, bool trackChanges);
        Task<IEnumerable<DoctorDto?>> GetAllDoctors(bool trackChanges);
        //Task<IEnumerable<AppointmentDto>> GetAllAppointments(string doctorId);
        //Task<IEnumerable<AppointmentDto>> GetAppointmentByIdForDoctor(string doctorId,Guid appointmentId);

    }
}
