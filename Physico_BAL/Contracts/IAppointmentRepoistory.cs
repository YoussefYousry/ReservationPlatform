using Physico_BAL.DTO;
using Physico_BAL.RequestFeatures;
using Physico_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.Contracts
{
    public interface IAppointmentRepoistory
    {
        void CreateAppointment(Appointment appointment);
        void DeleteAppointment(Appointment appointment);
        Task <IEnumerable<AppointmentDto?>> GetAllReserverdAppointmentsToDoctor(AppointmentParamters paramters, string doctorId , DateOnly date);
        Task <IEnumerable<AppointmentDto?>> GetReserverdAppointmentsToDoctor(string doctorId , DateOnly date);
        Task<IEnumerable<AppointmentDto?>> GetReserverdAppointmentsByDoctorId(AppointmentParamters paramters,string doctorId);
        Task<Appointment?> GetAppointAsync(Guid Id);
        Task<AppointmentDto?> GetAppointmentById(Guid Id);
        Task<IEnumerable<AppointmentDto?>> GetReserverdAppointmentsByDoctorId(string doctorId);
        //public bool IsAppointmentAvailable(IEnumerable<AppointmentDto> existingAppointments, TimeSpan requestedStartTime);


    }
}
