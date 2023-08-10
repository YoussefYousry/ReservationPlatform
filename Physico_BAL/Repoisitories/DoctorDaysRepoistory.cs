using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Physico_BAL.Contracts;
using Physico_BAL.DTO;
using Physico_DAL.Data;
using Physico_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.Repoisitories
{
    public class DoctorDaysRepoistory : RepositoryBase<DoctorDays> , IDoctorDaysRepoistory
    {
        private readonly IMapper _mapper;
        private IAppointmentRepoistory _appointment;
        public DoctorDaysRepoistory(AppDbContext context , IMapper mapper, IAppointmentRepoistory appointment) : base(context)
        {
            _mapper = mapper;
            _appointment = appointment;

        }
        public void Create(List<DoctorDays> doctorDays) 
        {
            _context.Set<DoctorDays>().AddRange(doctorDays);
        }
        public void DeleteDoctorDay(DoctorDays doctorDays) =>Delete(doctorDays);
        public void UpdateDoctorDay(DoctorDays doctorDays) => Update(doctorDays);

        public async Task<DoctorDaysDto?> GetDoctorDay(string doctorId, DateOnly day)
        {
            var doctorDay = await FindByCondition(d => d.DoctorId == doctorId && d.AppointmentDay == day, trackChanges: false)
                .Include(d => d.DoctorObject)
                .Select(c => new { id = c.Id, day = c.AppointmentDay, start = c.Start, end = c.End })
                .FirstOrDefaultAsync();

            var duration = TimeSpan.FromHours(1);
            var times = new List<string>();

            for (var time = doctorDay!.start; time <= doctorDay.end; time += duration)
            {
                var isAvailable = await IsAppointmentAvailable(doctorId, doctorDay.day, time);
                if (isAvailable)
                {
                    times.Add(time.ToString());
                }
            }

            var doctorDayVM = new DoctorDaysDto
            {
                DoctorId = doctorId,
                Id = doctorDay.id,
                AppointmentDay = doctorDay.day,
                Times = times
            };

            return doctorDayVM;
        }

        public async Task<IEnumerable<DoctorDaysDto?>> GetDoctorDays(string doctorId)
        {
            var doctorDays = await FindByCondition(d => d.DoctorId== doctorId, trackChanges: false)
                .Include(d => d.DoctorObject)
                .Select( c => new {id = c.Id , day = c.AppointmentDay , start = c.Start , end = c.End})
                .ToListAsync();

            var doctorDaysVM = new List<DoctorDaysDto>();
            var duration = TimeSpan.FromHours(1);
            foreach (var doctorDay in doctorDays)
            {
                var times = new List<string>();
                for(var time = doctorDay.start; time<= doctorDay.end; time+= duration)
                {
                    var isAvailable = await IsAppointmentAvailable(doctorId, doctorDay.day, time);
                    if (isAvailable)
                    {
                        times.Add(time.ToString());
                    }
                }
                doctorDaysVM.Add(new DoctorDaysDto
                {
                    DoctorId = doctorId,
                    Id = doctorDay.id,
                    AppointmentDay = doctorDay.day,
                    Times = times

                });
            }
            return doctorDaysVM;
        }
        public async Task<DoctorDays?> GetDayById(int id, bool trackChanges)
            => await FindByCondition(i => i.Id == id, trackChanges: trackChanges)
            .FirstOrDefaultAsync();
        private async Task<bool> IsAppointmentAvailable(string doctorId, DateOnly appointmentDay, TimeSpan appointmentTime)
        {
            var existingAppointments = await _appointment.GetReserverdAppointmentsToDoctor(doctorId, appointmentDay);
    
            foreach (var existingAppointment in existingAppointments)
            {
                if (appointmentTime < existingAppointment!.Time.Add(TimeSpan.FromHours(1)) && existingAppointment.Time< appointmentTime.Add(TimeSpan.FromHours(1)))
                {
                    return false; 
                }
            }
            return true; 
        }
    }
}
