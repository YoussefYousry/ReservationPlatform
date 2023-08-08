using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Physico_BAL.Contracts;
using Physico_BAL.DTO;
using Physico_DAL.Models;

namespace Physico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IFilesRepository _filesRepository;

        public DoctorsController(IRepositoryManager repositoryManager, IMapper mapper, IFilesRepository filesRepository)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _filesRepository = filesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _repositoryManager.Doctor.GetAllDoctors(trackChanges: false);
            if (doctors is null)
            {
                return NotFound("There Are no Doctors in the system yet!");
            }
            var doctorsToReturn = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            return Ok(doctorsToReturn);
        }
        [HttpGet("{doctorId}")]
        public async Task<IActionResult> GetDoctor(string doctorId)
        {
            var doctor = await _repositoryManager.Doctor.GetDoctorById(doctorId,trackChanges: false);
            if (doctor is null)
            {
                return NotFound($"Doctor with ID {doctorId} doesn't exist in the database");
            }
            return Ok(doctor);
        }
        [HttpPost("UploadImage"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadImage([FromForm] ImageForUploadViewModel img, string userId)
        {
            var doctor = await _repositoryManager.Doctor.GetDoctorById(userId, trackChanges: false);
            if (doctor is null)
            {
                return NotFound($"Doctor with ID: {userId} doesn't exist in the database ");
            }
            _filesRepository.UploadImageToUser(userId, img.File!);
            await _repositoryManager.SaveChangesAsync();
            return StatusCode(201, "Image Uploaded Successfully");
        }
        [HttpGet("Image/{userId}")]
        public async Task<IActionResult> GetImage(string userId)
        {
            try
            {
                FileStream file = await _filesRepository.GetImageToUser(userId);
                return new FileStreamResult(file, "image/png");
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest($"User doesn't have an Image : {ex}");
            }

        }

        [HttpPost("DoctorDays")]
        public async Task<IActionResult> AddDoctorDays(string doctorId , [FromBody] List<DoctorDaysForCreationDto> daysDto)
        {
            if (doctorId.IsNullOrEmpty())
                return BadRequest("Doctor Id is null or empty");
            var doctor = await _repositoryManager.Doctor.GetDoctorById(doctorId, false);
            if (doctor is null)
                return BadRequest("Invalid Doctor Id");
            try
            {
                var days = _mapper.Map<List<DoctorDays>>(daysDto);
                _repositoryManager.DoctorDays.Create(days);
                await _repositoryManager.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
                {
                    return BadRequest("An Appointment Day with the same Date is already created.");
                }
                return StatusCode(400, new { message = "An error occurred while creating the AppointmentDays." });
            }
        }
        [HttpPut("DoctorDays/{dayId}")]
        public async Task<IActionResult> UpdateDoctorDay([FromBody]DoctorDaysForUpdateDto doctorDaysDto  ,int dayId)
        {
            var doctorDay = await _repositoryManager.DoctorDays.GetDayById(dayId, true);
            if (doctorDay is null)
                return BadRequest("Invalid Doctor Day");
            _mapper.Map(doctorDaysDto,doctorDay);
            await _repositoryManager.SaveChangesAsync();
            return Ok(new { day = doctorDay!.AppointmentDay, Message = "Doctor Day has been updated successfully!" });
        }

        [HttpDelete("DoctorDay/{dayId}")]
        public async Task<IActionResult> DeleteDayById(int dayId)
        {
            var doctorDay = await _repositoryManager.DoctorDays.GetDayById(dayId,false);
            if (doctorDay is null)
                return NotFound($"Day with Id {dayId} doesn't exist in the database");
            _repositoryManager.DoctorDays.DeleteDoctorDay(doctorDay);
            await _repositoryManager.SaveChangesAsync();
            return Ok(new { Message = $"Day with ID {dayId} has been deleted successfully!" });
        }

        [HttpGet("DoctorDays/{doctorId}")]
        public async Task<IActionResult> GetDoctorDays(string doctorId)
        {
            var doctor = await _repositoryManager.Doctor.GetDoctorById(doctorId, false);
            if (doctor is null)
                return BadRequest("Invalid Doctor Id");
            var days = await _repositoryManager.DoctorDays.GetDoctorDays(doctorId);
            return Ok(days);
        }
        [HttpGet("DoctoryDays/{doctorId}/{day}")]
        public async Task<IActionResult> GetDoctorDay(string doctorId , DateOnly day)
        {
            var doctor = await _repositoryManager.Doctor.GetDoctorById(doctorId, false);
            if (doctor is null)
                return BadRequest("Invalid Doctor Id");
            try { 
            var dayToReturn = await _repositoryManager.DoctorDays.GetDoctorDay(doctorId, day);
            if (dayToReturn is null)
                return NotFound($"Invalid DoctorId or Day");
            return Ok(dayToReturn);
            }
            catch
            {
                return BadRequest("Invalid DoctorId or Day");
            }
        }

    }
}
