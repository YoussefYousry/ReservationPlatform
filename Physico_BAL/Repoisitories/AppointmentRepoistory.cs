using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Physico_BAL.Contracts;
using Physico_BAL.DTO;
using Physico_BAL.RequestFeatures;
using Physico_BAL.RepoisitoriesExtensions;
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
        private readonly IMapper _mapper;
        public AppointmentRepoistory(AppDbContext context , IMapper mapper):base(context)
        {
            _mapper = mapper;
        }
        public void CreateAppointment(Appointment appointment)
        {
            appointment.IsAvailable = false;
            Create(appointment);
        }
        public void DeleteAppointment(Appointment appointment) => Delete(appointment);
        
        public async Task<IEnumerable<AppointmentDto?>> GetAllReserverdAppointmentsToDoctor(AppointmentParamters paramters ,string doctorId , DateOnly date)
            => await FindByCondition(a => a.DoctorId == doctorId && a.Date==date && a.IsAvailable == false, trackChanges:false)
            .Search(paramters.SearchTerm! , paramters.Date)
            .Skip((paramters.PageNumber-1)  * paramters.PageSize)
            .Take(paramters.PageSize)
            .OrderBy(e => e.Time)
            .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<IEnumerable<AppointmentDto?>> GetReserverdAppointmentsToDoctor(string doctorId, DateOnly date)
            => await FindByCondition(a => a.DoctorId == doctorId && a.Date == date && a.IsAvailable == false, trackChanges: false)
            .OrderBy(e => e.Time)
            .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<IEnumerable<AppointmentDto?>> GetReserverdAppointmentsByDoctorId(AppointmentParamters paramters, string doctorId)
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            return await FindByCondition(a => a.DoctorId == doctorId && a.IsAvailable == false && a.Date >= currentDate, trackChanges: false)
            .Search(paramters.SearchTerm!, paramters.Date)
            .Skip((paramters.PageNumber - 1) * paramters.PageSize)
            .Take(paramters.PageSize)
            .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider)
            .OrderBy(e => e.Date)
            .ToListAsync();
        }
        public async Task<IEnumerable<AppointmentDto?>> GetReserverdAppointmentsByDoctorId( string doctorId)
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            return await FindByCondition(a => a.DoctorId == doctorId && a.IsAvailable == false && a.Date >= currentDate, trackChanges: false)
            .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider)
            .OrderBy(e => e.Date)
            .ToListAsync();
        }
        public async Task<AppointmentDto?> GetAppointmentById(Guid Id)
            => await FindByCondition(a => a.Id == Id, trackChanges: false)
            .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        public async Task<Appointment?> GetAppointAsync(Guid Id)
            => await FindByCondition(a => a.Id == Id, false)
            .FirstOrDefaultAsync();
        //public bool IsAppointmentAvailable(DoctorDaysDto existingAppointments, TimeSpan requestedStartTime)
        //{
        //    foreach (var existingAppointment in existingAppointments)
        //    {
        //        if (existingAppointment.IsAvailable == true && existingAppointment.Time == requestedStartTime)
        //        {
        //            return true; // Found an available appointment with the requested start time
        //        }
        //    }
        //    return false; // No available appointment with the requested start time found
        //}
    }
}
