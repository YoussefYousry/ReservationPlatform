using Physico_BAL.DTO;
using Physico_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.Contracts
{
    public interface IDoctorDaysRepoistory
    {
        Task<IEnumerable<DoctorDaysDto?>> GetDoctorDays(string doctorId);
        Task<DoctorDaysDto?> GetDoctorDay(string doctorId,DateOnly day);
        void Create(List<DoctorDays> doctorDays);
        void DeleteDoctorDay(DoctorDays doctorDays);
        void UpdateDoctorDay(DoctorDays doctorDays);
        Task<DoctorDays?> GetDayById(int id, bool trackChanges);


    }
}
