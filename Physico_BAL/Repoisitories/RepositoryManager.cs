using AutoMapper;
using Physico_BAL.Contracts;
using Physico_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.Repoisitories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _context;
        private IAppointmentRepoistory _appointment;
        private IDoctorRepoistory _doctor;
        private IDoctorDaysRepoistory _doctorDays;
        private IFilesManager _filesManager;
        private IMapper _mapper;

        public RepositoryManager(AppDbContext context
            ,IDoctorDaysRepoistory doctorDays
            ,IDoctorRepoistory doctor
            ,IAppointmentRepoistory appointment
            ,IFilesManager filesManager
            ,IMapper mapper
            )
        {
            _appointment = appointment;
            _context = context;
            _doctor = doctor;
            _doctorDays = doctorDays;
            _filesManager = filesManager;
            _mapper = mapper;
        }
        public IDoctorRepoistory Doctor
        {
            get
            {
                _doctor ??= new DoctorRepoistory(_context, _filesManager, _mapper);
                return _doctor;
            }
        }
        public IAppointmentRepoistory Appointment
        {
            get
            {
                _appointment ??= new AppointmentRepoistory(_context);
                return _appointment;
            }
        }
        public IDoctorDaysRepoistory DoctorDays
        {
            get
            {
                _doctorDays ??= new DoctorDaysRepoistory(_context);
                return _doctorDays;
            }
        }
        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
