using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Physico_BAL.Contracts;
using Physico_BAL.DTO;
using Physico_BAL.Repoisitories;
using Physico_BAL.RequestFeatures;
using Physico_DAL.Models;

namespace Physico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IRepositoryManager _repository;
        private readonly IEmailService _emailService;

        public AppointmentsController(IMapper mapper, IRepositoryManager repository, IEmailService emailService)
        {
            _mapper = mapper;
            _repository = repository;
            _emailService = emailService;
        }
        [HttpPost("Reservation")]
        public async Task<IActionResult> CreateReservation([FromBody] AppointmentForCreationDto appointmentDto)
        {
            if(!ModelState.IsValid || appointmentDto is null)
                return BadRequest($"Something went wrong when filling the form : {ModelState}");
            var doctor = await _repository.Doctor.GetDoctorById(appointmentDto.DoctorId!, false);
            if (doctor is null)
                return NotFound($"Doctor with ID {appointmentDto.DoctorId!} Doesn't exist in the database");
            try {
                var appointment = _mapper.Map<Appointment>(appointmentDto);
                _repository.Appointment.CreateAppointment(appointment);
                await _emailService.SendReservationEmail(appointmentDto.PatientEmail!, appointmentDto.PatientName!, appointmentDto.Time, appointmentDto.Date);
                await _repository.SaveChangesAsync();
                var appointmentToReturn = _mapper.Map<AppointmentDto>(appointment);
                return Ok(appointmentToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while reservation. {ex.Message}");
            }
        }
        //[HttpGet("Reserved/{doctorId}/{date}")]
        //public async Task<IActionResult> GetReservedAppointments([FromQuery]AppointmentParamters paramters ,string doctorId)
        //{
        //    var appointments = await _repository.Appointment.GetAllReserverdAppointmentsToDoctor(paramters,doctorId , paramters.Date);
        //    if (appointments is null)
        //        return NotFound("There are no reserved appointments yet");
        //    var appointmentsToReturn = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        //    return Ok(appointmentsToReturn);
        //}
        [HttpGet("Reserved/{doctorId}")]
        public async Task<IActionResult> GetReservedAppointmentsforDoctor([FromQuery] AppointmentParamters paramters ,string doctorId)
        {
            if (paramters.Date == default(DateOnly))
            {
                paramters.Date = DateOnly.FromDateTime(DateTime.Now.Date);
            }
            var appointments = await _repository.Appointment.GetReserverdAppointmentsByDoctorId(paramters,doctorId);
            if (appointments is null)
                return NotFound("There are no reserved appointments yet");
            var appointmentsToReturn = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            return Ok(appointmentsToReturn);
        }
        [HttpDelete("appointmentId")]
        public async Task<IActionResult> DeleteAppointment(Guid appointmentId)
        {
            var appointment = await _repository.Appointment.GetAppointAsync(appointmentId);
            if (appointment is null)
                return NotFound($"Appointment With ID {appointmentId} doesnt' exist in the database");
             _repository.Appointment.DeleteAppointment(appointment);
            await _repository.SaveChangesAsync();
            return Ok(new { Message = $"Appointment with ID {appointmentId} has been deleted successfully!" });
        }
    }
}
